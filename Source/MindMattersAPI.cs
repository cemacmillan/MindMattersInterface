using RimWorld;
using Verse;
using System.Reflection;
using UnityEngine;
using HarmonyLib;

namespace MindMattersInterface;

public static class MindMattersAPI
{
    //private static readonly PropertyInfo ReadyToParleyProperty = GameComponentType?.GetProperty("ReadyToParley", BindingFlags.Public | BindingFlags.Instance);
    private static readonly bool ReadyToParleyStatus = MindMatters.MindMattersMod.ReadyToParley;

    public const bool EnableLogging = true;

    static MindMattersAPI()
    {
        if (ReadyToParleyStatus == false)
        {
            // MMToolkit.GripeOnce("[MindMattersAPI] API initialized while 'ReadyToParley' is");
        }
    }

// Cached readiness state
    private static readonly bool isMindMattersLoaded =
        ModsConfig.IsActive("cem.mindmatters") || ModsConfig.IsActive("cem.mindmatterspr");

    private static bool cachedIsReady = false;

// Check readiness, optionally using cachedIsReady
    private static bool IsMindMattersReady()
    {
        if (!isMindMattersLoaded)
        {
            MMToolkit.GripeOnce("No Mind Matters candidate mod loaded.");
            return false;
        }

        try
        {
            cachedIsReady = MindMatters.MindMattersMod.ReadyToParley;
            // MMToolkit.DebugLog($"cachedIsReady: {cachedIsReady} from ReadyToParleyProperty.");
            return cachedIsReady;
        }
        catch (Exception ex)
        {
            MMToolkit.GripeOnce($"[MindMattersAPI] Error checking readiness: {ex.Message}");
            cachedIsReady = false;
            return false;
        }
    }


    // ----------------------------------------------------------------------
    // Dynamic Needs Management
    // ----------------------------------------------------------------------
    public static NeedDef? GetNeedDef(string needDefName)
    {
        return DynamicNeeds.GetNeedDefFromName(needDefName);
    }

    public static bool HasDynamicNeed(Pawn pawn, string needDefName)
    {
        if (pawn == null || string.IsNullOrEmpty(needDefName))
        {
            MMToolkit.DebugWarn("[MindMattersAPI] Invalid parameters for HasDynamicNeed.");
            return false;
        }

        var needDef = DefDatabase<NeedDef>.GetNamedSilentFail(needDefName);
        if (needDef == null)
        {
            MMToolkit.DebugWarn($"[MindMattersAPI] NeedDef '{needDefName}' not found.");
            return false;
        }

        Need? need = pawn.needs.TryGetNeed(needDef);
        return need is IDynamicNeed;
    }

    /// <summary>
    /// Updates the baseline contribution for a given Need, triggered by external events such as onEquip/onUnequip.
    /// </summary>
    /// <param name="pawn">The pawn whose Need is being updated.</param>
    /// <param name="needDefName">The defName of the Need to update.</param>
    /// <param name="contribution">The contribution value to add or remove from the baseline.</param>
    /// <param name="isAdding">Whether the contribution is being added (true) or removed (false).</param>
    [JetBrains.Annotations.UsedImplicitlyAttribute]
    public static void UpdateNeedBaselineContribution(Pawn pawn, string needDefName, float contribution, bool isAdding)
    {
        if (pawn == null || string.IsNullOrEmpty(needDefName))
        {
            MMToolkit.GripeOnce("[MindMattersAPI] Invalid parameters for UpdateNeedBaselineContribution.");
            return;
        }

        var needDef = DefDatabase<NeedDef>.GetNamedSilentFail(needDefName);
        if (needDef == null)
        {
            MMToolkit.GripeOnce($"[MindMattersAPI] NeedDef '{needDefName}' not found.");
            return;
        }

        var need = pawn.needs.TryGetNeed(needDef);
        if (need == null)
        {
            MMToolkit.GripeOnce($"[MindMattersAPI] Pawn '{pawn.LabelShort}' does not have the need '{needDefName}'.");
            return;
        }

        if (need is IDynamicNeed dynamicNeed)
        {
            try
            {
                if (isAdding)
                {
                    dynamicNeed.AddSatisfactionContribution(contribution);
                    MMToolkit.DebugLog(
                        $"[MindMattersAPI] Added {contribution} to baseline of '{needDefName}' for {pawn.LabelShort}.");
                }
                else
                {
                    dynamicNeed.RemoveSatisfactionContribution(contribution);
                    MMToolkit.DebugLog(
                        $"[MindMattersAPI] Removed {contribution} from baseline of '{needDefName}' for {pawn.LabelShort}.");
                }
            }
            catch (Exception ex)
            {
                MMToolkit.GripeOnce(
                    $"[MindMattersAPI] Error updating baseline contribution for '{needDefName}': {ex.Message}");
            }
        }
        else
        {
            MMToolkit.GripeOnce($"[MindMattersAPI] Need '{needDefName}' is not a valid IDynamicNeed.");
        }
    }

    /// <summary>
    /// Applies a temporary satisfaction value to a Need, typically fired on rare ticks or temporary events.
    /// </summary>
    /// <param name="pawn">The pawn whose Need is being satisfied.</param>
    /// <param name="needDefName">The defName of the Need to satisfy.</param>
    /// <param name="amount">The amount to add to the Need's satisfaction level.</param>
    /// <param name="satisfyToMax">Whether to cap the Need's satisfaction level to 100%.</param>
    [JetBrains.Annotations.UsedImplicitlyAttribute]
    public static void SatisfyDynamicNeed(Pawn pawn, string needDefName, float amount, bool satisfyToMax = false)
    {
        if (pawn == null || string.IsNullOrEmpty(needDefName))
        {
            MMToolkit.GripeOnce("[MindMattersAPI] Invalid parameters for SatisfyDynamicNeed.");
            return;
        }

        var needDef = DefDatabase<NeedDef>.GetNamedSilentFail(needDefName);
        if (needDef == null)
        {
            MMToolkit.GripeOnce($"[MindMattersAPI] NeedDef '{needDefName}' not found.");
            return;
        }

        var need = pawn.needs.TryGetNeed(needDef);
        if (need == null)
        {
            MMToolkit.GripeOnce($"[MindMattersAPI] Pawn '{pawn.LabelShort}' does not have the need '{needDefName}'.");
            return;
        }

        if (need is IDynamicNeed dynamicNeed)
        {
            try
            {
                if (satisfyToMax)
                {
                    // Set CurLevel to max by boosting tick satisfaction
                    dynamicNeed.ApplyTickSatisfaction(dynamicNeed.MaxSatisfaction - dynamicNeed.CurLevel);
                }
                else
                {
                    // Adjust tick satisfaction by the specified amount
                    dynamicNeed.ApplyTickSatisfaction(amount);
                }

                MMToolkit.DebugLog(
                    $"[MindMattersAPI] Adjusted '{needDefName}' for {pawn.LabelShort} by {amount}, new level: {dynamicNeed.CurLevel}.");
            }
            catch (Exception ex)
            {
                MMToolkit.GripeOnce(
                    $"[MindMattersAPI] Error satisfying Need '{needDefName}' dynamically: {ex.Message}");
            }
        }
        else
        {
            MMToolkit.GripeOnce($"[MindMattersAPI] Need '{needDefName}' is not a valid IDynamicNeed.");
        }
    }

    [JetBrains.Annotations.UsedImplicitlyAttribute]
    public static bool NotifyEventForDynamicNeeds(Pawn pawn, string eventType)
    {
        if (pawn == null || string.IsNullOrEmpty(eventType))
        {
            MMToolkit.DebugWarn("[MindMattersAPI] Invalid parameters for NotifyEventForDynamicNeeds.");
            return false;
        }

        var dynamicNeeds = pawn.needs.AllNeeds.OfType<IDynamicNeed>().ToList();
        if (!dynamicNeeds.Any())
        {
            MMToolkit.GripeOnce($"[MindMattersAPI] No dynamic needs found for {pawn.LabelShort}.");
            return false;
        }

        foreach (var need in dynamicNeeds)
        {
            need.HandleEvent(eventType);
        }

        return true;
    }

    // ----------------------------------------------------------------------
    // Experience System Integration
    // ----------------------------------------------------------------------

    private static IMindMattersExperienceComponent? GetMindMattersExperienceComponent()
    {
        if (!IsMindMattersReady())
        {
            MMToolkit.GripeOnce("[MindMattersAPI] Mind Matters mod is not active.");
            return null;
        }

        try
        {
            Type? type = AccessTools.TypeByName("MindMatters.MindMattersExperienceComponent");
            object? instance = type?.GetProperty("Instance", BindingFlags.Public | BindingFlags.Static)?.GetValue(null);

            if (instance == null)
            {
                MMToolkit.GripeOnce("[MindMattersAPI] MindMattersExperienceComponent.Instance is null.");
            }

            return instance as IMindMattersExperienceComponent;
        }
        catch (Exception ex)
        {
            MMToolkit.GripeOnce($"[MindMattersAPI] Error accessing ExperienceComponent: {ex.Message}");
            return null;
        }
    }

    public static void AddExperience(Pawn pawn, Experience experience)
    {
        IMindMattersExperienceComponent? instance = GetMindMattersExperienceComponent();
        if (instance == null)
        {
            // MMToolkit.GripeOnce("[MindMattersAPI] ExperienceComponent.Instance not found.");
            return;
        }

        instance.AddExperience(pawn, experience);
    }

    public static void NotifyExperience(Pawn pawn, string eventType, ExperienceValency valency,
        HashSet<string> flags = null)
    {
        if (!IsMindMattersReady())
        {
            MMToolkit.GripeOnce("[MindMattersAPI] Mind Matters is not active.");
            return;
        }

        IMindMattersExperienceComponent? instance = GetMindMattersExperienceComponent();
        if (instance == null)
        {
            MMToolkit.GripeOnce("[MindMattersAPI] ExperienceComponent not available.");
            return;
        }

        Experience experience = new Experience(eventType, valency) { Flags = flags ?? new HashSet<string>() };
        instance.AddExperience(pawn, experience);

        MMToolkit.DebugLog($"[MindMattersAPI] Recorded experience '{eventType}' ({valency}) for {pawn.LabelShort}.");
    }

    // ----------------------------------------------------------------------
    // Debugging Utilities
    // ----------------------------------------------------------------------

    public static void DebugLogDynamicNeeds(Pawn pawn)
    {
        List<string> needs = pawn.needs.AllNeeds.OfType<IDynamicNeed>().Select(need => need.NeedDefName).ToList();
        MMToolkit.DebugLog($"[MindMattersAPI] {pawn.LabelShort}'s dynamic needs: {string.Join(", ", needs)}");
    }
}
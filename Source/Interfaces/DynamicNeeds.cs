using Verse;
using RimWorld;
using System.Reflection;
using UnityEngine;
using HarmonyLib;



namespace MindMattersInterface;

    /// <summary>
    /// Exposes functionality for working with dynamic needs.
    /// </summary>
    public static class DynamicNeeds
    {
        
        // glue imports from Mind Matters - Types
        /*
        static Type DynamicNeedPropertiesType = 
            typeof(MindMatters.MindMattersMod).GetNestedType("DynamicNeedProperties", BindingFlags.NonPublic);
            
            - If we ever have to do this via reflection, it looks like the above.
        */
        
        // Doing stuff like direct API calls
        /// <summary>
        /// Updates the baseline contribution for a dynamic need.
        /// </summary>
        public static void UpdateBaseline(Pawn pawn, string needDefName, float contribution, bool isAdding)
        {
            if (pawn == null || string.IsNullOrEmpty(needDefName))
            {
                MMToolkit.GripeOnce("[DynamicNeeds] Invalid parameters for UpdateBaseline.");
                return;
            }

            MindMattersAPI.UpdateNeedBaselineContribution(pawn, needDefName, contribution, isAdding);
        }

        /// <summary>
        /// Satisfies a dynamic need with a specified amount.
        /// </summary>
        public static void SatisfyNeed(Pawn pawn, string needDefName, float amount, bool satisfyToMax = false)
        {
            if (pawn == null || string.IsNullOrEmpty(needDefName))
            {
                MMToolkit.GripeOnce("[DynamicNeeds] Invalid parameters for SatisfyNeed.");
                return;
            }

            MindMattersAPI.SatisfyDynamicNeed(pawn, needDefName, amount, satisfyToMax);
        }

        /// <summary>
        /// Records an experience for a pawn.
        /// </summary>
        public static void NotifyExperience(Pawn pawn, string eventType, ExperienceValency valency,
            HashSet<string> tags = null)
        {
            if (pawn == null || string.IsNullOrEmpty(eventType))
            {
                MMToolkit.GripeOnce("[DynamicNeeds] Invalid parameters for NotifyExperience.");
                return;
            }

            MindMattersAPI.NotifyExperience(pawn, eventType, valency, tags);
        }
        
        // ---------------------------------------------
        // Retrieve Need Definitions
        // ---------------------------------------------

        /// <summary>
        /// Retrieve a NeedDef from the registry or DefDatabase using its defName.
        /// Mods using the API should use this and related methods in preference to direct access to DefDatabase due to
        /// the more complex structure and properties of DynamicNeeds, and that in the future we may allow Need
        /// creation without any accompanying XML for passing fancies.
        /// </summary>
        public static NeedDef? GetNeedDefFromName(string defName)
        {
            return MindMatters.DynamicNeedsRegistry.GetNeedDefFromName(defName);
        }

        /// <summary>
        /// Retrieve a NeedDef from the registry using its associated Type.
        /// </summary>
        public static NeedDef? GetNeedDef(Type needType)
        {
            return MindMatters.DynamicNeedsRegistry.GetNeedDef(needType);
        }

        /// <summary>
        /// Retrieve a NeedDef from the registry using a DynamicNeedsBitmap.
        /// </summary>
        public static NeedDef? GetNeedDefFromBitmap(MindMatters.DynamicNeedsBitmap bitmap)
        {
            return MindMatters.DynamicNeedsRegistry.GetNeedDefForBitmap(bitmap);
        }

        // ---------------------------------------------
        // Retrieve Dynamic Need Properties
        // ---------------------------------------------

        /// <summary>
        /// Retrieve all properties for dynamic needs.
        /// </summary>
        public static IEnumerable<MindMatters.DynamicNeedProperties> GetAllDynamicNeeds()
        {
            return MindMatters.DynamicNeedsRegistry.GetAllProperties();
        }

        /// <summary>
        /// Retrieve properties for a specific DynamicNeed by Type.
        /// </summary>
        public static MindMatters.DynamicNeedProperties? GetProperties(Type needType)
        {
            return MindMatters.DynamicNeedsRegistry.GetProperties(needType);
        }

        /// <summary>
        /// Retrieve properties for a DynamicNeed using its bitmap.
        /// </summary>
        public static MindMatters.DynamicNeedProperties? GetPropertiesForBitmap(MindMatters.DynamicNeedsBitmap bitmap)
        {
            return MindMatters.DynamicNeedsRegistry.GetPropertiesForBitmap(bitmap);
        }

        // ---------------------------------------------
        // DynamicNeed State Management
        // ---------------------------------------------

        /// <summary>
        /// Retrieve the global state of a DynamicNeed using its bitmap.
        /// </summary>
        public static MindMatters.DynamicNeedState? GetGlobalState(MindMatters.DynamicNeedsBitmap bitmap)
        {
            if (MindMatters.DynamicNeedsRegistry.TryGetGlobalState(bitmap, out var state))
            {
                return state;
            }
            return null;
        }

        /// <summary>
        /// Set the global state of a DynamicNeed using its bitmap.
        /// </summary>
        public static void SetGlobalState(MindMatters.DynamicNeedsBitmap bitmap, MindMatters.DynamicNeedState state)
        {
            MindMatters.DynamicNeedsRegistry.   SetGlobalState(bitmap, state);
        }

        // ---------------------------------------------
        // Utility Methods for Modders
        // ---------------------------------------------

        /// <summary>
        /// Determine if a specific pawn should have a given DynamicNeed.
        /// </summary>
        public static bool ShouldPawnHaveNeed(Pawn pawn, Type needType)
        {
            return MindMatters.DynamicNeedsRegistry.ShouldPawnHaveNeed(pawn, needType);
        }

        /// <summary>
        /// Retrieve all dynamic needs in a specific category.
        /// </summary>
        public static IEnumerable<MindMatters.DynamicNeedProperties> GetNeedsForCategory(MindMatters.DynamicNeedCategory? category = null)
        {
            return MindMatters.DynamicNeedsRegistry.GetNeedsForCategory(category);
        }

        /// <summary>
        /// Return the bitmap associated with a DynamicNeed Type.
        /// </summary>
        public static MindMatters.DynamicNeedsBitmap GetBitmapForNeed(Type needClass)
        {
            return MindMatters.DynamicNeedsRegistry.GetBitmapForNeed(needClass);
        }
    }
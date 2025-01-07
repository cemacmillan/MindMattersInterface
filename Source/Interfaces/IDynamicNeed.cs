using System.Collections.Generic;
using RimWorld;
using Verse;

namespace MindMattersInterface;

public interface IDynamicNeed
{
    // Current level of the need, clamped between 0 and 1
    float CurLevel { get; set; }
    
    float MaxSatisfaction { get; set; }

    // DefName of the need for identification
    string NeedDefName { get; }

    // Indicates whether the need is temporarily suppressed
    bool IsSuppressed { get; set; }

    // Handles events that may influence the need
    void HandleEvent(string eventName);

    // Adds a baseline contribution to the need (e.g., from apparel or traits)
    void AddSatisfactionContribution(float contribution);

    // Removes a baseline contribution from the need
    void RemoveSatisfactionContribution(float contribution);
    
    void ApplyTickSatisfaction(float contribution);

    // Updates the baseline contribution for the need based on external factors
    void UpdateNeedBaselineContribution(Pawn pawn, string needDefName, float contribution, bool isAdding);

    // Optional metadata for additional information
    Dictionary<string, object> GetMetadata();
}
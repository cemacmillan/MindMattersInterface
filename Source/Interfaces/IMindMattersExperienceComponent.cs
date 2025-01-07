using RimWorld;
using Verse;
using System.Collections.Generic;

namespace MindMattersInterface;

public interface IMindMattersExperienceComponent
{
    // ----------------------------------------------------------------------
    // Experience Management
    // ----------------------------------------------------------------------

    /// <summary>
    /// Adds an experience to the specified pawn.
    /// </summary>
    /// <param name="pawn">The pawn to whom the experience is added.</param>
    /// <param name="experience">The experience object containing the event type, valency, and optional flags.</param>
    void AddExperience(Pawn pawn, Experience experience);

    /// <summary>
    /// Removes an experience from the specified pawn.
    /// </summary>
    /// <param name="pawn">The pawn whose experience is to be removed.</param>
    /// <param name="eventType">The event type of the experience to remove.</param>
    /// <returns>True if the experience was successfully removed; otherwise, false.</returns>
    bool RemoveExperience(Pawn pawn, string eventType);

    /// <summary>
    /// Retrieves all experiences associated with a specific pawn.
    /// </summary>
    /// <param name="pawn">The pawn whose experiences are being queried.</param>
    /// <returns>A list of experiences for the specified pawn.</returns>
    List<Experience> GetPawnExperiences(Pawn pawn);

    /// <summary>
    /// Expires old experiences for all pawns, removing those that exceed the expiration threshold.
    /// </summary>
    /// <param name="expireThreshold">The expiration timestamp threshold for experiences.</param>
    void ExpireOldExperiences(int expireThreshold);

    // ----------------------------------------------------------------------
    // Event Notifications
    // ----------------------------------------------------------------------

    /// <summary>
    /// Event triggered when a new experience is added to a pawn.
    /// </summary>
    event Action<Pawn, Experience> OnExperienceAdded;

    /// <summary>
    /// Event triggered when an experience is removed from a pawn.
    /// </summary>
    event Action<Pawn, Experience> OnExperienceRemoved;

    // ----------------------------------------------------------------------
    // Specialized Experience Handlers
    // ----------------------------------------------------------------------

    /// <summary>
    /// Checks if the specified thought is therapy-related or tied to similar experiences.
    /// </summary>
    /// <param name="thought">The thought to check.</param>
    /// <returns>True if the thought is therapy-related; otherwise, false.</returns>
    bool IsTherapyRelated(Thought thought);

    /// <summary>
    /// Handles the event of a pawn being downed, generating appropriate experiences for the pawn.
    /// </summary>
    /// <param name="pawn">The pawn who has been downed.</param>
    void OnPawnDowned(Pawn pawn);

    /// <summary>
    /// Handles the event of a pawn killing another pawn, generating experiences for the killer.
    /// </summary>
    /// <param name="killer">The pawn who performed the killing.</param>
    void OnPawnKilled(Pawn killer);

    /// <summary>
    /// Handles the event of a colonist's death, generating experiences for all related pawns.
    /// </summary>
    /// <param name="colonist">The colonist who has died.</param>
    void OnColonistDied(Pawn colonist);
}
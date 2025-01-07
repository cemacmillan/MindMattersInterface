using Verse;

namespace MindMattersInterface;

public class Experience
{
    public string EventType { get; set; }
    public ExperienceValency Valency { get; set; }
    public HashSet<string> Flags { get; set; } = new();
    public int Timestamp { get; set; } = Find.TickManager.TicksGame;

    public Experience(string eventType, ExperienceValency valency)
    {
        EventType = eventType;
        Valency = valency;
    }
}
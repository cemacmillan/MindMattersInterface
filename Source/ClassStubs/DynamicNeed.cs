namespace MindMattersInterface;

[JetBrains.Annotations.UsedImplicitly,
 System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1052:Static holder types should be static",
     Justification = "This is a placeholder for dynamic instantiation via reflection.")]
public class DynamicNeed
{
    public static DynamicNeed CreateInstance()
    {
        return new DynamicNeed();
    }

    /*public void AddSatisfactionContribution(float contribution)
    {
        // stub
    }

    public void RemoveSatisfactionContribution(float contribution)
    {
        // stub
    }*/

}
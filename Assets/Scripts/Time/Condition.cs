using System;

/// <summary>
/// One condition for an event (such as "Guidance Computer works)"
/// </summary>
public class Condition
{
    //The function that determines if the Condition is met
    public Func<bool> Heuristic { get; private set; }
    //Text description (for the UI)
    public string Description { get; private set; }

   public Condition(Func<bool> heuristic, string desc)
    {
        Heuristic = heuristic;
        Description = desc;
    }
}

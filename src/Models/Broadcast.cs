namespace AwcHelper.Txt.Models;

public class Broadcast
{
    public string? DayOfTheWeek { get; set; } = null;

    public string? StartTime { get; set; } = null;

    public Broadcast() { }

    public override string? ToString()
    {
        if (DayOfTheWeek == null)
            return null;

        if (StartTime == null)
            return $"{DayOfTheWeek}s";

        return $"{DayOfTheWeek}s at {StartTime} (JST)";
    }
}

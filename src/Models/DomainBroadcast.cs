using Newtonsoft.Json;

namespace AwcHelper.Txt.Models;

public class DomainBroadcast
{
    [JsonProperty("day_of_the_week")]
    public string? DayOfTheWeek { get; } = null;

    [JsonProperty("start_time")]
    public string? StartTime { get; } = null;

    public DomainBroadcast(string? dayOfTheWeek, string? startTime)
    {
        DayOfTheWeek = dayOfTheWeek;
        StartTime = startTime;
    }

    public override string? ToString()
    {
        if (DayOfTheWeek == null)
            return null;

        if (StartTime == null)
            return $"{DayOfTheWeek}s";

        return $"{DayOfTheWeek}s at {StartTime} (JST)";
    }
}

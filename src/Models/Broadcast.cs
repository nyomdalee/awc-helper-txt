using Newtonsoft.Json;

namespace AwcHelper.Txt.Models;

public class Broadcast
{
    [JsonProperty("day_of_the_week")]
    public string? DayOfTheWeek { get; set; } = null;

    [JsonProperty("start_time")]
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

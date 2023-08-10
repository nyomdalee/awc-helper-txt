using Newtonsoft.Json;

namespace AwcHelper.Txt.Models;

public class Status
{
    [JsonProperty("watching")]
    public int? Watching { get; set; }

    [JsonProperty("completed")]
    public int? Completed { get; set; }

    [JsonProperty("on_hold")]
    public int? OnHold { get; set; }

    [JsonProperty("dropped")]
    public int? Dropped { get; set; }

    [JsonProperty("plan_to_watch")]
    public int? PlanToWatch { get; set; }
}

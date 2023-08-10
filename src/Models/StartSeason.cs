using Newtonsoft.Json;

namespace AwcHelper.Txt.Models;

public class StartSeason
{
    [JsonProperty("year")]
    public int? Year { get; set; } = null;

    [JsonProperty("season")]
    public string? Season { get; set; } = null;

    public StartSeason() { }

    public override string? ToString()
    {
        if (Year == null)
            return null;

        return $"{Season} {Year}";
    }
}

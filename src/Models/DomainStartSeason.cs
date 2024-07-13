using Newtonsoft.Json;

namespace AwcHelper.Txt.Models;

public class DomainStartSeason
{
    [JsonProperty("year")]
    public int? Year { get; }

    [JsonProperty("season")]
    public string? Season { get; }

    public DomainStartSeason(int? year, string? season)
    {
        Year = year;
        Season = season;
    }

    public override string? ToString()
    {
        if (Year == null)
            return null;

        return $"{Season} {Year}";
    }
}

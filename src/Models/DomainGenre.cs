using Newtonsoft.Json;

namespace MALSuite.Txt.Models;

public class DomainGenre
{
    [JsonProperty("id")]
    public int? Id { get; }

    [JsonProperty("name")]
    public string? Name { get; }

    public DomainGenre(int? id, string? name)
    {
        Id = id;
        Name = name;
    }
}
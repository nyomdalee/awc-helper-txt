using Newtonsoft.Json;

namespace AwcHelper.Txt.Models;

public class Genre
{
    [JsonProperty("id")]
    public int? Id { get; set; }

    [JsonProperty("name")]
    public string? Name { get; set; }
}
using Newtonsoft.Json;

namespace AwcHelper.Txt.Models;

public class MainPicture
{
    [JsonProperty("medium")]
    public string? Medium { get; set; }

    [JsonProperty("large")]
    public string? Large { get; set; }
}
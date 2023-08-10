using Newtonsoft.Json;

namespace AwcHelper.Txt.Models;

public class Statistics
{
    [JsonProperty("status")]
    public Status? Status { get; set; }

    [JsonProperty("num_list_users")]
    public int? NumListUsers { get; set; }
}

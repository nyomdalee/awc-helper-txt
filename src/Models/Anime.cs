using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace AwcHelper.Txt.Models;

public class Anime
{
    [JsonProperty("id")]
    public int Id { get; set; }

    [JsonProperty("title")]
    public string? Title { get; set; }

    [JsonProperty("main_picture")]
    public MainPicture MainPicture { get; set; } = new MainPicture();

    [JsonProperty("start_date")]
    public string? StartDate { get; set; }
    public DeserializedDate DeserializedStartDate { get; set; } = new DeserializedDate();

    [JsonProperty("end_date")]
    public string? EndDate { get; set; }
    public DeserializedDate DeserializedEndDate { get; set; } = new DeserializedDate();

    [JsonProperty("synopsis")]
    public string? Synopsis { get; set; }

    [JsonProperty("mean")]
    public double? Mean { get; set; }

    [JsonProperty("rank")]
    public int? Rank { get; set; }

    [JsonProperty("popularity")]
    public int? Popularity { get; set; }

    [JsonProperty("genres")]
    public List<Genre> Genres { get; set; } = new List<Genre>();

    [JsonProperty("media_type")]
    public string? MediaType { get; set; }

    [JsonProperty("status")]
    public string? Status { get; set; }

    [JsonProperty("num_episodes")]
    public int? NumEpisodes { get; set; }

    [JsonProperty("start_season")]
    public StartSeason? StartSeason { get; set; } = new StartSeason();

    [JsonProperty("broadcast")]
    public Broadcast? Broadcast { get; set; } = new Broadcast();

    [JsonProperty("source")]
    public string? Source { get; set; }

    [JsonProperty("average_episode_duration")]
    public int? AverageEpisodeDuration { get; set; }

    [JsonProperty("rating")]
    public string? Rating { get; set; }

    [JsonProperty("statistics")]
    public Statistics? Statistics { get; set; } = new Statistics();

    [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
    public DateTime LastUpdated { get; set; }

    public Anime() { }
}

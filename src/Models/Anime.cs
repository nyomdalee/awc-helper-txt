using MongoDB.Bson.Serialization.Attributes;

namespace AwcHelper.Txt.Models;

public class Anime
{
    public int Id { get; set; }

    public string? Title { get; set; }

    public MainPicture MainPicture { get; set; } = new MainPicture();

    public string? StartDate { get; set; }
    public DeserializedDate DeserializedStartDate { get; set; } = new DeserializedDate();

    public string? EndDate { get; set; }
    public DeserializedDate DeserializedEndDate { get; set; } = new DeserializedDate();

    public string? Synopsis { get; set; }

    public double? Mean { get; set; }

    public int? Rank { get; set; }

    public int? Popularity { get; set; }

    public List<Genre> Genres { get; set; } = new List<Genre>();

    public string? MediaType { get; set; }

    public string? Status { get; set; }

    public int? NumEpisodes { get; set; }

    public StartSeason? StartSeason { get; set; } = new StartSeason();

    public Broadcast? Broadcast { get; set; } = new Broadcast();

    public string? Source { get; set; }

    public int? AverageEpisodeDuration { get; set; }

    public string? Rating { get; set; }

    public Statistics? Statistics { get; set; } = new Statistics();

    [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
    public DateTime LastUpdated { get; set; }

    public Anime() { }

    public string? GenresToString()
    {
        if (Genres.Count == 0)
            return null;

        List<string> genreList = new();
        foreach (var genre in Genres)
            genreList.Add(genre.Name);

        return String.Join(", ", genreList);
    }

    public string EpisodeDurationToString()
    {
        if (AverageEpisodeDuration == null)
            return "unknown";

        if (AverageEpisodeDuration < 60)
            return $"{AverageEpisodeDuration} sec";

        if (AverageEpisodeDuration / 60 < 60)
            return $"{AverageEpisodeDuration / 60} min";

        return $"{AverageEpisodeDuration / 3600} h {(AverageEpisodeDuration % 3600) / 60} min";
    }
}
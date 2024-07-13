using MongoDB.Bson.Serialization.Attributes;

namespace AwcHelper.Txt.Models;

public class DomainAnime
{
    public int Id { get; }

    public string? Title { get; }

    public string? Picture { get; }

    public DomainDate StartDate { get; }

    public DomainDate EndDate { get; }

    public string? Synopsis { get; }

    public double? Mean { get; }

    public int? Rank { get; }

    public int? Popularity { get; }

    public IEnumerable<DomainGenre> Genres { get; }

    public IEnumerable<DomainGenre> ExplicitGenres { get; }

    public IEnumerable<DomainGenre> Themes { get; }

    public IEnumerable<DomainGenre> Demographics { get; }

    public string? MediaType { get; }

    public string? Status { get; }

    public int? NumEpisodes { get; }

    public DomainStartSeason? StartSeason { get; }

    public DomainBroadcast? Broadcast { get; }

    public string? Source { get; }

    public int? AverageEpisodeDuration { get; }

    public string? Rating { get; }

    public DomainStatistics? Statistics { get; }

    public int? Favorites { get; }

    public IEnumerable<string>? Producers { get; }

    public IEnumerable<string>? Licensors { get; }

    public IEnumerable<string>? Studios { get; }

    public bool? Approved { get; }

    //public IEnumerable<DomainRelation> Relations { get; }

    public DomainOpEd? Theme { get; }

    public string? AniDb { get; }

    [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
    public DateTime LastUpdated { get; }

    public DomainAnime(int id, string? title, string? picture, DomainDate startDate, DomainDate endDate, string? synopsis, double? mean, int? rank, int? popularity, IEnumerable<DomainGenre> genres, IEnumerable<DomainGenre> explicitGenres, IEnumerable<DomainGenre> themes, IEnumerable<DomainGenre> demographics, string? mediaType, string? status, int? numEpisodes, DomainStartSeason? startSeason, DomainBroadcast? broadcast, string? source, int? averageEpisodeDuration, string? rating, DomainStatistics? statistics, int? favorites, IEnumerable<string>? producers, IEnumerable<string>? licensors, IEnumerable<string>? studios, bool? approved, DomainOpEd? theme, string? aniDb, DateTime lastUpdated)
    {
        Id = id;
        Title = title;
        Picture = picture;
        StartDate = startDate;
        EndDate = endDate;
        Synopsis = synopsis;
        Mean = mean;
        Rank = rank;
        Popularity = popularity;
        Genres = genres;
        ExplicitGenres = explicitGenres;
        Themes = themes;
        Demographics = demographics;
        MediaType = mediaType;
        Status = status;
        NumEpisodes = numEpisodes;
        StartSeason = startSeason;
        Broadcast = broadcast;
        Source = source;
        AverageEpisodeDuration = averageEpisodeDuration;
        Rating = rating;
        Statistics = statistics;
        Favorites = favorites;
        Producers = producers;
        Licensors = licensors;
        Studios = studios;
        Approved = approved;
        //Relations = relations;
        Theme = theme;
        AniDb = aniDb;
        LastUpdated = lastUpdated;
    }


    public string? GenresAndExplicitGenresToString()
    {
        List<string> combinedGenres = new();
        if (Genres.Any())
            combinedGenres.AddRange(Genres.Select(g => g.Name));

        if (ExplicitGenres.Any())
            combinedGenres.AddRange(ExplicitGenres.Select(g => g.Name));

        return string.Join(", ", combinedGenres);
    }

    public string EpisodeDurationToString()
    {
        if (AverageEpisodeDuration == 0 || AverageEpisodeDuration == null)
            return "Unknown";

        if (AverageEpisodeDuration < 60)
            return $"{AverageEpisodeDuration} sec";

        if (AverageEpisodeDuration / 60 < 60)
            return $"{AverageEpisodeDuration / 60} min";

        return $"{AverageEpisodeDuration / 3600} h {(AverageEpisodeDuration % 3600) / 60} min";
    }

    //TODO: improve this later
    public string? AllGenresThemesDemographicsToString()
    {
        List<string> combinedGenres = new();

        if (Genres.Any())
            combinedGenres.AddRange(Genres.Select(g => g.Name));

        if (ExplicitGenres.Any())
            combinedGenres.AddRange(ExplicitGenres.Select(g => g.Name));

        if (Themes.Any())
            combinedGenres.AddRange(Themes.Select(g => g.Name));

        if (Demographics.Any())
            combinedGenres.AddRange(Demographics.Select(g => g.Name));

        combinedGenres.Sort();

        return string.Join(", ", combinedGenres);
    }
}

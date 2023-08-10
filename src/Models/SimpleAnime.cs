namespace AwcHelper.Txt.Models;
internal class SimpleAnime
{
    public string Title { get; }
    public string Score { get; }
    public string Year { get; }
    public string Episodes { get; }
    public string EpDuration { get; }
    public string Genres { get; }
    public string Link { get; }

    public SimpleAnime(Anime anime)
    {
        Title = anime.Title!;
        Score = anime.Mean.ToString() ?? "none";
        Year = anime.DeserializedStartDate.Year.ToString() ?? "unknown";
        Episodes = anime.NumEpisodes.ToString() ?? "unknown";
        EpDuration = anime.EpisodeDurationToString();
        Genres = anime.GenresToString() ?? "none";
        Link = $@"https://myanimelist.net/anime/{anime.Id}";
    }
}
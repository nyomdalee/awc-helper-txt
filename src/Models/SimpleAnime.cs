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

        if (anime.Mean == null)
            Score = "None";
        else
            Score = anime.Mean.ToString()!;

        if (anime.DeserializedStartDate.Year == null)
            Year = "Unknown";
        else
            Year = anime.DeserializedStartDate.Year.ToString()!;

        if (anime.NumEpisodes == 0 || anime.NumEpisodes == null)
            Episodes = "Unknown";
        else
            Episodes = anime.NumEpisodes.ToString()!;

        EpDuration = anime.EpisodeDurationToString();
        Genres = anime.GenresToString();
        Link = $@"https://myanimelist.net/anime/{anime.Id}";
    }
}
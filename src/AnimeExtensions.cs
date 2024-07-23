using MALSuite.Core.Entities;
using MALSuite.Txt.Models;

namespace MALSuite.Txt.Extensions;

public static class AnimeExtensions
{
    internal static IEnumerable<TabularAnime> MapToTabular(this IEnumerable<Anime> animeList)
    {
        foreach (var anime in animeList)
        {
            yield return anime.MapToTabular();
        }
    }

    internal static TabularAnime MapToTabular(this Anime anime) =>
        new(
            Title: anime.Title ?? string.Empty,
            Score: anime.Mean?.ToString() ?? "None",
            Year: anime.StartDate?.Year?.ToString() ?? "Unknown",
            Episodes: anime.NumEpisodes?.ToString() ?? "Unknown",
            EpDuration: anime.EpisodeDurationToString(),
            Genres: anime.AllGenresThemesDemographicsToString(),
            Link: $"https://myanimelist.net/anime/{anime.Id}");
}
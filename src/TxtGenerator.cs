using AwcHelper.Txt.Models;
using Tababular;

namespace AwcHelper.Txt;

internal class TxtGenerator
{
    public void GenerateAll()
    {
        var animeList = new MongoDbHandler().GetAllAnime().Result;
        GenerateBySource(animeList);
    }

    private void GenerateBySource(List<Anime> animeList)
    {
        var distinctSources = animeList.Select(a => a.Source).Distinct().ToList();

        string dir = Directory.GetCurrentDirectory();
        int index = dir.IndexOf("awc-helper-txt");
        string path = dir.Substring(0, index + 14);

        var dirInfo = new DirectoryInfo(@$"{path}\Anime by Source\");
        foreach (var file in dirInfo.GetFiles())
            file.Delete();

        foreach (var source in distinctSources)
        {
            var filteredList = animeList.Where(a => a.Source == source).ToList();

            var simpleList = new List<SimpleAnime>();
            foreach (var anime in filteredList)
            {
                simpleList.Add(new SimpleAnime(anime));
            }

            var hints = new Hints { MaxTableWidth = 250 };
            var formatter = new TableFormatter(hints);
            var text = formatter.FormatObjects(simpleList);

            File.WriteAllText($@"{path}\Anime by Source\{source}.txt", text);
        }
    }
}




using AwcHelper.Txt.Models;
using Tababular;

namespace AwcHelper.Txt;

internal class TxtGenerator
{
    private readonly string _baseDirectory;
    private readonly TableFormatter _formatter;

    public TxtGenerator()
    {
        string dir = Directory.GetCurrentDirectory();
        var baseName = "awc-helper-txt";
        int index = dir.IndexOf(baseName);
        _baseDirectory = dir.Substring(0, index + baseName.Length);

        var hints = new Hints { MaxTableWidth = 250 };
        _formatter = new TableFormatter(hints);
    }

    public void GenerateAll()
    {
        var animeList = new MongoDbHandler().GetAllAnime().Result;
        GenerateBySource(animeList);
    }

    private void GenerateBySource(List<Anime> animeList)
    {
        var distinctSources = animeList
            .Where(a => a.Source != null)
            .Select(a => a.Source)
            .Distinct().ToList();

        var folder = "Anime by Source";
        DeleteExistingFiles(folder);

        foreach (var source in distinctSources)
        {
            var simpleList = GetSimpleList(animeList.Where(a => a.Source == source));

            CreateFile(simpleList, folder, source!);
        }

        Console.WriteLine($"Completed: {folder}");
    }

    private void DeleteExistingFiles(string folder)
    {
        var dirInfo = new DirectoryInfo($@"{_baseDirectory}\{folder}\");

        foreach (var file in dirInfo.GetFiles())
            file.Delete();
    }

    private void CreateFile(List<SimpleAnime> simpleList, string folder, string fileName)
    {
        var text = _formatter.FormatObjects(simpleList);
        File.WriteAllText($@"{_baseDirectory}\{folder}\{fileName}.txt", text);

        Console.WriteLine($@"Created: {folder}\{fileName}");
    }

    private List<SimpleAnime> GetSimpleList(IEnumerable<Anime> animeList)
    {
        var simpleList = new List<SimpleAnime>();
        foreach (var anime in animeList)
        {
            simpleList.Add(new SimpleAnime(anime));
        }
        return simpleList;
    }
}
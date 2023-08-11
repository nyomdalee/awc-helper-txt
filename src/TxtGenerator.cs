using AwcHelper.Txt.Models;
using Tababular;

namespace AwcHelper.Txt;

internal class TxtGenerator
{
    private readonly string _baseDirectory;
    private readonly TableFormatter _formatter;
    private readonly List<Anime> _animeList;

    public TxtGenerator()
    {
        string dir = Directory.GetCurrentDirectory();
        var baseName = "awc-helper-txt";
        int index = dir.IndexOf(baseName);
        _baseDirectory = dir.Substring(0, index + baseName.Length);

        var hints = new Hints { MaxTableWidth = 250 };
        _formatter = new TableFormatter(hints);

        _animeList = new MongoDbHandler().GetAllAnime().Result;
    }

    public void GenerateAll()
    {
        GenerateBySource();
        GenerateByStartDay();
        GenerateByStartMonth();
        GenerateByStartYear();
        GenerateByEndDay();
        GenerateByEndMonth();
        GenerateByEndYear();
    }

    private void GenerateBySource()
    {
        var distinctSources = _animeList
            .Where(a => a.Source != null)
            .Select(a => a.Source)
            .Distinct().ToList();

        var folder = "Anime by Source";
        PrepareDirectory(folder);

        foreach (var source in distinctSources)
        {
            var simpleList = GetSimpleList(_animeList.Where(a => a.Source == source));

            CreateFile(simpleList, folder, source!);
        }

        Console.WriteLine($"Completed: {folder}");
    }

    private void GenerateByStartDay()
    {
        var distinctDays = _animeList
            .Where(a => a.DeserializedStartDate.Day != null)
            .Select(a => a.DeserializedStartDate.Day)
            .Distinct()
            .ToList();

        var folder = "Anime by Start Day";
        PrepareDirectory(folder);

        foreach (var day in distinctDays)
        {
            var simpleList = GetSimpleList(_animeList.Where(a => a.DeserializedStartDate.Day == day));

            CreateFile(simpleList, folder, day.ToString()!);
        }

        Console.WriteLine($"Completed: {folder}");
    }

    private void GenerateByStartMonth()
    {
        var distinctMonths = _animeList
            .Where(a => a.DeserializedStartDate.Month != null)
            .Select(a => a.DeserializedStartDate.Month)
            .Distinct().ToList();

        var folder = "Anime by Start Month";
        PrepareDirectory(folder);

        foreach (var month in distinctMonths)
        {
            var simpleList = GetSimpleList(_animeList.Where(a => a.DeserializedStartDate.Month == month));

            CreateFile(simpleList, folder, month.ToString()!);
        }

        Console.WriteLine($"Completed: {folder}");
    }

    private void GenerateByStartYear()
    {
        var distinctYears = _animeList
            .Where(a => a.DeserializedStartDate.Year != null)
            .Select(a => a.DeserializedStartDate.Year)
            .Distinct().ToList();

        var folder = "Anime by Start Year";
        PrepareDirectory(folder);

        foreach (var year in distinctYears)
        {
            var simpleList = GetSimpleList(_animeList.Where(a => a.DeserializedStartDate.Year == year));

            CreateFile(simpleList, folder, year.ToString()!);
        }

        Console.WriteLine($"Completed: {folder}");
    }

    private void GenerateByEndDay()
    {
        var distinctDays = _animeList
            .Where(a => a.DeserializedEndDate.Day != null)
            .Select(a => a.DeserializedEndDate.Day)
            .Distinct()
            .ToList();

        var folder = "Anime by End Day";
        PrepareDirectory(folder);

        foreach (var day in distinctDays)
        {
            var simpleList = GetSimpleList(_animeList.Where(a => a.DeserializedEndDate.Day == day));

            CreateFile(simpleList, folder, day.ToString()!);
        }

        Console.WriteLine($"Completed: {folder}");
    }

    private void GenerateByEndMonth()
    {
        var distinctMonths = _animeList
            .Where(a => a.DeserializedEndDate.Month != null)
            .Select(a => a.DeserializedEndDate.Month)
            .Distinct().ToList();

        var folder = "Anime by End Month";
        PrepareDirectory(folder);

        foreach (var month in distinctMonths)
        {
            var simpleList = GetSimpleList(_animeList.Where(a => a.DeserializedEndDate.Month == month));

            CreateFile(simpleList, folder, month.ToString()!);
        }

        Console.WriteLine($"Completed: {folder}");
    }

    private void GenerateByEndYear()
    {
        var distinctYears = _animeList
            .Where(a => a.DeserializedEndDate.Year != null)
            .Select(a => a.DeserializedEndDate.Year)
            .Distinct().ToList();

        var folder = "Anime by End Year";
        PrepareDirectory(folder);

        foreach (var year in distinctYears)
        {
            var simpleList = GetSimpleList(_animeList.Where(a => a.DeserializedEndDate.Year == year));

            CreateFile(simpleList, folder, year.ToString()!);
        }

        Console.WriteLine($"Completed: {folder}");
    }

    private void PrepareDirectory(string folder)
    {
        var directory = $@"{_baseDirectory}\{folder}\";
        if (Directory.Exists(directory))
        {
            var dirInfo = new DirectoryInfo($@"{_baseDirectory}\{folder}\");

            foreach (var file in dirInfo.GetFiles())
                file.Delete();

            Console.WriteLine($"Folder cleared: {folder}");
        }
        else
        {
            Directory.CreateDirectory(directory);
            Console.WriteLine($"Folder created: {folder}");
        }
    }

    private void CreateFile(List<SimpleAnime> simpleList, string folder, string fileName)
    {
        var text = _formatter.FormatObjects(simpleList);
        File.WriteAllText($@"{_baseDirectory}\{folder}\{fileName}.txt", text);

        Console.WriteLine($@"Created: {folder}\{fileName}");
    }

    private List<SimpleAnime> GetSimpleList(IEnumerable<Anime> _animeList)
    {
        var simpleList = new List<SimpleAnime>();
        foreach (var anime in _animeList)
        {
            simpleList.Add(new SimpleAnime(anime));
        }
        return simpleList;
    }
}
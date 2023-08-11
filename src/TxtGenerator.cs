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
        GenerateByStartDay(animeList);
        GenerateByStartMonth(animeList);
        GenerateByStartYear(animeList);
        GenerateByEndDay(animeList);
        GenerateByEndMonth(animeList);
        GenerateByEndYear(animeList);
    }

    private void GenerateBySource(List<Anime> animeList)
    {
        var distinctSources = animeList
            .Where(a => a.Source != null)
            .Select(a => a.Source)
            .Distinct().ToList();

        var folder = "Anime by Source";
        PrepareDirectory(folder);

        foreach (var source in distinctSources)
        {
            var simpleList = GetSimpleList(animeList.Where(a => a.Source == source));

            CreateFile(simpleList, folder, source!);
        }

        Console.WriteLine($"Completed: {folder}");
    }

    private void GenerateByStartDay(List<Anime> animeList)
    {
        var distinctDays = animeList
            .Where(a => a.DeserializedStartDate.Day != null)
            .Select(a => a.DeserializedStartDate.Day)
            .Distinct()
            .ToList();

        var folder = "Anime by Start Day";
        PrepareDirectory(folder);

        foreach (var day in distinctDays)
        {
            var simpleList = GetSimpleList(animeList.Where(a => a.DeserializedStartDate.Day == day));

            CreateFile(simpleList, folder, day.ToString()!);
        }

        Console.WriteLine($"Completed: {folder}");
    }

    private void GenerateByStartMonth(List<Anime> animeList)
    {
        var distinctMonths = animeList
            .Where(a => a.DeserializedStartDate.Month != null)
            .Select(a => a.DeserializedStartDate.Month)
            .Distinct().ToList();

        var folder = "Anime by Start Month";
        PrepareDirectory(folder);

        foreach (var month in distinctMonths)
        {
            var simpleList = GetSimpleList(animeList.Where(a => a.DeserializedStartDate.Month == month));

            CreateFile(simpleList, folder, month.ToString()!);
        }

        Console.WriteLine($"Completed: {folder}");
    }

    private void GenerateByStartYear(List<Anime> animeList)
    {
        var distinctYears = animeList
            .Where(a => a.DeserializedStartDate.Year != null)
            .Select(a => a.DeserializedStartDate.Year)
            .Distinct().ToList();

        var folder = "Anime by Start Year";
        PrepareDirectory(folder);

        foreach (var year in distinctYears)
        {
            var simpleList = GetSimpleList(animeList.Where(a => a.DeserializedStartDate.Year == year));

            CreateFile(simpleList, folder, year.ToString()!);
        }

        Console.WriteLine($"Completed: {folder}");
    }

    private void GenerateByEndDay(List<Anime> animeList)
    {
        var distinctDays = animeList
            .Where(a => a.DeserializedEndDate.Day != null)
            .Select(a => a.DeserializedEndDate.Day)
            .Distinct()
            .ToList();

        var folder = "Anime by End Day";
        PrepareDirectory(folder);

        foreach (var day in distinctDays)
        {
            var simpleList = GetSimpleList(animeList.Where(a => a.DeserializedEndDate.Day == day));

            CreateFile(simpleList, folder, day.ToString()!);
        }

        Console.WriteLine($"Completed: {folder}");
    }

    private void GenerateByEndMonth(List<Anime> animeList)
    {
        var distinctMonths = animeList
            .Where(a => a.DeserializedEndDate.Month != null)
            .Select(a => a.DeserializedEndDate.Month)
            .Distinct().ToList();

        var folder = "Anime by End Month";
        PrepareDirectory(folder);

        foreach (var month in distinctMonths)
        {
            var simpleList = GetSimpleList(animeList.Where(a => a.DeserializedEndDate.Month == month));

            CreateFile(simpleList, folder, month.ToString()!);
        }

        Console.WriteLine($"Completed: {folder}");
    }

    private void GenerateByEndYear(List<Anime> animeList)
    {
        var distinctYears = animeList
            .Where(a => a.DeserializedEndDate.Year != null)
            .Select(a => a.DeserializedEndDate.Year)
            .Distinct().ToList();

        var folder = "Anime by End Year";
        PrepareDirectory(folder);

        foreach (var year in distinctYears)
        {
            var simpleList = GetSimpleList(animeList.Where(a => a.DeserializedEndDate.Year == year));

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
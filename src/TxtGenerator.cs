using MALSuite.Core.Entities;
using MALSuite.Core.Specifications;
using MALSuite.Database.Repositories;
using MALSuite.Database.Repositories.Interfaces;
using MALSuite.Txt.Models;
using System.Reflection;
using Tababular;

namespace MALSuite.Txt;

internal class TxtGenerator
{
    private readonly string baseDirectory;
    private readonly TableFormatter formatter;
    private readonly IAnimeRepository animeRepository = new AnimeRepository();
    private List<Anime> animeList = [];

    public TxtGenerator()
    {
        Assembly assembly = Assembly.GetExecutingAssembly();
        string baseName = assembly.GetName().Name ?? throw new NullReferenceException();
        var dir = assembly.Location;
        baseDirectory = dir[..(dir.IndexOf(baseName) + baseName.Length)];

        formatter = new TableFormatter(new Hints { MaxTableWidth = 230 });
    }

    public async Task GenerateAllAsync()
    {
        animeList = await animeRepository.GetAllAsync();

        GenerateAllBroadcasts();
        GenerateBySource();
        await GenerateByStartDay();
        GenerateByStartMonth();
        GenerateByStartYear();
        GenerateByEndDay();
        GenerateByEndMonth();
        GenerateByEndYear();
        GenerateByOpEd();
        GenerateTenTitle();
        GenerateTenId();
        UpdateReadme();
    }

    private void WriteFolderComplete(string folder) => Console.WriteLine($"Completed: {folder}");

    private void GenerateAllBroadcasts()
    {
        var folder = "Anime by Broadcast";
        PrepareDirectory(folder);

        GenerateByBroadcastWindow(new TimeSpan(6, 0, 0), new TimeSpan(11, 59, 0), folder, "Morning (0600 - 1159 JST)");
        GenerateByBroadcastWindow(new TimeSpan(17, 0, 0), new TimeSpan(22, 59, 0), folder, "Afternoon-Evening (1700 - 2259 JST)");
        GenerateByBroadcastWindow(new TimeSpan(23, 0, 0), new TimeSpan(3, 59, 0), folder, "Late night (2300 - 0359 JST)");

        WriteFolderComplete(folder);
    }

    private void GenerateByBroadcastWindow(TimeSpan timeStart, TimeSpan timeEnd, string folder, string broadcastWindowName)
    {
        IEnumerable<Anime> filteredAnime = timeEnd > timeStart
            ? animeList
                .Where(a => (a.Broadcast?.StartTime != null))
                .Where(a => TimeSpan.Parse(a.Broadcast.StartTime!) >= timeStart && TimeSpan.Parse(a.Broadcast.StartTime!) <= timeEnd)
            : animeList
                .Where(a => (a.Broadcast?.StartTime != null))
                .Where(a => TimeSpan.Parse(a.Broadcast.StartTime!) >= timeStart || TimeSpan.Parse(a.Broadcast.StartTime!) <= timeEnd);

        CreateFile(GetSimpleList(filteredAnime), folder, broadcastWindowName!);
    }

    private void GenerateBySource()
    {
        var distinctSources = animeList
            .Where(a => a.Source != null)
            .Select(a => a.Source)
            .Distinct();

        var folder = "Anime by Source";
        PrepareDirectory(folder);

        foreach (var source in distinctSources)
        {
            CreateFile(GetSimpleList(animeList.Where(a => a.Source == source)), folder, source!);
        }

        WriteFolderComplete(folder);
    }

    private async Task GenerateByStartDay()
    {
        var distinctDays = animeList
            .Where(a => a.StartDate.Day != null)
            .Select(a => a.StartDate.Day)
            .Distinct();

        var folder = "Anime by Start Day";
        PrepareDirectory(folder);

        foreach (var day in distinctDays)
        {
            var spec = new AnimeSpecification() { Day = day };
            var filtered = await animeRepository.GetBySpecification(spec);
            var simpleList = GetSimpleList(filtered);

            CreateFile(simpleList, folder, day.ToString()!);
        }

        WriteFolderComplete(folder);
    }

    private void GenerateByStartMonth()
    {
        var distinctMonths = animeList
            .Where(a => a.StartDate.Month != null)
            .Select(a => a.StartDate.Month)
            .Distinct();

        var folder = "Anime by Start Month";
        PrepareDirectory(folder);

        foreach (var month in distinctMonths)
        {
            CreateFile(GetSimpleList(animeList.Where(a => a.StartDate.Month == month)), folder, month.ToString()!);
        }

        WriteFolderComplete(folder);
    }

    private void GenerateByStartYear()
    {
        var distinctYears = animeList
            .Where(a => a.StartDate.Year != null)
            .Select(a => a.StartDate.Year)
            .Distinct();

        var folder = "Anime by Start Year";
        PrepareDirectory(folder);

        foreach (var year in distinctYears)
        {
            CreateFile(GetSimpleList(animeList.Where(a => a.StartDate.Year == year)), folder, year.ToString()!);
        }

        WriteFolderComplete(folder);
    }

    private void GenerateByEndDay()
    {
        var distinctDays = animeList
            .Where(a => a.EndDate.Day != null)
            .Select(a => a.EndDate.Day)
            .Distinct();

        var folder = "Anime by End Day";
        PrepareDirectory(folder);

        foreach (var day in distinctDays)
        {
            CreateFile(GetSimpleList(animeList.Where(a => a.EndDate.Day == day)), folder, day.ToString()!);
        }

        WriteFolderComplete(folder);
    }

    private void GenerateByEndMonth()
    {
        var distinctMonths = animeList
            .Where(a => a.EndDate.Month != null)
            .Select(a => a.EndDate.Month)
            .Distinct();

        var folder = "Anime by End Month";
        PrepareDirectory(folder);

        foreach (var month in distinctMonths)
        {
            CreateFile(GetSimpleList(animeList.Where(a => a.EndDate.Month == month)), folder, month.ToString()!);
        }

        WriteFolderComplete(folder);
    }

    private void GenerateByEndYear()
    {
        var distinctYears = animeList
            .Where(a => a.EndDate.Year != null)
            .Select(a => a.EndDate.Year)
            .Distinct();

        var folder = "Anime by End Year";
        PrepareDirectory(folder);

        foreach (var year in distinctYears)
        {
            CreateFile(GetSimpleList(animeList.Where(a => a.EndDate.Year == year)), folder, year.ToString()!);
        }

        WriteFolderComplete(folder);
    }

    private void GenerateByOpEd()
    {
        var folder = @"Anime by OP and ED";
        PrepareDirectory(folder);

        var simpleList = GetSimpleList(animeList.Where(a => a.NumEpisodes >= 20 && a.Theme.Openings.Count() == 1 && a.Theme.Endings.Count() == 1));
        CreateFile(simpleList, folder, @"20 or more episodes with only 1 OP and ED");

        simpleList = GetSimpleList(animeList.Where(a => a.Theme.Openings.Count() >= 5 || a.Theme.Endings.Count() >= 5));
        CreateFile(simpleList, folder, @"5 or more OP or ED");

        simpleList = GetSimpleList(animeList.Where(IsTwoBySameArtist));
        CreateFile(simpleList, folder, @"2 or more OP or ED by the same artist");

        static bool IsTwoBySameArtist(Anime anime)
        {
            int initialCount = anime.Theme.Openings.Count + anime.Theme.Endings.Count;

            if (initialCount < 2)
            {
                return false;
            }

            List<string> cleaned = new();
            foreach (var item in anime.Theme.Endings.Concat(anime.Theme.Openings))
            {
                cleaned.Add(item[(item.Contains("\" by ") ? item.IndexOf("\" by ") + 5 : 0)..(item.Contains("(eps") ? item.IndexOf("(eps") : item.Length)]);
            }

            if (initialCount > cleaned.Distinct().Count())
            {
                return true;
            }

            return false;
        }

        WriteFolderComplete(folder);
    }

    private void PrepareDirectory(string folder)
    {
        var directory = $@"{baseDirectory}\{folder}\";
        if (Directory.Exists(directory))
        {
            var dirInfo = new DirectoryInfo($@"{baseDirectory}\{folder}\");

            foreach (var file in dirInfo.GetFiles())
            {
                file.Delete();
            }

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
        var text = formatter.FormatObjects(simpleList);
        File.WriteAllText($@"{baseDirectory}\{folder}\{fileName}.txt", text);

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

    private void UpdateReadme()
    {
        var date = animeList.Min(a => a.LastUpdated).ToString("MMMM dd, yyyy");

        var path = $"{baseDirectory}/README.md";
        var readmeLines = File.ReadAllLines(path);
        File.WriteAllLines(path, readmeLines.Take(readmeLines.Length - 1).ToArray());
        File.AppendAllText(path, date);
    }

    public void GenerateTenTitle()
    {
        var folder = "Anime by Title";
        PrepareDirectory(folder);

        var textList = GetSimpleList(animeList
            .Where(a => a.Title.Contains("10", StringComparison.InvariantCultureIgnoreCase)));

        var twoList = GetSimpleList(animeList
            .Where(a => a.Title.Contains("ten", StringComparison.InvariantCultureIgnoreCase)));

        textList.AddRange(twoList);
        CreateFile(textList, folder, "Title contains 'ten' or '10'");

        WriteFolderComplete(folder);
    }

    public void GenerateTenId()
    {
        var folder = "Anime by ID";
        PrepareDirectory(folder);

        var oneList = GetSimpleList(animeList
            .Where(a => a.Id.ToString().Contains("10", StringComparison.InvariantCultureIgnoreCase)));

        CreateFile(oneList, folder, "ID contains '10'");

        WriteFolderComplete(folder);
    }
}
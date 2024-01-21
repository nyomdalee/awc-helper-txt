using Models;
using System.Text.RegularExpressions;
using Tababular;

namespace AwcHelper.Txt;

internal class TxtGenerator
{
    private readonly string _baseDirectory;
    private readonly TableFormatter _formatter;
    private readonly List<DomainAnime> _animeList;

    public TxtGenerator()
    {
        string dir = Directory.GetCurrentDirectory();
        var baseName = "awc-helper-txt";
        int index = dir.IndexOf(baseName);
        _baseDirectory = dir.Substring(0, index + baseName.Length);

        var hints = new Hints { MaxTableWidth = 230 };
        _formatter = new TableFormatter(hints);

        _animeList = new MongoDbHandler().GetAllAnime().Result;
    }

    public void GenerateAll()
    {
        GenerateAllBroadcasts();
        GenerateBySource();
        GenerateByStartDay();
        GenerateByStartMonth();
        GenerateByStartYear();
        GenerateByEndDay();
        GenerateByEndMonth();
        GenerateByEndYear();
        GenerateByOpEd();
        UpdateReadme();
    }

    private void GenerateAllBroadcasts()
    {
        var folder = "Anime by Broadcast";
        PrepareDirectory(folder);

        GenerateByBroadcastWindow(new TimeSpan(6, 0, 0), new TimeSpan(11, 59, 0), folder, "Morning (0600 - 1159 JST)");
        GenerateByBroadcastWindow(new TimeSpan(17, 0, 0), new TimeSpan(22, 59, 0), folder, "Afternoon-Evening (1700 - 2259 JST)");
        GenerateByBroadcastWindow(new TimeSpan(23, 0, 0), new TimeSpan(3, 59, 0), folder, "Late night (2300 - 0359 JST)");

        Console.WriteLine($"Completed: {folder}");
    }

    private void GenerateByBroadcastWindow(TimeSpan timeStart, TimeSpan timeEnd, string folder, string broadcastWindowName)
    {
        IEnumerable<DomainAnime> filteredAnime;
        if (timeEnd > timeStart)
        {
            filteredAnime = _animeList
                .Where(a => (a.Broadcast?.StartTime != null))
                .Where(a => TimeSpan.Parse(a.Broadcast.StartTime!) >= timeStart && TimeSpan.Parse(a.Broadcast.StartTime!) <= timeEnd);
        }
        else
        {
            filteredAnime = _animeList
                .Where(a => (a.Broadcast?.StartTime != null))
                .Where(a => TimeSpan.Parse(a.Broadcast.StartTime!) >= timeStart || TimeSpan.Parse(a.Broadcast.StartTime!) <= timeEnd);
        }

        var simpleList = GetSimpleList(filteredAnime);
        CreateFile(simpleList, folder, broadcastWindowName!);
    }

    private void GenerateBySource()
    {
        var distinctSources = _animeList
            .Where(a => a.Source != null)
            .Select(a => a.Source)
            .Distinct();

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
            .Where(a => a.StartDate.Day != null)
            .Select(a => a.StartDate.Day)
            .Distinct();

        var folder = "Anime by Start Day";
        PrepareDirectory(folder);

        foreach (var day in distinctDays)
        {
            var simpleList = GetSimpleList(_animeList.Where(a => a.StartDate.Day == day));

            CreateFile(simpleList, folder, day.ToString()!);
        }

        Console.WriteLine($"Completed: {folder}");
    }

    private void GenerateByStartMonth()
    {
        var distinctMonths = _animeList
            .Where(a => a.StartDate.Month != null)
            .Select(a => a.StartDate.Month)
            .Distinct();

        var folder = "Anime by Start Month";
        PrepareDirectory(folder);

        foreach (var month in distinctMonths)
        {
            var simpleList = GetSimpleList(_animeList.Where(a => a.StartDate.Month == month));

            CreateFile(simpleList, folder, month.ToString()!);
        }

        Console.WriteLine($"Completed: {folder}");
    }

    private void GenerateByStartYear()
    {
        var distinctYears = _animeList
            .Where(a => a.StartDate.Year != null)
            .Select(a => a.StartDate.Year)
            .Distinct();

        var folder = "Anime by Start Year";
        PrepareDirectory(folder);

        foreach (var year in distinctYears)
        {
            var simpleList = GetSimpleList(_animeList.Where(a => a.StartDate.Year == year));

            CreateFile(simpleList, folder, year.ToString()!);
        }

        Console.WriteLine($"Completed: {folder}");
    }

    private void GenerateByEndDay()
    {
        var distinctDays = _animeList
            .Where(a => a.EndDate.Day != null)
            .Select(a => a.EndDate.Day)
            .Distinct();

        var folder = "Anime by End Day";
        PrepareDirectory(folder);

        foreach (var day in distinctDays)
        {
            var simpleList = GetSimpleList(_animeList.Where(a => a.EndDate.Day == day));

            CreateFile(simpleList, folder, day.ToString()!);
        }

        Console.WriteLine($"Completed: {folder}");
    }

    private void GenerateByEndMonth()
    {
        var distinctMonths = _animeList
            .Where(a => a.EndDate.Month != null)
            .Select(a => a.EndDate.Month)
            .Distinct();

        var folder = "Anime by End Month";
        PrepareDirectory(folder);

        foreach (var month in distinctMonths)
        {
            var simpleList = GetSimpleList(_animeList.Where(a => a.EndDate.Month == month));

            CreateFile(simpleList, folder, month.ToString()!);
        }

        Console.WriteLine($"Completed: {folder}");
    }

    private void GenerateByEndYear()
    {
        var distinctYears = _animeList
            .Where(a => a.EndDate.Year != null)
            .Select(a => a.EndDate.Year)
            .Distinct();

        var folder = "Anime by End Year";
        PrepareDirectory(folder);

        foreach (var year in distinctYears)
        {
            var simpleList = GetSimpleList(_animeList.Where(a => a.EndDate.Year == year));

            CreateFile(simpleList, folder, year.ToString()!);
        }

        Console.WriteLine($"Completed: {folder}");
    }


    private void GenerateByOpEd()
    {
        var folder = @"Anime by OP and ED";
        PrepareDirectory(folder);

        var simpleList = GetSimpleList(_animeList.Where(a => a.NumEpisodes >= 20 && a.Theme.Openings.Count() == 1 && a.Theme.Endings.Count() == 1));
        CreateFile(simpleList, folder, @"20episodesOnly1OPand1ED");

        simpleList = GetSimpleList(_animeList.Where(a => a.Theme.Openings.Count() >= 5 || a.Theme.Endings.Count() >= 5));
        CreateFile(simpleList, folder, @"5 or more OP or ED");

        simpleList = GetSimpleList(_animeList.Where(IsTwoBySameArtist));
        CreateFile(simpleList, folder, @"2 or more OP or ED by the same artist");

        static bool IsTwoBySameArtist(DomainAnime anime)
        {
            int initialCount = anime.Theme.Openings.Count + anime.Theme.Endings.Count;

            if (initialCount < 2)
                return false;

            List<string> cleaned = new();
            foreach (var item in anime.Theme.Endings.Concat(anime.Theme.Openings))
                cleaned.Add(item[(item.Contains("\" by ") ? item.IndexOf("\" by ") + 5 : 0)..(item.Contains("(eps") ? item.IndexOf("(eps") : item.Length)]);

            if (initialCount > cleaned.Distinct().Count())
                return true;

            return false;
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

    private List<SimpleAnime> GetSimpleList(IEnumerable<DomainAnime> animeList)
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
        var date = _animeList.Min(a => a.LastUpdated).ToString("MMMM dd, yyyy");

        var path = $"{_baseDirectory}/README.md";
        var readmeLines = File.ReadAllLines(path);
        File.WriteAllLines(path, readmeLines.Take(readmeLines.Length - 1).ToArray());
        File.AppendAllText(path, date);
    }

    public void GenerateTwentyThree()
    {
        var folder = "Anime by 23";
        PrepareDirectory(folder);

        var simpleList = GetSimpleList(_animeList.Where(a => Regex.IsMatch(a.Id.ToString(), @".*23.*")));

        CreateFile(simpleList, folder, "23");

        Console.WriteLine($"Completed: {folder}");
    }

}
using MALSuite.Core.Entities;
using MALSuite.Core.Extensions;
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
    private IQueryable<Anime> animeQueryable = Enumerable.Empty<Anime>().AsQueryable();

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
        // TOOD: preorder by id
        animeList = await animeRepository.GetAllAsync();
        animeQueryable = animeList.AsQueryable();

        GenerateAllBroadcasts();
        GenerateBySource();
        GenerateByStartDay();
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

    private void GenerateBySource() =>
        GenerateByDistinctPropertyValues(
            "Anime by Source",
            x => x.Source,
            source => new AnimeSpecification { Source = source });

    private void GenerateByStartDay() =>
        GenerateByDistinctPropertyValues(
            "Anime by Start Day",
            x => x.StartDate.Day,
            startDay => new AnimeSpecification { StartDay = startDay });

    private void GenerateByStartMonth() =>
        GenerateByDistinctPropertyValues(
            "Anime by Start Month",
            x => x.StartDate.Month,
            startMonth => new AnimeSpecification { StartMonth = startMonth });

    private void GenerateByStartYear() =>
        GenerateByDistinctPropertyValues(
            "Anime by Start Year",
            x => x.StartDate.Year,
            startYear => new AnimeSpecification { StartYear = startYear });

    private void GenerateByEndDay() =>
        GenerateByDistinctPropertyValues(
            "Anime by End Day",
            x => x.EndDate.Day,
            endDay => new AnimeSpecification { EndDay = endDay });

    private void GenerateByEndMonth() =>
        GenerateByDistinctPropertyValues(
            "Anime by End Month",
            x => x.EndDate.Month,
            endMonth => new AnimeSpecification { EndMonth = endMonth });

    private void GenerateByEndYear() =>
        GenerateByDistinctPropertyValues(
            "Anime by End Year",
            x => x.EndDate.Year,
            endYear => new AnimeSpecification { EndYear = endYear });

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

    private void GenerateByDistinctPropertyValues<TProperty>(
        string folderName,
        Func<Anime, TProperty> propertySelector,
        Func<TProperty, AnimeSpecification> createSpecification)
    {
        var distinctValues = animeList.GetDistinctPropertyValues(propertySelector);

        PrepareDirectory(folderName);

        foreach (var value in distinctValues)
        {
            var spec = createSpecification(value);
            var filtered = animeQueryable.Filter(spec);
            var simpleList = GetSimpleList(filtered);

            CreateFile(simpleList, folderName, value.ToString()!);
        }

        WriteFolderComplete(folderName);
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

    private void WriteFolderComplete(string folder) => Console.WriteLine($"Completed: {folder}");

    private void UpdateReadme()
    {
        var date = animeList.Min(a => a.LastUpdated).ToString("MMMM dd, yyyy");

        var path = $"{baseDirectory}/README.md";
        var readmeLines = File.ReadAllLines(path);
        File.WriteAllLines(path, readmeLines.Take(readmeLines.Length - 1).ToArray());
        File.AppendAllText(path, date);
    }
}
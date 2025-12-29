using MALSuite.Core.Entities;
using MALSuite.Core.Extensions;
using MALSuite.Core.Specifications;
using MALSuite.Database.Repositories;
using MALSuite.Txt.Extensions;
using MALSuite.Txt.Models;
using System.Reflection;
using Tababular;

namespace MALSuite.Txt;

internal class TxtGenerator
{
    private readonly string baseDirectory;
    private readonly TableFormatter formatter;
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
        animeQueryable = (await new AnimeRepository().GetAllAsync()).OrderBy(x => x.Id).Where(x => x.Approved == true).AsQueryable();

        GenerateBySource();
        GenerateByStartDay();
        GenerateByStartDayOfWeek();
        GenerateByStartMonth();
        GenerateByStartYear();
        GenerateByEndDay();
        GenerateByEndMonth();
        GenerateByEndYear();
        GenerateByBroadcastWindow();
        GenerateByBroadcastDay();
        GenerateByOpEd();
        GenerateTenTitle();
        GenerateTenId();
        GenerateTvTypeWith25EpisodeDuration();
        UpdateReadme();
    }

    private void GenerateBySource() =>
        GenerateByDistinctPropertyValues(
            "Anime by Source",
            x => x.Source,
            source => new() { Source = source });

    private void GenerateByStartDay() =>
        GenerateByDistinctPropertyValues(
            "Anime by Start Day",
            x => x.StartDate?.Day,
            startDay => new() { StartDay = startDay });

    private void GenerateByStartDayOfWeek() =>
        GenerateByDistinctPropertyValues(
            "Anime by Start Day of Week",
            x => x.StartDayOfWeek,
            startDayOfWeek => new() { StartDayOfWeek = startDayOfWeek });

    private void GenerateByStartMonth() =>
        GenerateByDistinctPropertyValues(
            "Anime by Start Month",
            x => x.StartDate?.Month,
            startMonth => new() { StartMonth = startMonth });

    private void GenerateByStartYear() =>
        GenerateByDistinctPropertyValues(
            "Anime by Start Year",
            x => x.StartDate?.Year,
            startYear => new() { StartYear = startYear });

    private void GenerateByEndDay() =>
        GenerateByDistinctPropertyValues(
            "Anime by End Day",
            x => x.EndDate?.Day,
            endDay => new() { EndDay = endDay });

    private void GenerateByEndMonth() =>
        GenerateByDistinctPropertyValues(
            "Anime by End Month",
            x => x.EndDate?.Month,
            endMonth => new() { EndMonth = endMonth });

    private void GenerateByEndYear() =>
        GenerateByDistinctPropertyValues(
            "Anime by End Year",
            x => x.EndDate?.Year,
            endYear => new() { EndYear = endYear });

    private void GenerateByBroadcastWindow() =>
        GenerateBySpecs(
            @"Anime by Broadcast\Time",
            [
                (new() { BroadcastWindow = new(new TimeSpan(6, 0, 0), new TimeSpan(11, 59, 0)) }, "Morning (0600 - 1159 JST)"),
                (new() { BroadcastWindow = new(new TimeSpan(17, 0, 0), new TimeSpan(22, 59, 0)) }, "Afternoon-Evening (1700 - 2259 JST)"),
                (new() { BroadcastWindow = new(new TimeSpan(23, 0, 0), new TimeSpan(3, 59, 0)) }, "Late night (2300 - 0359 JST)"),
            ]);

    private void GenerateByBroadcastDay() =>
        GenerateByDistinctPropertyValues(
            @"Anime by Broadcast\Day of week",
            x => x.Broadcast?.DayOfTheWeek,
            broadCastDay => new() { BroadcastDayOfWeek = broadCastDay });

    private void GenerateByOpEd() =>
    GenerateBySpecs(
        "Anime by OP and ED",
        [
            (new() { Is20PlusEpsWithOnly1OpEd = true }, "20 or more episodes with only 1 OP and ED"),
                (new() { Has5OrMoreOpOrEd = true }, "5 or more OP or ED"),
                (new() { Has2OrMoreOpOrEdBySameArtist = true }, "2 or more OP or ED by the same artist"),
        ]);

    private void GenerateTenTitle() =>
        GenerateBySpecs(
            "Anime by Title",
            [
                (new() { TitleContains = ["10", "ten"] }, "Title contains 'ten' or '10'")
            ]);

    private void GenerateTenId() =>
        GenerateBySpecs(
            "Anime by ID",
            [
                (new() { IdContains = ["10"] }, "ID contains '10'"),
                (new() { IdContains = ["25"] }, "ID contains '25'"),
                (new() { IdContains = ["26"] }, "ID contains '26'"),
            ]);

    private void GenerateTvTypeWith25EpisodeDuration() =>
        GenerateBySpecs(
            "Anime by Episode Duration",
            [
                (new() { MediaType = "TV", EpisodeDuration = new(25 * 60, ValueComparerEnum.GreaterThanOrEqual) }, "TV type with episode duration of 25min or more")
            ]);

    private void GenerateByDistinctPropertyValues<TProperty>(
        string folderName,
        Func<Anime, TProperty> propertySelector,
        Func<TProperty, AnimeSpecification> createSpecification)
    {
        var distinctValues = animeQueryable.GetDistinctPropertyValues(propertySelector);
        PrepareDirectory(folderName);

        foreach (var value in distinctValues)
        {
            var spec = createSpecification(value);
            var tabularList = animeQueryable.Filter(spec).MapToTabular();

            CreateFile(tabularList, folderName, value?.ToString() ?? throw new NullReferenceException());
        }

        WriteFolderComplete(folderName);
    }

    private void GenerateBySpecs(string folderName, List<(AnimeSpecification Spec, string FileName)> specs)
    {
        PrepareDirectory(folderName);

        foreach (var (spec, fileName) in specs)
        {
            var tabularList = animeQueryable.Filter(spec).MapToTabular();

            CreateFile(tabularList, folderName, fileName);
        }

        WriteFolderComplete(folderName);
    }

    private void PrepareDirectory(string folder)
    {
        var directory = Path.Combine(baseDirectory, folder);
        if (Directory.Exists(directory))
        {
            foreach (var file in Directory.GetFiles(directory))
            {
                File.Delete(file);
            }

            Console.WriteLine($"Folder cleared: {folder}");
        }
        else
        {
            Directory.CreateDirectory(directory);
            Console.WriteLine($"Folder created: {folder}");
        }
    }

    private void CreateFile(IEnumerable<TabularAnime> tabularList, string folderName, string fileName)
    {
        var text = formatter.FormatObjects(tabularList);
        File.WriteAllText($@"{baseDirectory}\{folderName}\{fileName}.txt", text);

        Console.WriteLine($@"Created: {folderName}\{fileName}");
    }

    private void WriteFolderComplete(string folder) => Console.WriteLine($"Completed: {folder}");

    private void UpdateReadme()
    {
        var date = animeQueryable.Min(a => a.LastUpdated).ToString("MMMM dd, yyyy");

        var path = $"{baseDirectory}/README.md";
        var readmeLines = File.ReadAllLines(path);
        File.WriteAllLines(path, readmeLines.Take(readmeLines.Length - 1).ToArray());
        File.AppendAllText(path, date);
    }
}
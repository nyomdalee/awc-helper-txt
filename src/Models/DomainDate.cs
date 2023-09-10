namespace Models;

public class DomainDate
{
    public int? Year { get; }
    public int? Month { get; }
    public int? Day { get; }
    public string? Full { get; }

    public DomainDate(string? full, int? year, int? month, int? day)
    {
        Full = full;
        Year = year;
        Month = month;
        Day = day;
    }
}

namespace AwcHelper.Txt.Models;

public class StartSeason
{
    public int? Year { get; set; } = null;

    public string? Season { get; set; } = null;

    public StartSeason() { }

    public override string? ToString()
    {
        if (Year == null)
            return null;

        return $"{Season} {Year}";
    }
}

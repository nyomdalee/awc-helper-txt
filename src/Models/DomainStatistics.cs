namespace MALSuite.Txt.Models;

public class DomainStatistics
{
    public int? Watching { get; }

    public int? Completed { get; }

    public int? OnHold { get; }

    public int? Dropped { get; }

    public int? PlanToWatch { get; }

    public int? Total { get; }

    public DomainStatistics(int? watching, int? completed, int? onHold, int? dropped, int? planToWatch, int? total)
    {
        Watching = watching;
        Completed = completed;
        OnHold = onHold;
        Dropped = dropped;
        PlanToWatch = planToWatch;
        Total = total;
    }
}

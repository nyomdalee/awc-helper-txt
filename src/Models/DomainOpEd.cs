namespace MALSuite.Txt.Models;

public class DomainOpEd
{
    public List<string> Openings { get; }

    public List<string> Endings { get; }

    public DomainOpEd(List<string> openings, List<string> endings)
    {
        Openings = openings;
        Endings = endings;
    }
}
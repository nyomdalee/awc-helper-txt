namespace MALSuite.Txt;

public static class Program
{
    public static async Task Main() => await new TxtGenerator().GenerateAllAsync();
}
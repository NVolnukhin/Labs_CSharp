public static class ProbabilityCalculator
{
    private static readonly Random _random = new Random();

    public static bool IsEventHappened(double probability) =>
        _random.NextDouble() <= probability;
}
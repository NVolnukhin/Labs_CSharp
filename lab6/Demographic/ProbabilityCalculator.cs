/// <summary>
/// Утилита для расчета вероятности события.
/// </summary>
public static class ProbabilityCalculator
{
    private static readonly Random _random = new Random();

    public static bool IsEventHappened(double eventProbability)
    {
        return _random.NextDouble() <= eventProbability;
    }
}
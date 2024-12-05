namespace Demographic.FileOperations;

/// <summary>
/// Представляет правило смертности для возрастного интервала.
/// </summary>
public class DeathRate
{
    public int StartAge { get; }
    public int EndAge { get; }
    public double MaleDeathRate { get; }
    public double FemaleDeathRate { get; }

    public DeathRate(int startAge, int endAge, double maleDeathRate, double femaleDeathRate)
    {
        StartAge = startAge;
        EndAge = endAge;
        MaleDeathRate = maleDeathRate;
        FemaleDeathRate = femaleDeathRate;
    }
}
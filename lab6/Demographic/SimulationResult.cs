namespace Demographic.FileOperations;

/// <summary>
/// Представляет результат моделирования для одного года.
/// </summary>
public class SimulationResult
{
    public int Year { get; }
    public int TotalPopulation { get; }
    public int MalePopulation { get; }
    public int FemalePopulation { get; }

    public SimulationResult(int year, int totalPopulation, int malePopulation, int femalePopulation)
    {
        Year = year;
        TotalPopulation = totalPopulation;
        MalePopulation = malePopulation;
        FemalePopulation = femalePopulation;
    }
}

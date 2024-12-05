using Demographic;
using Demographic.FileOperations;

namespace Demographic;

public interface IEngine
{
    IEnumerable<SimulationResult> RunSimulation(int startYear, int endYear);
}
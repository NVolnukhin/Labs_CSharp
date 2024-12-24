public interface IEngine
{
    event Action<int> YearTick; 

    void StartSimulation(int startYear, int endYear); 
    IEnumerable<SimulationResult> GetResults(); 
}
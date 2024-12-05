namespace Demographic;

public interface IEngine
{
    void StartSimulation(int startYear, int endYear, int initialPopulation);
    event EventHandler<int> YearTick; // Событие о начале нового года
}

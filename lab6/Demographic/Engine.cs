namespace Demographic;

public class Engine : IEngine
{
    public event EventHandler<int> YearTick;
    private List<Person> people;

    public Engine(IEnumerable<Person> initialPopulation)
    {
        people = new List<Person>(initialPopulation);
    }

    public void StartSimulation(int startYear, int endYear, int initialPopulation)
    {
        for (int year = startYear; year <= endYear; year++)
        {
            YearTick?.Invoke(this, year);
            foreach (var person in people.ToList())
                person.OnYearPassed(year);
        }
    }
}

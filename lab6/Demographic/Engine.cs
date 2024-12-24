public class Engine : IEngine
{
    private readonly List<Person> _population;
    private readonly List<DeathRule> _deathRules;
    private readonly List<SimulationResult> _results;

    
    private const double BirthProbability = 0.151; 
    private const int GroupSize = 1000;            
    private const double FemaleBirthRate = 0.55;
    
    public event Action<int>? YearTick;

    public Engine(IEnumerable<Person> initialPopulation, IEnumerable<DeathRule> deathRules)
    {
        _population = initialPopulation.ToList();
        _deathRules = deathRules.ToList();
        _results = new List<SimulationResult>();

        
        int totalPopulation = _population.Sum(p => p.GroupSize);
        int malePopulation = _population.Where(p => p.Gender == "Male").Sum(p => p.GroupSize);
        int femalePopulation = _population.Where(p => p.Gender == "Female").Sum(p => p.GroupSize);
        Console.WriteLine($"Initial Population: {totalPopulation}, Males: {malePopulation}, Females: {femalePopulation}");
    }

    public void StartSimulation(int startYear, int endYear)
    {
        for (int year = startYear; year <= endYear; year++)
        {
            YearTick?.Invoke(year);

            
            foreach (var person in _population)
            {
                person.OnYearTick(year, _deathRules);
            }

            
            var newChildren = new List<Person>();
            foreach (var person in _population.Where(p => p.IsAlive && p.Gender == "Female"))
            {
                int age = year - person.BirthYear;
                if (age >= 18 && age <= 45)
                {
                    int numberOfBirths = (int)(person.GroupSize * BirthProbability); 

                    if (numberOfBirths > 0)
                    {
                        int females = (int)Math.Round(numberOfBirths * FemaleBirthRate); 
                        int males = numberOfBirths - females;                            

                        
                        while (females > 0)
                        {
                            int size = Math.Min(females, GroupSize);  
                            newChildren.Add(new Person("Female", year, size));
                            females -= size;
                        }

                        
                        while (males > 0)
                        {
                            int size = Math.Min(males, GroupSize);  
                            newChildren.Add(new Person("Male", year, size));
                            males -= size;
                        }
                    }
                }
            }

            _population.AddRange(newChildren);

            
            _results.Add(new SimulationResult
            {
                Year = year,
                TotalPopulation = _population.Where(p => p.IsAlive).Sum(p => p.GroupSize),
                MalePopulation = _population.Where(p => p.Gender == "Male" && p.IsAlive).Sum(p => p.GroupSize),
                FemalePopulation = _population.Where(p => p.Gender == "Female" && p.IsAlive).Sum(p => p.GroupSize),
            });
        }

        
        WriteAgeGroupDistribution(_population, endYear);
    }

    private void WriteAgeGroupDistribution(List<Person> population, int endYear)
    {
        var ageGroups = new[]
        {
            new { MinAge = 0, MaxAge = 18 },
            new { MinAge = 19, MaxAge = 44 },
            new { MinAge = 45, MaxAge = 64 },
            new { MinAge = 65, MaxAge = 100 }
        };

        var ageGroupResults = new List<(string AgeGroup, int Total, int Males, int Females)>();

        foreach (var ageGroup in ageGroups)
        {
            int total = population
                .Where(p => p.IsAlive && (endYear - p.BirthYear) >= ageGroup.MinAge && (endYear - p.BirthYear) <= ageGroup.MaxAge)
                .Sum(p => p.GroupSize);

            int males = population
                .Where(p => p.IsAlive && p.Gender == "Male" && (endYear - p.BirthYear) >= ageGroup.MinAge && (endYear - p.BirthYear) <= ageGroup.MaxAge)
                .Sum(p => p.GroupSize);

            int females = population
                .Where(p => p.IsAlive && p.Gender == "Female" && (endYear - p.BirthYear) >= ageGroup.MinAge && (endYear - p.BirthYear) <= ageGroup.MaxAge)
                .Sum(p => p.GroupSize);

            ageGroupResults.Add((AgeGroup: $"{ageGroup.MinAge}-{ageGroup.MaxAge}", Total: total, Males: males, Females: females));
        }

        using (var writer = new StreamWriter("AgeGroupDistribution.csv"))
        {
            writer.WriteLine("Age,Population,Men,Women");
            foreach (var group in ageGroupResults)
            {
                writer.WriteLine($"{group.AgeGroup},{group.Total},{group.Males},{group.Females}");
            }
        }
    }

    public IEnumerable<SimulationResult> GetResults() => _results;
}

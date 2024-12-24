using System.Globalization;

public class InitialAgeDataReader : IDataReader<Person>
{
    private readonly int _totalPopulation;
    private readonly int _startYear;

    
    private const double AdjustedPopulationFactor = 0.9366; 
    private const int DefaultGroupSize = 1000;              

    public InitialAgeDataReader(int totalPopulation, int startYear)
    {
        _totalPopulation = totalPopulation;
        _startYear = startYear;
    }

    public IEnumerable<Person> ReadData(string filePath)
    {
        var lines = File.ReadAllLines(filePath).Skip(1); 
        double totalCountPerThousand = 0;
        var ageGroups = new List<(int Age, double CountPerThousand)>();

       
        foreach (var line in lines)
        {
            var parts = line.Split(',');

            if (parts.Length != 2)
            {
                throw new FormatException($"Некорректная строка в файле: {line}");
            }

            int age = int.Parse(parts[0].Trim());
            double countPerThousand = double.Parse(parts[1].Trim(), CultureInfo.InvariantCulture);
            ageGroups.Add((age, countPerThousand));
            totalCountPerThousand += countPerThousand;
        }

        List<Person> population = new List<Person>();
        int populationAssigned = 0; 

        
        foreach (var (age, countPerThousand) in ageGroups)
        {
            double groupFraction = countPerThousand / totalCountPerThousand;
            int groupPopulation = (int)(_totalPopulation * groupFraction);  

            int numberOfGroups = groupPopulation / DefaultGroupSize;

            
            for (int i = 0; i < numberOfGroups; i++)
            {
                string gender = i % 2 == 0 ? "Male" : "Female";
                population.Add(new Person(gender, _startYear - age, DefaultGroupSize));
                populationAssigned += DefaultGroupSize;
            }

            
            int remaining = groupPopulation % DefaultGroupSize;
            if (remaining > 0)
            {
                string gender = numberOfGroups % 2 == 0 ? "Male" : "Female";
                population.Add(new Person(gender, _startYear - age, remaining));
                populationAssigned += remaining;
            }
        }

        
        int adjustedPopulation = (int)(_totalPopulation / AdjustedPopulationFactor);  
        int difference = adjustedPopulation - populationAssigned;

        if (difference != 0)
        {
            Console.WriteLine($"Adjusting population by {difference} to match total population");

            
            foreach (var person in population)
            {
                int adjustedGroupSize = (int)(person.GroupSize * (1 / AdjustedPopulationFactor)); 
                person.AdjustGroupSize(adjustedGroupSize);  
            }
        }

        
        int initialPopulation = population.Sum(p => p.GroupSize);
        Console.WriteLine($"Initial Population (adjusted): {initialPopulation} (target was {_totalPopulation})");

        
        return population;
    }
}

using System.Globalization;
using CsvHelper;

namespace Demographic.FileOperations;

public static class FileReader
{
    public static IEnumerable<Person> ReadInitialPopulation(string filePath)
    {
        var population = new List<Person>();

        using (var reader = new StreamReader(filePath))
        {
            string? line = reader.ReadLine(); // Считываем заголовок
            if (line == null)
                throw new InvalidOperationException("Файл пуст или отсутствует заголовок.");

            while ((line = reader.ReadLine()) != null)
            {
                var parts = line.Split(',');

                if (parts.Length != 2)
                    throw new FormatException($"Неверный формат строки: {line}");

                var age = int.Parse(parts[0], CultureInfo.InvariantCulture);
                var count = double.Parse(parts[1], CultureInfo.InvariantCulture);

                // Добавляем `count` объектов Person с данным возрастом и рандомным полом
                for (int i = 0; i < count; i++)
                {
                    bool isFemale = Random.Shared.NextDouble() < 0.5; // 50% вероятность женского пола
                    population.Add(new Person(DateTime.Now.Year - age, isFemale));
                }
            }
        }

        return population;
    }

    public static MortalityTable ReadDeathRules(string filePath)
    {
        var rules = new List<MortalityRule>();

        using (var reader = new StreamReader(filePath))
        using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
        {
            csv.Read();
            csv.ReadHeader();

            while (csv.Read())
            {
                var rule = new MortalityRule
                {
                    AgeStart = csv.GetField<int>("AgeStart"),
                    AgeEnd = csv.GetField<int>("AgeEnd"),
                    MaleMortality = csv.GetField<double>("MaleMortality"),
                    FemaleMortality = csv.GetField<double>("FemaleMortality"),
                };
                rules.Add(rule);
            }
        }

        return new MortalityTable(rules);
    }

}

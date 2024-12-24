using System.Globalization;

public class DeathRulesDataReader : IDataReader<DeathRule>
{
    public IEnumerable<DeathRule> ReadData(string filePath)
    {
        var lines = File.ReadAllLines(filePath).Skip(1); 
        foreach (var line in lines)
        {
            var parts = line.Split(',');

            
            if (parts.Length != 4)
            {
                throw new FormatException($"Некорректная строка в файле DeathRules.csv: {line}");
            }

            
            int ageStart = int.Parse(parts[0].Trim());
            int ageEnd = int.Parse(parts[1].Trim());
            double maleDeathProbability = double.Parse(parts[2].Trim(), CultureInfo.InvariantCulture);
            double femaleDeathProbability = double.Parse(parts[3].Trim(), CultureInfo.InvariantCulture);

            yield return new DeathRule
            {
                AgeStart = ageStart,
                AgeEnd = ageEnd,
                MaleDeathProbability = maleDeathProbability,
                FemaleDeathProbability = femaleDeathProbability
            };
        }
    }
}
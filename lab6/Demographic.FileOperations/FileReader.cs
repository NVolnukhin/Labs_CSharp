using System.Globalization;

namespace Demographic.FileOperations;

public static class FileOperations
{
    /// <summary>
    /// Загрузка данных о первоначальном возрастном составе населения из CSV файла.
    /// </summary>
    /// <param name="filePath">Путь к файлу.</param>
    /// <returns>Словарь, где ключ — возраст, значение — количество людей (в тыс.).</returns>
    public static Dictionary<int, double> LoadInitialPopulation(string filePath)
    {
        if (!File.Exists(filePath))
            throw new FileNotFoundException($"Файл с первоначальным возрастным составом не найден: {filePath}");

        var initialPopulation = new Dictionary<int, double>();

        foreach (var line in File.ReadLines(filePath).Skip(1)) // Пропускаем заголовок
        {
            var parts = line.Split(", ");
            if (parts.Length < 2)
                continue;

            int age = int.Parse(parts[0]);
            var count = double.Parse(parts[1]);

            initialPopulation[age] = count;
        }

        return initialPopulation;
    }

    /// <summary>
    /// Загрузка таблицы смертности из CSV файла.
    /// </summary>
    /// <param name="filePath">Путь к файлу.</param>
    /// <returns>Список правил смертности, каждый элемент содержит возрастной интервал и вероятности смерти.</returns>
    public static List<DeathRate> LoadDeathRates(string filePath)
    {
        if (!File.Exists(filePath))
            throw new FileNotFoundException($"Файл с вероятностями смертей не найден: {filePath}");

        var deathRates = new List<DeathRate>();

        foreach (var line in File.ReadLines(filePath).Skip(1)) // Пропускаем заголовок
        {
            var parts = line.Split(',');
            if (parts.Length < 4)
                continue;

            int startAge = int.Parse(parts[0]);
            int endAge = int.Parse(parts[1]);
            double maleDeathRate = double.Parse(parts[2], CultureInfo.InvariantCulture);
            double femaleDeathRate = double.Parse(parts[3], CultureInfo.InvariantCulture);

            deathRates.Add(new DeathRate(startAge, endAge, maleDeathRate, femaleDeathRate));
        }

        return deathRates;
    }

    /// <summary>
    /// Сохранение результатов моделирования в файл CSV.
    /// </summary>
    /// <param name="results">Результаты моделирования.</param>
    /// <param name="filePath">Путь к файлу.</param>
    public static void SaveResults(IEnumerable<SimulationResult> results, string filePath)
    {
        using var writer = new StreamWriter(filePath);
        writer.WriteLine("Year,TotalPopulation,MalePopulation,FemalePopulation");

        foreach (var result in results)
        {
            writer.WriteLine($"{result.Year},{result.TotalPopulation},{result.MalePopulation},{result.FemalePopulation}");
        }
    }
}
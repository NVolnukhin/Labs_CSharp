using System;
using System.IO;
using Demographic;
using Demographic.FileOperations;

class Program
{
    private static void Main(string[] args)
    {
        try
        {
            // Чтение аргументов командной строки
            string initialPopulationFile = args.Length > 0 ? args[0] : "/Users/nikitavolnuhin/Labs_cs/lab6/InitialAge.csv";
            string deathRatesFile = args.Length > 1 ? args[1] : "/Users/nikitavolnuhin/Labs_cs/lab6/DeathRules.csv";
            int startYear = args.Length > 2 ? int.Parse(args[2]) : 1970;
            int endYear = args.Length > 3 ? int.Parse(args[3]) : 2021;
            int initialPopulation = args.Length > 4 ? int.Parse(args[4]) : 130000000;

            Console.WriteLine($"Начало симуляции с {startYear} года по {endYear} год...");

            // Загрузка данных
            var initialPopulationData = FileOperations.LoadInitialPopulation(initialPopulationFile);
            var deathRates = FileOperations.LoadDeathRates(deathRatesFile);

            // Создание движка моделирования
            IEngine engine = new Engine(initialPopulationData, deathRates, startYear, initialPopulation);

            // Запуск моделирования
            var results = engine.RunSimulation(startYear, endYear);

            // Сохранение результатов
            var outputFilePath = "/Users/nikitavolnuhin/Labs_cs/lab6/SimulationResults.csv";
            FileOperations.SaveResults(results, outputFilePath);

            Console.WriteLine($"Симуляция завершена. Результаты сохранены в {outputFilePath}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Возникла ошибка: {ex.Message}");
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using Demographic;
using Demographic.FileOperations;

/// <summary>
/// Реализация интерфейса движка моделирования.
/// </summary>
public class Engine : IEngine
{
    private readonly List<Person> _population;
    private readonly List<DeathRate> _deathRates;
    private readonly int _initialYear;

    public Engine(Dictionary<int, double> initialPopulationData, List<DeathRate> deathRates, int initialYear, int initialPopulation)
    {
        _population = GenerateInitialPopulation(initialPopulationData, initialPopulation);
        _deathRates = deathRates;
        _initialYear = initialYear;
    }

    /// <summary>
    /// Запускает моделирование.
    /// </summary>
    public IEnumerable<SimulationResult> RunSimulation(int startYear, int endYear)
    {
        var results = new List<SimulationResult>();

        for (int year = startYear; year <= endYear; year++)
        {
            // Годовое событие для каждого человека
            foreach (var person in _population)
            {
                person.OnYearTick(year, _deathRates);
            }

            // Добавление данных за текущий год в результаты
            var totalPopulation = _population.Count(p => p.IsAlive);
            var malePopulation = _population.Count(p => p.IsAlive && p.Gender == Gender.Male);
            var femalePopulation = _population.Count(p => p.IsAlive && p.Gender == Gender.Female);

            results.Add(new SimulationResult(year, totalPopulation, malePopulation, femalePopulation));
        }

        return results;
    }

    /// <summary>
    /// Генерирует начальную популяцию.
    /// </summary>
    private List<Person> GenerateInitialPopulation(Dictionary<int, double> initialPopulationData, int totalPopulation)
    {
        var population = new List<Person>();
        int totalPeople = totalPopulation / 1000;

        foreach (var (age, count) in initialPopulationData)
        {
            for (int i = 0; i < count; i++)
            {
                population.Add(new Person(age, Gender.Male));
                population.Add(new Person(age, Gender.Female));
            }
        }

        return population.Take(totalPeople).ToList(); // Ограничение на общее число людей
    }
}
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

/// <summary>
/// Класс, представляющий человека.
/// </summary>
public class Person
{
    public int Age { get; private set; }
    public Gender Gender { get; }
    public bool IsAlive { get; private set; }
    public int BirthYear { get; }
    public int? DeathYear { get; private set; }

    public Person(int age, Gender gender)
    {
        Age = age;
        Gender = gender;
        IsAlive = true;
    }

    /// <summary>
    /// Обрабатывает событие "новый год".
    /// </summary>
    public void OnYearTick(int currentYear, List<DeathRate> deathRates)
    {
        if (!IsAlive) return;

        Age++;
        var deathRate = deathRates.FirstOrDefault(dr => dr.StartAge <= Age && dr.EndAge >= Age);

        if (deathRate != null && ProbabilityCalculator.IsEventHappened(
            Gender == Gender.Male ? deathRate.MaleDeathRate : deathRate.FemaleDeathRate))
        {
            IsAlive = false;
            DeathYear = currentYear;
        }
    }
}

/// <summary>
/// Пол человека.
/// </summary>
public enum Gender
{
    Male,
    Female
}

/// <summary>
/// Утилита для расчета вероятности события.
/// </summary>
public static class ProbabilityCalculator
{
    private static readonly Random _random = new Random();

    public static bool IsEventHappened(double eventProbability)
    {
        return _random.NextDouble() <= eventProbability;
    }
}


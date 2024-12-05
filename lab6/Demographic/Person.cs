using Demographic.FileOperations;

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
namespace Demographic;

public class Person
{
    public int BirthYear { get; private set; }
    public bool IsAlive { get; private set; } = true;
    public bool IsFemale { get; private set; }
    public event EventHandler<int> ChildBirth;

    public Person(int birthYear, bool isFemale)
    {
        BirthYear = birthYear;
        IsFemale = isFemale;
    }

    public void OnYearPassed(int currentYear)
    {
        if (!IsAlive) return;

        int age = currentYear - BirthYear;
        // Логика смерти
        if (ShouldDie(age))
            IsAlive = false;

        // Логика рождения ребенка
        if (IsFemale && age >= 18 && age <= 45 && IsAlive)
        {
            if (new Random().NextDouble() < 0.151) // Вероятность рождения
                ChildBirth?.Invoke(this, currentYear);
        }
    }

    private readonly MortalityTable mortalityTable = new MortalityTable(new List<MortalityRule>()); // Add actual rules

    private bool ShouldDie(int age)
    {
        // Получить вероятность смерти для данного возраста и пола
        double probability = mortalityTable.GetMortalityProbability(age, IsFemale);

        // Генерация случайного числа для проверки
        double randomValue = Random.Shared.NextDouble();

        // Умирает ли человек
        return randomValue < probability;
    }

}

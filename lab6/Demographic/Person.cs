public class Person
{
    private const double BirthProbability = 0.151; 
    private const int MinReproductiveAge = 18;     
    private const int MaxReproductiveAge = 45;     

    public bool IsAlive { get; private set; } = true;
    public string Gender { get; }
    public int BirthYear { get; }
    public int? DeathYear { get; private set; }

    
    public int GroupSize { get; private set; }

    
    public event Action? ChildBirth;

    
    private bool hasHadChildrenThisYear = false;

    public Person(string gender, int birthYear, int groupSize)
    {
        Gender = gender;
        BirthYear = birthYear;
        GroupSize = groupSize;
    }

    public void OnYearTick(int currentYear, List<DeathRule> deathRules)
    {
        if (!IsAlive) return;

        int age = currentYear - BirthYear;

        
        var rule = deathRules.FirstOrDefault(r => age >= r.AgeStart && age <= r.AgeEnd);
        if (rule != null)
        {
            double deathProbability = Gender == "Male" ? rule.MaleDeathProbability : rule.FemaleDeathProbability;
            int deaths = (int)Math.Round(GroupSize * deathProbability); // Масштабируем на размер группы
            GroupSize -= deaths;

            if (GroupSize <= 0)
            {
                IsAlive = false;
                GroupSize = 0;
                DeathYear = currentYear;
            }
        }

        
        if (Gender == "Female" && age >= MinReproductiveAge && age <= MaxReproductiveAge && !hasHadChildrenThisYear)
        {
            hasHadChildrenThisYear = true;

            if (ProbabilityCalculator.IsEventHappened(BirthProbability)) 
            {
                ChildBirth?.Invoke(); 
            }
        }
    }

    
    public void OnNewYear()
    {
        hasHadChildrenThisYear = false; 
    }

    
    public void AdjustGroupSize(int newSize)
    {
        GroupSize = newSize;
    }
}
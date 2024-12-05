namespace Demographic;

public class MortalityTable
{
    private List<MortalityRule> rules;

    public MortalityTable(IEnumerable<MortalityRule> mortalityRules)
    {
        rules = mortalityRules.ToList();
    }

    public double GetMortalityProbability(int age, bool isFemale)
    {
        var rule = rules.FirstOrDefault(r => r.AgeStart <= age && r.AgeEnd >= age);
        if (rule == null)
            return 0; // Если правило не найдено, вероятность смерти 0.

        return isFemale ? rule.FemaleMortality : rule.MaleMortality;
    }
}

public class MortalityRule
{
    public int AgeStart { get; set; }
    public int AgeEnd { get; set; }
    public double MaleMortality { get; set; }
    public double FemaleMortality { get; set; }
}

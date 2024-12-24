using Demographic.FileOperations;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            string initialAgeFile = args.Length > 0 ? args[0] : "/Users/nikitavolnuhin/Labs_cs/lab6/InitialAge.csv";
            string deathRulesFile = args.Length > 1 ? args[1] : "/Users/nikitavolnuhin/Labs_cs/lab6/DeathRules.csv";
            string resultFile = args.Length > 2 ? args[2] : "/Users/nikitavolnuhin/Labs_cs/lab6/SimulationResult.csv";
            int startYear = args.Length > 3 ? int.Parse(args[3]) : 1970;
            int endYear = args.Length > 4 ? int.Parse(args[4]) : 2021;
            int totalPopulation = args.Length > 5 ? int.Parse(args[5]) : 130000000;
            
            var reader = new InitialAgeDataReader(totalPopulation, startYear);  
            var initialPopulation = reader.ReadData(initialAgeFile);

       
            var deathRulesReader = new DeathRulesDataReader();
            var deathRules = deathRulesReader.ReadData(deathRulesFile);

        
            var engine = new Engine(initialPopulation, deathRules);
            engine.StartSimulation(startYear, endYear);

        
            var writer = new SimulationResultWriter();
            writer.WriteData(resultFile, engine.GetResults());
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка: {ex.Message}");
        }
    }
}
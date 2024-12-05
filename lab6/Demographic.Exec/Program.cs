using Demographic;
using Demographic.FileOperations;

class Program
{
    /*
    static void Main(string[] args)
    {
        string initialPopulationPath = args[0];
        string deathRulesPath = args[1];
        int startYear = int.Parse(args[2]);
        int endYear = int.Parse(args[3]);
        int initialPopulation = int.Parse(args[4]);

        var population = FileReader.ReadInitialPopulation(initialPopulationPath);
        var engine = new Engine(population);

        engine.StartSimulation(startYear, endYear, initialPopulation);
    }
    */

    static void Main()
    {
        var initialPopulationPath = "/Users/nikitavolnuhin/Labs_cs/lab6/InitialAge.csv";
        var deathRulesPath = "/Users/nikitavolnuhin/Labs_cs/lab6/DeathRules.csv";
        var startYear = 1970;
        var endYear = 2021;
        var initialPopulation = 130000;
            
        var population = FileReader.ReadInitialPopulation(initialPopulationPath);
        var engine = new Engine(population);

        engine.StartSimulation(startYear, endYear, initialPopulation);
    }
}
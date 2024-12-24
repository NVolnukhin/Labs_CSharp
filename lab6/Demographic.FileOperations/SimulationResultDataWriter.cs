namespace Demographic.FileOperations;

public class SimulationResultDataWriter
{
    
}

public class SimulationResultWriter : IDataWriter<SimulationResult>
{
    public void WriteData(string filePath, IEnumerable<SimulationResult> data)
    {
        var lines = data.Select(r => $"{r.Year},{r.TotalPopulation},{r.MalePopulation},{r.FemalePopulation}");
        File.WriteAllLines(filePath, lines);
    }
}


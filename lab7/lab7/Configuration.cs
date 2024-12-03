namespace lab7;

public class Configuration
{
    //интервал генерации событий (стекла)
    public int EmitterInterval { get; set; }
    //интервал обработки событий (выдувки стекла)
    public int[] ProcessorDelays { get; set; }
    //продолжительность симуляции
    public int SimulationDuration { get; set; }
}
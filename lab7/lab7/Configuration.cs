namespace lab7;

public class Configuration
{
    //интервал генерации событий (генерация поставки)
    public int[] EmitterDelays { get; set; }
    //интервал обработки событий (сборки поставки)
    public int[] ProcessorDelays { get; set; }
    //интервал обработки событий 2 (доставка стекла получателю)
    public int[] RecieverDelays { get; set; }
    //продолжительность симуляции
    public int SimulationDuration { get; set; }
}
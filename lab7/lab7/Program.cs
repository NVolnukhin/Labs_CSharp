namespace lab7;

class Program
{
    static async Task Main(string[] args)
    {
        var config = new Configuration
        {
            EmitterInterval = 500, // Интервал генерации событий (мс)
            ProcessorDelays = [300, 500], // Задержки обработки
            SimulationDuration = 10000 // Продолжительность моделирования (мс)
        };

        var system = new ConveyorSystem(config);
        await system.StartAsync();
    }
    

}
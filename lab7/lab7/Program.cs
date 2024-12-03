namespace lab7;

class Program
{
    static async Task Main(string[] args)
    {
        var config = new Configuration
        {
            EmitterInterval = [1000], // Интервал генерации событий (мс)
            ProcessorDelays = [500, 300], // Задержки обработки
            RecieverDelays = [700, 500],
            SimulationDuration = 10000 // Продолжительность моделирования (мс)
        };

        var system = new ConveyorSystem(config);
        await system.StartAsync();
    }
    

}
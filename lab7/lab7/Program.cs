namespace lab7;

class Program
{
    static async Task Main(string[] args)
    {
        var config = new Configuration
        {
            EmitterDelays = [2000, 1000], // Задержки генерации событий (мс)
            ProcessorDelays = [500, 300], // Задержки обработки
            RecieverDelays = [700, 500], //Задержки получения
            SimulationDuration = 1000 // Продолжительность моделирования (мс)
        };

        var system = new ConveyorSystem(config);
        await system.StartAsync();
    }
}
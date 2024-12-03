namespace lab7;

class Program
{
    static async Task Main(string[] args)
    {
        var config = new Configuration
        {
            EmitterDelays = [2000, 3000, 1500], // Задержки генерации событий (мс)
            ProcessorDelays = [5000, 3700, 6000, 9000], // Задержки обработки
            RecieverDelays = [700, 500], //Задержки получения
            SimulationDuration = 20000 // Продолжительность моделирования (мс)
        };

        var system = new ConveyorSystem(config);
        await system.StartAsync();
    }
}
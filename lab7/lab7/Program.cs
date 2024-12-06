namespace lab7;

class Program
{
    static async Task Main(string[] args)
    {
        var config = new Configuration
        {
            EmitterDelays = [20, 30, 150, 40], // Задержки генерации событий (мс)
            ProcessorDelays = [10, 12, 15], // Задержки обработки
            RecieverDelays = [2, 3], //Задержки получения
            SimulationDuration = 400 // Продолжительность моделирования (мс)
        };

        var system = new ConveyorSystem(config);
        await system.StartAsync();
    }
}
using System.Collections.Concurrent;
using System.ComponentModel;

namespace lab7;

public class ConveyorSystem
{
    private readonly Configuration _config;
    private readonly BlockingCollection<Event> _orderQueue = new();
    private readonly BlockingCollection<Event> _billingQueue = new();
    private readonly List<EventProcessor> _processors = new();
    private readonly List<EventReciever> _recievers = new();
    private readonly List<EventEmitter> _emitters = new();

    public ConveyorSystem(Configuration config)
    {
        _config = config;
        
        // Создаем эмиттер (стекольный завод)
        for (int i = 0; i < config.EmitterDelays.Length; ++i)
        {
            _emitters.Add(new EventEmitter(i + 1, config.EmitterDelays[i], _orderQueue));
        }
        
        // Создаем обработчики (сборщики поставок)
        for (int i = 0; i < config.ProcessorDelays.Length; ++i)
        {
            _processors.Add(new EventProcessor(i + 1, config.ProcessorDelays[i], _orderQueue, _billingQueue));
        }
        
        // Создаем получателей (поставок)
        for (int i = 0; i < config.RecieverDelays.Length; ++i)
        {
            _recievers.Add(new EventReciever(i + 1, config.RecieverDelays[i], _billingQueue));
        }
    }

    public async Task StartAsync()
    {
        Console.WriteLine("Запуск системы...");

        // Запуск эмиттера и обработчиков
        var emittersTask = _emitters.Select(e => e.StartAsync()).ToArray();
        var processorTasks = _processors.Select(p => p.StartAsync()).ToArray();
        var recieversTask = _recievers.Select(r => r.StartAsync()).ToArray();
        
        // Ожидание завершения моделирования
        await Task.Delay(_config.SimulationDuration);

        // Завершение работы
        foreach (var emitter in _emitters)
        {
            emitter.Stop();
        }
        foreach (var processor in _processors)
        {
            processor.Stop();
        }
        foreach (var reciever in _recievers)
        {
            reciever.Stop();
        }

        // Ждем завершения всех задач
        await Task.WhenAll(emittersTask);
        await Task.WhenAll(processorTasks);
        await Task.WhenAll(recieversTask);

        Log.Write("Система завершила работу.");
        LogMetrics();
    }

    private void LogMetrics()
    {
        Console.WriteLine("\n------------------------------------Сводка------------------------------------\n");
        Console.WriteLine($"---Конфигурация--- \nЗадержки стекольных заводов(в мс):");
        foreach (var delay in _config.EmitterDelays)
        {
            Console.WriteLine($"{delay} ");
        }
        Console.WriteLine("\nЗадержки курьерских компаний(в мс): ");
        foreach (var delay in _config.ProcessorDelays)
        {
            Console.WriteLine($"{delay} ");
        }
        Console.WriteLine("\nЗадержки получателей(в мс): ");
        foreach (var delay in _config.RecieverDelays)
        {
            Console.WriteLine($"{delay} ");
        }
        Console.WriteLine($"\nПродолжительность симуляции(в мс): {_config.SimulationDuration}\n");
        Console.WriteLine("\n---Результаты симуляции---");
        foreach (var emitter in _emitters)
        {
            Console.WriteLine($"Стекольный завод {emitter.Id}: Выполнено заказов: {emitter.EventsGenerated}");
        }
        Console.WriteLine("\n");
        foreach (var processor in _processors)
        {
            Console.WriteLine($"Курьерская компания {processor.Id}: Доставлено заказов: {processor.EventsProcessed}");
        }
        Console.WriteLine("\n");
        foreach (var reciever in _recievers)
        {
            Console.WriteLine($"Получатель {reciever.Id}: Получено заказов: {reciever.EventsProcessed}");
        }
    }
}
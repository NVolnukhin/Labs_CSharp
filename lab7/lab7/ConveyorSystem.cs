using System.Collections.Concurrent;

namespace lab7;

public class ConveyorSystem
{
    private readonly Configuration _config;
    private readonly BlockingCollection<Event> _eventQueue = new();
    private readonly List<EventProcessor> _processors = new();
    private readonly EventEmitter _emitter;

    public ConveyorSystem(Configuration config)
    {
        _config = config;

        // Создаем обработчики
        for (int i = 0; i < config.ProcessorDelays.Length; i++)
        {
            _processors.Add(new EventProcessor(i + 1, config.ProcessorDelays[i], _eventQueue));
        }

        // Создаем эмиттер
        _emitter = new EventEmitter(1, config.EmitterInterval, _eventQueue);
    }

    public async Task StartAsync()
    {
        Console.WriteLine("Запуск системы...");

        // Запуск эмиттера и обработчиков
        var emitterTask = _emitter.StartAsync();
        var processorTasks = _processors.Select(p => p.StartAsync()).ToArray();

        // Ожидание завершения моделирования
        await Task.Delay(_config.SimulationDuration);

        // Завершение работы
        _emitter.Stop();
        foreach (var processor in _processors)
        {
            processor.Stop();
        }

        // Ждем завершения всех задач
        await Task.WhenAll(emitterTask);
        await Task.WhenAll(processorTasks);

        Console.WriteLine("Система завершила работу.");
        LogMetrics();
    }

    private void LogMetrics()
    {
        Console.WriteLine("Сводка:");
        Console.WriteLine($"Всего сгенерировано событий: {_emitter.EventsGenerated}");
        foreach (var processor in _processors)
        {
            Console.WriteLine($"Обработчик {processor.Id}: Обработано событий: {EventProcessor.EventsProcessed}");
        }
    }
}
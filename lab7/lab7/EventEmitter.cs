using System.Collections.Concurrent;

namespace lab7;

public class EventEmitter
{
    private readonly int _id;
    private readonly int _interval;
    private readonly BlockingCollection<Event> _queue;
    private CancellationTokenSource _cts;
    private static int _eventsGenerated = 0;
    
    public int EventsGenerated => _eventsGenerated;

    public EventEmitter(int id, int interval, BlockingCollection<Event> queue)
    {
        _id = id;
        _interval = interval;
        _queue = queue;
        _cts = new CancellationTokenSource();
    }

    public Task StartAsync()
    {
        return Task.Run(async () =>
        {
            while (!_cts.Token.IsCancellationRequested)
            {
                var newEvent = new Event(Interlocked.Increment(ref _eventsGenerated));
                _queue.Add(newEvent);
                Log($"Эмиттер {_id} сгенерировал событие {newEvent}");
                await Task.Delay(_interval, _cts.Token);
            }
        });
    }

    public void Stop() => _cts.Cancel();

    private static void Log(string message)
    {
        Console.WriteLine($"{DateTime.Now:HH:mm:ss.fff} - {message}");
    }
}

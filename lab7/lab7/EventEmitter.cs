namespace lab7;

public class EventEmitter
{
    private readonly int _emitterId;
    private readonly int _intervalMs;
    private readonly Random _random;
    private readonly Queue _queue;

    public EventEmitter(int emitterId, int intervalMs, Queue queue)
    {
        _emitterId = emitterId;
        _intervalMs = intervalMs;
        _queue = queue;
        _random = new Random();
    }

    public async Task StartAsync(CancellationToken token)
    {
        int eventId = 0;
        while (!token.IsCancellationRequested)
        {
            await Task.Delay(_intervalMs + _random.Next(0, 1000), token);
            var ev = new Event(eventId++);
            _queue.Enqueue(ev);
            Console.WriteLine($"{DateTime.Now} - Эмиттер {_emitterId} сгенерировал событие {ev.Id}");
        }
    }
}
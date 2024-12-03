namespace lab7;

public class EventHandler
{
    private readonly int _handlerId;
    private readonly int _processingTimeMs;
    private readonly Queue _queue;

    public EventHandler(int handlerId, int processingTimeMs, Queue queue)
    {
        _handlerId = handlerId;
        _processingTimeMs = processingTimeMs;
        _queue = queue;
    }

    public async Task StartAsync(CancellationToken token)
    {
        while (!token.IsCancellationRequested)
        {
            if (_queue.TryDequeue(out var ev))
            {
                Console.WriteLine($"{DateTime.Now} - Обработчик {_handlerId} начал обработку события {ev.Id}");
                await Task.Delay(_processingTimeMs, token);
                Console.WriteLine($"{DateTime.Now} - Обработчик {_handlerId} обработал событие {ev.Id}");
            }
            else
            {
                await Task.Delay(100, token); // Ожидание появления события
            }
        }
    }
}

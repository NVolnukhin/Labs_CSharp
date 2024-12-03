using System.Collections.Concurrent;

namespace lab7;

public class EventReciever
{
    private readonly int _id;
    private readonly int _processingDelay;
    private readonly BlockingCollection<Event> _queue;
    private CancellationTokenSource _cts;
    private static int _eventsProcessed = 0;

    public int Id => _id;
    public static int EventsProcessed => _eventsProcessed;

    public EventReciever(int id, int processingDelay, BlockingCollection<Event> queue)
    {
        _id = id;
        _processingDelay = processingDelay;
        _queue = queue;
        _cts = new CancellationTokenSource();
    }

    public Task StartAsync()
    {
        return Task.Run(async () =>
        {
            while (!_cts.Token.IsCancellationRequested)
            {
                if (_queue.TryTake(out var eventItem, Timeout.Infinite, _cts.Token))
                {
                    await ProcessEvent(eventItem);
                }
            }
        });
    }

    public void Stop() => _cts.Cancel();

    private async Task ProcessEvent(Event eventItem)
    {
        Log.Write($"Обработчик {_id} начал обработку события {eventItem}");
        await Task.Delay(_processingDelay, _cts.Token);
        Interlocked.Increment(ref _eventsProcessed);
        Log.Write($"Обработчик {_id} завершил обработку события {eventItem}");
    }
}


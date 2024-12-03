using System.Collections.Concurrent;

namespace lab7;

public class EventReciever
{
    private readonly int _id;
    private readonly int _processingDelay;
    private readonly BlockingCollection<Event> _queue;
    private CancellationTokenSource _cts;
    private int _eventsProcessed = 0;

    public int Id => _id;
    public int EventsProcessed => _eventsProcessed;

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
            try
            {
                while (!_cts.Token.IsCancellationRequested)
                {
                    if (_queue.TryTake(out var eventItem, Timeout.Infinite, _cts.Token))
                    {
                        await ProcessEvent(eventItem);
                    }
                }
            }
            catch (OperationCanceledException)
            {
                Log.Write($"Получение заказов было отменено.");
            }
        });
    }

    public void Stop() => _cts.Cancel();

    private async Task ProcessEvent(Event eventItem)
    {
        Log.Write($"Получатель {_id} начал приемку заказа {eventItem}");
        await Task.Delay(_processingDelay, _cts.Token);
        Interlocked.Increment(ref _eventsProcessed);
        Log.Write($"Получатель {_id} завершил приемку заказа {eventItem}");
    }
}


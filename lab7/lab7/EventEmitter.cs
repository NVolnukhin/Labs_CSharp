using System.Collections.Concurrent;

namespace lab7;

public class EventEmitter
{
    private readonly int _id;
    private readonly int _delay;
    private readonly BlockingCollection<Event> _queue;
    private CancellationTokenSource _cts;
    private int _eventsGenerated = 0;
    
    public int Id => _id;
    public int EventsGenerated => _eventsGenerated;

    public EventEmitter(int id, int delay, BlockingCollection<Event> queue)
    {
        _id = id;
        _delay = delay;
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
                try
                {
                    await ProcessEvent(newEvent);
                }
                catch (TaskCanceledException)
                {
                    Log.Write($"Произвоодство заказа {newEvent.Id} было отменено.");
                }
            }
        });
    }

    public void Stop() => _cts.Cancel();
    
    private async Task ProcessEvent(Event eventItem)
    {
        Log.Write($"Стекольный завод {_id} начал выполнение заказа {eventItem}");
        await Task.Delay(_delay, _cts.Token);
        //Interlocked.Increment(ref _eventsGenerated);
        Log.Write($"Стекольный завод {_id} завершил выполнение заказа {eventItem}");

        // Передаем событие в следующую очередь
        _queue.Add(eventItem);
        Log.Write($"Стекольный завод {_id} передал заказ {eventItem} в доставку");
    }
}

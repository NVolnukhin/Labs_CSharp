using System.Collections.Concurrent;

namespace lab7;

public class EventProcessor
{
    private readonly int _id;
    private readonly int _processingDelay;
    private readonly BlockingCollection<Event> _inputQueue;
    private readonly BlockingCollection<Event> _outputQueue;
    private CancellationTokenSource _cts;
    private static int _eventProcessed = 0;

    public int Id => _id;

    public EventProcessor(
        int id,
        int processingDelay,
        BlockingCollection<Event> inputQueue,
        BlockingCollection<Event> outputQueue)
    {
        _id = id;
        _processingDelay = processingDelay;
        _inputQueue = inputQueue;
        _outputQueue = outputQueue;
        _cts = new CancellationTokenSource();
    }

    public Task StartAsync()
    {
        return Task.Run(async () =>
        {
            while (!_cts.Token.IsCancellationRequested)
            {
                if (_inputQueue.TryTake(out var eventItem, Timeout.Infinite, _cts.Token))
                {
                    await ProcessEvent(eventItem);
                }
            }
        });
    }

    public void Stop() => _cts.Cancel();

    private async Task ProcessEvent(Event eventItem)
    {
        Log.Write($"Курьерская компания {_id} начала доставку заказа {eventItem}");
        await Task.Delay(_processingDelay, _cts.Token);
        Interlocked.Increment(ref _eventProcessed);
        Log.Write($"Курьерская компания {_id} завершила доставку заказа {eventItem}");

        // Передаем событие в следующую очередь
        _outputQueue.Add(eventItem);
        Log.Write($"Курьерская компания {_id} передала заказ {eventItem} получателю");
    }
}
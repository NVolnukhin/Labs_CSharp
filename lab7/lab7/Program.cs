namespace lab7;

class Program
{
    static async Task Main(string[] args)
    {
        var queue = new Queue();

        //var emitter = new EventEmitter(emitterId: 1, intervalMs: 1000, queue);
        var handler1 = new EventProcessor(handlerId: 1, processingTimeMs: 1500, queue);
        var handler2 = new EventProcessor(handlerId: 2, processingTimeMs: 1200, queue);

        var cts = new CancellationTokenSource();

        //var emitterTask = emitter.StartAsync(cts.Token);
        var handlerTask1 = handler1.StartAsync(cts.Token);
        var handlerTask2 = handler2.StartAsync(cts.Token);
        Console.WriteLine("Нажмите любую клавишу для остановки...");
        Console.ReadKey();
        cts.Cancel();

        //await Task.WhenAll(emitterTask, handlerTask1, handlerTask2); 
        Console.WriteLine("Система остановлена.");
    }
}
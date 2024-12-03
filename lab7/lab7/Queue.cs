using System.Collections.Concurrent;
using lab7;


class Queue
{
    private ConcurrentQueue<Event> _queue = new();
    public void Enqueue(Event ev) => _queue.Enqueue(ev);
    public bool TryDequeue(out Event ev) => _queue.TryDequeue(out ev);
    public int Count => _queue.Count;
}
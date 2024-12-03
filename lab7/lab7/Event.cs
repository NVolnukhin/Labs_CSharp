using System;
using System.Collections.Concurrent;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace lab7;

public class Event
{
    public int Id { get; }
    public DateTime CreatedAt { get; }

    public Event(int id)
    {
        Id = id;
        CreatedAt = DateTime.Now;
    }
}
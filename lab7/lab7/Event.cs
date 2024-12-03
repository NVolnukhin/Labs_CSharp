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

    public override string ToString()
    {
        return $"Event {Id} (Создано: {CreatedAt:HH:mm:ss.fff})";
    }
}
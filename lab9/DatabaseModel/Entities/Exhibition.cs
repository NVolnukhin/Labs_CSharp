namespace DatabaseModel;

public class Exhibition
{
    private Exhibition(Guid id, string name, DateTime date)
    {
        Id = id;
        Name = name;
        Date = date;
    }
    
    public Guid Id { get; set; } // Primary Key
    public string Name { get; set; } = string.Empty; // Название выставки
    public DateTime Date { get; set; }
    public ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
    
    public static Exhibition Create(string name, DateTime date)
    {
        return new Exhibition(Guid.NewGuid(), name, date);
    }
}
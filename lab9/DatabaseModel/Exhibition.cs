namespace DatabaseModel;

public class Exhibition
{
    public int Id { get; set; } // Primary Key
    public string Name { get; set; } = string.Empty; // Название выставки
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }

    // Навигационное свойство для билетов
    public ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
}
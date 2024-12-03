namespace DatabaseModel;

public class Visitor
{
    public int Id { get; set; } // Primary Key
    public string FullName { get; set; } = string.Empty; // Имя посетителя
    public string Email { get; set; } = string.Empty;

    // Навигационное свойство для билетов
    public ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
}
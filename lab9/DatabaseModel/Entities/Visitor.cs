namespace DatabaseModel;

public class Visitor
{
    public Guid Id { get; init; } // PK
    public string FullName { get; init; } = string.Empty; // Имя посетителя
    public double Discount { get; set; } // Скидка 
    public ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
}
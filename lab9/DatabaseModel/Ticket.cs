namespace DatabaseModel;

public class Ticket
{
    public int Id { get; set; } // Primary Key
    public int ExhibitionId { get; set; } // FK на выставку
    public Exhibition Exhibition { get; set; } = null!;
    public int VisitorId { get; set; } // FK на посетителя
    public Visitor Visitor { get; set; } = null!;
    public double Discount { get; set; } // Скидка на билет
}
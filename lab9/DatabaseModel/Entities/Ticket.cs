namespace DatabaseModel;

public class Ticket
{
    public int Id { get; init; } // Primary Key
    public int ExhibitionId { get; init; } // FK на выставку
    public Exhibition Exhibition { get; set; } = null!;
    public int VisitorId { get; init; } // FK на посетителя
    public Visitor Visitor { get; set; } = null!;
}
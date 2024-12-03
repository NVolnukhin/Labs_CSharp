namespace DatabaseModel;

public class Ticket
{
    private Ticket(Guid id, Exhibition exhibition, string name, DateTime date)
    {
        Id = id;
        Name = name;
        Date = date;
    }
    
    public Guid Id { get; init; } // Primary Key
    public Guid ExhibitionId { get; init; } // FK на выставку
    public Exhibition Exhibition { get; set; } = null!;
    public Guid VisitorId { get; init; } // FK на посетителя
    public Visitor Visitor { get; set; } = null!;
}
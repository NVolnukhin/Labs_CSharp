namespace DatabaseModel;

public class Ticket
{
    private Ticket(Guid id, Guid exhibitionId, Exhibition exhibition, Guid visitorId, Visitor visitor)
    {
        Id = id;
        ExhibitionId = exhibitionId;
        Exhibition = exhibition;
        VisitorId = visitorId;
        Visitor = visitor;
    }
    
    public Guid Id { get; init; } // Primary Key
    public Guid ExhibitionId { get; init; } // FK на выставку
    public Exhibition Exhibition { get; set; }
    public Guid VisitorId { get; init; } // FK на посетителя
    public Visitor Visitor { get; set; }
    
    public static Ticket Create(Guid exhibitionId, Exhibition exhibition, Guid visitorId, Visitor visitor)
    {
        return new Ticket(Guid.NewGuid(), exhibitionId, exhibition, visitorId, visitor);
    } 
}
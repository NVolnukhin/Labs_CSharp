namespace DatabaseModel;

public class Ticket
{
    public Ticket()
    {
        throw new MemberAccessException("Use '.Create' method to create a new ticket.");
    }
    
    private Ticket(Guid id, Guid exhibitionId, Exhibition exhibition, Guid visitorId, Visitor visitor, double price)
    {
        Id = id;
        ExhibitionId = exhibitionId;
        Exhibition = exhibition;
        VisitorId = visitorId;
        Visitor = visitor;
        Price = price;
    }
    
    public Guid Id { get; init; } // Primary Key
    public Guid ExhibitionId { get; init; } // FK на выставку
    public Exhibition Exhibition { get; set; }
    public Guid VisitorId { get; init; } // FK на посетителя
    public Visitor Visitor { get; set; }
    public double Price { get; set; }
    
    public static Ticket Create(Guid exhibitionId, Exhibition exhibition, Guid visitorId, Visitor visitor, double price)
    {
        return new Ticket(Guid.NewGuid(), exhibitionId, exhibition, visitorId, visitor, price);
    } 
}
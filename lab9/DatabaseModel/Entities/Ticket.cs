namespace DatabaseModel;

public class Ticket
{
    private Ticket(Guid id, Guid exhibitionId, Guid visitorId, double price)
    {
        Id = id;
        ExhibitionId = exhibitionId;
        VisitorId = visitorId;
        Price = price;
    }
    
    public Guid Id { get; init; } // Primary Key
    public Guid ExhibitionId { get; set; } // FK на выставку
    public Guid VisitorId { get; set; } // FK на посетителя
    public double Price { get; set; }
    
    public static Ticket Create(Guid exhibitionId, Guid visitorId, double price)
    {
        return new Ticket(Guid.NewGuid(), exhibitionId, visitorId, price);
    } 
}
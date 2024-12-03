namespace DatabaseModel;

public class Visitor
{
    private Visitor(Guid id, string fullName, double discount)
    {
        Id = id;
        FullName = fullName;
        Discount = discount;
    }
    public Guid Id { get; init; } // PK
    public string FullName { get; init; } = string.Empty; // Имя посетителя
    public double Discount { get; set; } // Скидка 
    public ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
    
    public static Visitor Create(string fullName, double discount)
    {
        return new Visitor(Guid.NewGuid(), fullName, discount);
    } 
        
}
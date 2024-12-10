using DatabaseModel;
using Microsoft.EntityFrameworkCore;

namespace DatabaseContext.Repositories;

public class TicketRepository
{
    private readonly AppDbContext _appDbContext;
    
    public TicketRepository(AppDbContext dbContext)
    {
        _appDbContext = dbContext;
    }
    
    public async Task Add(Ticket ticket)
    {
        await _appDbContext.Tickets.AddAsync(ticket);
        Console.WriteLine($"Added ticket {ticket.Id}");
        try
        {
            await _appDbContext.SaveChangesAsync();
            Console.WriteLine($"Saved ticket {ticket.Id}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to add exhibition {ticket.Id}. \nReason: {ex}");
        }
    }
    
    public async Task Update(Guid ticketId, Guid exhibitionId, Guid visitorId, double price)
    {
        var exhibition = await _appDbContext.Exhibitions
            .FirstOrDefaultAsync(exhibition => exhibition.Id == exhibitionId);
        
        var visitor = await _appDbContext.Visitors
            .FirstOrDefaultAsync(visitor => visitor.Id == visitorId);
        
        
        await _appDbContext.Tickets
            .Where(t => t.Id == ticketId)
            .ExecuteUpdateAsync(t => t
                .SetProperty(p => p.ExhibitionId, exhibitionId)
                .SetProperty(p => p.Exhibition, exhibition)
                .SetProperty(p => p.VisitorId, visitorId)
                .SetProperty(p => p.Visitor, visitor)
                .SetProperty(p => p.Price, price)
            );
    }

    public async Task Delete(Guid ticketId)
    {
        await _appDbContext.Tickets
            .Where(t => t.Id == ticketId)
            .ExecuteDeleteAsync();
    }
}
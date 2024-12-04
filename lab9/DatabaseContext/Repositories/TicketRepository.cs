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
        await _appDbContext.SaveChangesAsync();
    }
    
    public async Task Update(Guid ticketId, Guid exhibitionId, Exhibition exhibition, Guid visitorId, Visitor visitor)
    {
        await _appDbContext.Tickets
            .Where(t => t.Id == ticketId)
            .ExecuteUpdateAsync(t => t
                .SetProperty(p => p.ExhibitionId, exhibitionId)
                .SetProperty(p => p.Exhibition, exhibition)
                .SetProperty(p => p.Visitor, visitor)
                .SetProperty(p => p.VisitorId, visitorId)
            );
    }

    public async Task Delete(Guid ticketId)
    {
        await _appDbContext.Tickets
            .Where(t => t.Id == ticketId)
            .ExecuteDeleteAsync();
    }
}
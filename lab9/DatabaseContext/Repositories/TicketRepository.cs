using DatabaseModel;

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
}
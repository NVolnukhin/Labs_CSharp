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
        Console.WriteLine($"Добавлен билет {ticket.Id}");
        try
        {
            await _appDbContext.SaveChangesAsync();
            Console.WriteLine($"Сохранен билет {ticket.Id}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Не удалось сохранить билет {ticket.Id}. \nПричина: {ex}");
        }
    }
    
    public async Task Update(Guid ticketId, Guid exhibitionId, Guid visitorId, double price)
    {
        await _appDbContext.Tickets
            .Where(t => t.Id == ticketId)
            .ExecuteUpdateAsync(t => t
                .SetProperty(p => p.ExhibitionId, exhibitionId)
                .SetProperty(p => p.VisitorId, visitorId)
                .SetProperty(p => p.Price, price)
            );
    }

    public async Task Delete(Guid ticketId)
    {
        await _appDbContext.Tickets
            .Where(t => t.Id == ticketId)
            .ExecuteDeleteAsync();
    }

    public async Task DeleteByExhibitionId(Guid exhibitionId)
    {
        await _appDbContext.Tickets
            .Where(t => t.ExhibitionId == exhibitionId)
            .ExecuteDeleteAsync();
    }

    public async Task DeleteByVisitorId(Guid visitorId)
    {
        await _appDbContext.Tickets
            .Where(t => t.VisitorId == visitorId)
            .ExecuteDeleteAsync();
    }
}
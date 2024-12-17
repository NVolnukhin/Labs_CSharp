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
        var ticket = await _appDbContext.Tickets
            .FirstAsync(t => t.Id == ticketId);
        
        ticket.ExhibitionId = exhibitionId;
        ticket.VisitorId = visitorId;
        ticket.Price = price;
        
        await _appDbContext.SaveChangesAsync();
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

    public IQueryable<Ticket> GetTicketQuery()
    {
        return 
            from ticket in _appDbContext.Tickets
            select ticket;
    }

    public async Task<Ticket?> GetById(Guid ticketId)
    {
        return await _appDbContext.Tickets
            .FirstOrDefaultAsync(t => t.Id == ticketId);
    }

    public IQueryable<double> TotalSpendQuery(string visitorName)
    {
        return
            from ticket in _appDbContext.Tickets
            join visitor in _appDbContext.Visitors on ticket.VisitorId equals visitor.Id
            where visitor.FullName == visitorName
            select ticket.Price * (100 - visitor.Discount) / 100;
    }
}
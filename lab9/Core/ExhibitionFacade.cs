using DatabaseContext;
using DatabaseModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

namespace Core;

public class ExhibitionFacade
{
    private readonly AppDbContext _context;

    public ExhibitionFacade(AppDbContext context)
    {
        _context = context;
    }

    // Сколько билетов продано на выставку
    public async Task<int> GetTicketsSoldAsync(int exhibitionId)
    {
        return await _context.Tickets.CountAsync(t => t.ExhibitionId == exhibitionId);
    }

    // На сколько уникальных выставок сходил посетитель
    public async Task<int> GetUniqueExhibitionsVisitedAsync(int visitorId)
    {
        return await _context.Tickets
            .Where(t => t.VisitorId == visitorId)
            .Select(t => t.ExhibitionId)
            .Distinct()
            .CountAsync();
    }

    // Средний процент скидки на выставку
    public async Task<double> GetAverageDiscountAsync(int exhibitionId)
    {
        var query = _context.Tickets
            .Where(ticket => ticket.ExhibitionId == exhibitionId)
            .Join<Ticket, Visitor, object, double>(
                _context.Visitors, 
                ticket => ticket.VisitorId,
                visitor => visitor.Id,
                (ticket, visitor) => visitor.Discount)
            .DefaultIfEmpty(0);
        
        var discounts = await query.ToListAsync();

        return discounts.Count != 0 ? discounts.Average() : 0.0;
    }
}
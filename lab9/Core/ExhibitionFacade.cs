using DatabaseContext;
using DatabaseModel;
using Microsoft.EntityFrameworkCore;

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
        var discounts = await _context.Tickets
            .Where(t => t.ExhibitionId == exhibitionId)
            .Select(t => t.Discount)
            .ToListAsync();

        return discounts.Count != 0 ? discounts.Average() : 0.0;
    }
}
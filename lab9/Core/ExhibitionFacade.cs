using Core.Services;
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
    public async Task<int> GetTicketsSoldAsync(string exhibitionName)
    {
        var ticketsQuery =
            from ticket in _context.Tickets
            join exhibition in _context.Exhibitions on ticket.ExhibitionId equals exhibition.Id
            where exhibition.Name == exhibitionName
            select ticket;
        
        var amount = await ticketsQuery.CountAsync();

        return amount;
    }

    // На сколько уникальных выставок сходил посетитель
    public async Task<int> GetUniqueExhibitionsVisitedAsync(string visitorName)
    {
        var exhibitionsQuery =
            from ticket in _context.Tickets
            join visitor in _context.Visitors on ticket.VisitorId equals visitor.Id
            where visitor.FullName == visitorName
            select visitor.Discount;
        
        var amount = await exhibitionsQuery.CountAsync();


        return amount;
    }

    // Средний процент скидки на выставку
    public async Task<double> GetAverageDiscountAsync(string exhibitionName)
    {
        var discountsQuery =
            from ticket in _context.Tickets
            join visitor in _context.Visitors on ticket.VisitorId equals visitor.Id
            join exhibition in _context.Exhibitions on ticket.ExhibitionId equals exhibition.Id
            where exhibition.Name == exhibitionName
            select visitor.Discount;
        
        var discounts = await discountsQuery.ToListAsync();

        return discounts.Count != 0 ? discounts.Average() : 0.0;
    }
}
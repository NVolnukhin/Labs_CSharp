using Core.Services;
using DatabaseContext;
using DatabaseContext.Repositories;
using DatabaseModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

namespace Core;

public class ExhibitionFacade
{
    private readonly AppDbContext _context;
    //private readonly ExhibitionRepository _exhibitionRepository;
    //private readonly VisitorRepository _visitorRepository;
    //private readonly TicketRepository _ticketRepository;

    public ExhibitionFacade(AppDbContext context)
    {
        _context = context;
    }
    
    public async Task AddExhibition()
    {
        try
        {
            Console.Write("Введите название выставки: ");
            var name = Console.ReadLine();
            Console.Write("Enter Start Date (yyyy-MM-dd): ");
            var date = DateTime.Parse(Console.ReadLine());
                        
            var ex = Exhibition.Create(name, date);
            Console.WriteLine($"ex {ex.Id} created");
            
            _context.Exhibitions.Add(ex);
            await _context.SaveChangesAsync();
        }
        catch (FormatException)
        {
            Console.WriteLine("Неверный формат вводимых данных");
            throw;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
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
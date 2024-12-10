using DatabaseContext;
using DatabaseContext.Repositories;
using DatabaseModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

namespace Core;

public class ExhibitionFacade
{
    private readonly AppDbContext _context;
    private readonly ExhibitionRepository _exhibitionRepository;
    private readonly VisitorRepository _visitorRepository;
    private readonly TicketRepository _ticketRepository;

    public ExhibitionFacade(AppDbContext context)
    {
        _context = context;
        _exhibitionRepository = new ExhibitionRepository(context);
        _visitorRepository = new VisitorRepository(context);
        _ticketRepository = new TicketRepository(context);
    }
    
    public async Task AddExhibition()
    {
        try
        {
            Console.Write("Введите название выставки: ");
            var name = Console.ReadLine() ?? "Concert";
            Console.Write("Enter Start Date (yyyy-MM-dd): ");
            DateTime date = DateTime.Parse(Console.ReadLine() ?? "01.01.2024 00:00:00").ToUniversalTime();
                        
            var ex = Exhibition.Create(name, date);
            Console.WriteLine($"ex {ex.Id} created");
            
            await _exhibitionRepository.Add(ex);
            Console.WriteLine("ex added");
        }
        catch (FormatException)
        {
            Console.WriteLine("Неверный формат вводимых данных");
            throw;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }

    public async Task DeleteExhibition()
    {
        try
        {
            Console.Write("Введите ID выставки: ");
            var id = Guid.Parse(Console.ReadLine());
            await _exhibitionRepository.Delete(id);
            Console.WriteLine("ex deleted");
        }
        catch (FormatException)
        {
            Console.WriteLine("Неверный формат вводимых данных");
            throw;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }
    
    public async Task UpdateExhibition()
    {
        try
        {
            Console.Write("Введите ID выставки: ");
            var id = Guid.Parse(Console.ReadLine());
            Console.Write("Введите название выставки: ");
            var name = Console.ReadLine() ?? "Concert";
            Console.Write("Enter Start Date (yyyy-MM-dd): ");
            var date = DateTime.Parse(Console.ReadLine() ?? "01.01.2024 00:00:00").ToUniversalTime();
            await _exhibitionRepository.Update(id, name, date);
            Console.WriteLine("ex updated");
        }
        catch (FormatException)
        {
            Console.WriteLine("Неверный формат вводимых данных");
            throw;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }
    
    public async Task GetAllExhibitions()
    {
        var exhibitions =
            from exhibition in _context.Exhibitions
            select exhibition;
        
        var list = await exhibitions.ToListAsync();

        foreach (var exhibition in list)
            Console.WriteLine($"exhibition id: {exhibition.Id}, name: {exhibition.Name}, date: {exhibition.Date}");
    }

    public async Task AddVisitor()
    {
        try
        {
            Console.Write("Введите полное имя: ");
            var name = Console.ReadLine() ?? "Иванов Иван Иваныч";
            Console.Write("Введите скидку посетителя: ");
            var discount = double.Parse(Console.ReadLine() ?? "0.0");
            var visitor = Visitor.Create(name, discount);
            Console.WriteLine($"ex {visitor.Id} created");
            
            await _visitorRepository.Add(visitor);
            Console.WriteLine("Посетитель добавлен");
        }
        catch (FormatException)
        {
            Console.WriteLine("Неверный формат вводимых данных");
            throw;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }

    public async Task DeleteVisitor()
    {
        try
        {
            Console.Write("Введите ID посетителя: ");
            var id = Guid.Parse(Console.ReadLine());
            await _visitorRepository.Delete(id);
            Console.WriteLine("Посетитель удален");
        }
        catch (FormatException)
        {
            Console.WriteLine("Неверный формат вводимых данных");
            throw;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }
    
    public async Task UpdateVisitor()
    {
        try
        {
            Console.Write("Введите ID посетителя: ");
            var id = Guid.Parse(Console.ReadLine());
            Console.Write("Введите полное имя посетителя: ");
            var name = Console.ReadLine() ?? "Иванов Иван Иваныч";
            Console.Write("Введите скидку посетителя: ");
            var discount = double.Parse(Console.ReadLine() ?? "0.0");
            await _visitorRepository.Update(id, name, discount);
            Console.WriteLine("Посетитель обновлен");
        }
        catch (FormatException)
        {
            Console.WriteLine("Неверный формат вводимых данных");
            throw;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }
    
    
    public async Task GetAllVisitors()
    {
        var visitors =
            from visitor in _context.Visitors
            select visitor;
        
        var list = await visitors.ToListAsync();

        foreach (var visitor in list)
            Console.WriteLine($"ID посетителя: {visitor.Id}, полное имя: {visitor.FullName}, скидка: {visitor.Discount}");
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
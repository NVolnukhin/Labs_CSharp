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
            var date = DateTime.Parse(Console.ReadLine() ?? "01.01.2024 00:00:00").ToUniversalTime();
                        
            var ex = Exhibition.Create(name, date);
            Console.WriteLine($"Создана выставка {ex.Id}");
            
            await _exhibitionRepository.Add(ex);
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
        
        var count = await exhibitions.CountAsync();
        if (count == 0)
        {
            Console.WriteLine($"Нет выставок в базе данных");
            return;
        }
        
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
            Console.WriteLine($"Создан посетитель {visitor.Id}");
            
            await _visitorRepository.Add(visitor);
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
        
        var count = await visitors.CountAsync();
        if (count == 0)
        {
            Console.WriteLine($"Нет посетителей в базе данных");
            return;
        }
        var list = await visitors.ToListAsync();

        foreach (var visitor in list)
            Console.WriteLine($"ID посетителя: {visitor.Id}, полное имя: {visitor.FullName}, скидка: {visitor.Discount}");
    }

    
    public async Task AddTicket() 
    {
        try
        {
            Console.Write("Введите полное имя посетителя: ");
            var fullName = Console.ReadLine();
            Console.Write("Введите название выставки: ");
            var exhibitionName = Console.ReadLine();
            Console.Write("Введите стоимость билета: ");
            var price = double.Parse(Console.ReadLine());
            
            var exhibition = await _exhibitionRepository.GetByName(exhibitionName);
            var visitor = await _visitorRepository.GetByName(fullName);
            
            var ticket = Ticket.Create(exhibition.Id, visitor.Id, price);
            
            
            Console.WriteLine($"Создан билет {ticket.Id}");
            
            await _ticketRepository.Add(ticket);
            Console.WriteLine("Билет добавлен");
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

    public async Task DeleteTicket()
    {
        try
        {
            Console.Write("Введите ID билета: ");
            var id = Guid.Parse(Console.ReadLine());
            await _ticketRepository.Delete(id);
            Console.WriteLine("Билет удален");
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
    
    public async Task UpdateTicket()  //TODO
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
    
    public async Task GetAllTickets()
    {
        var tickets =
            from ticket in _context.Tickets
            select ticket;

        var count = await tickets.CountAsync();
        if (count == 0)
        {
            Console.WriteLine($"Нет билетов в базе данных");
            return;
        }
        
        var list = await tickets.ToListAsync();

        foreach (var ticket in list)
        {
            var exhibition = await _exhibitionRepository.GetById(ticket.ExhibitionId);
            var visitor = await _visitorRepository.GetById(ticket.VisitorId);
            Console.WriteLine($"ID билета: {ticket.Id} | Выставка: {exhibition.Name}, Посетитель: {visitor.FullName}, Цена: {ticket.Price:0.00}(по скидке - {ticket.Price * (100 - visitor.Discount) / 100:0.00})");
        }
            
    }

    
    // Сколько билетов продано на выставку
    public async Task<int> GetTicketsSoldByName(string exhibitionName) //TODO
    {
        var ticketsQuery =
            from ticket in _context.Tickets
            join exhibition in _context.Exhibitions on ticket.ExhibitionId equals exhibition.Id
            where exhibition.Name == exhibitionName
            select ticket;
        
        var amount = await ticketsQuery.CountAsync();

        return amount;
    }
    
    public async Task<int> GetTicketsSoldById(Guid id) //TODO
    {
        var ticketsQuery =
            from ticket in _context.Tickets
            join exhibition in _context.Exhibitions on ticket.ExhibitionId equals exhibition.Id
            where exhibition.Id == id
            select ticket;
        
        var amount = await ticketsQuery.CountAsync();

        return amount;
    }

    // На сколько уникальных выставок сходил посетитель
    public async Task<int> GetUniqueExhibitionsVisitedAsync(string visitorName) //TODO
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
    public async Task<double> GetAverageDiscountAsync(string exhibitionName) //TODO
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
using DatabaseContext;
using DatabaseContext.Repositories;
using DatabaseModel;
using Microsoft.EntityFrameworkCore;

namespace Core;

public class ExhibitionFacade
{
    private readonly AppDbContext _context;
    private readonly ExhibitionRepository _exhibitionRepository;
    private readonly NamesGetter _namesGetter;

    public ExhibitionFacade(AppDbContext context)
    {
        _context = context;
        _exhibitionRepository = new ExhibitionRepository(context);
        _namesGetter = new NamesGetter(context);
    }
    
    public async Task AddExhibition()
    {
        try
        {
            Console.Write("Введите название выставки: ");
            var name = Console.ReadLine() ?? "Concert";
            Console.Write("Enter Start Date (yyyy-MM-dd): ");
            var date = DateTime.Parse(Console.ReadLine() ?? "01.01.2024 00:00:00").ToUniversalTime();
                        
            var exhibition = Exhibition.Create(name, date);
            Console.WriteLine($"Создана выставка {exhibition.Id}");
            
            await _exhibitionRepository.Add(exhibition);
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
            Console.WriteLine("Выставка удалена");
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
            Console.WriteLine("Выставка обновлена");
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
        Console.WriteLine("--------------------------------------------------------------------------------------------------------------------------------");
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
            Console.WriteLine($"ID выставки: {exhibition.Id} | Название: {exhibition.Name, -20} | Дата: {exhibition.Date.ToLongDateString()}");
        Console.WriteLine("--------------------------------------------------------------------------------------------------------------------------------\n");
    }
    
    // Сколько билетов продано на выставку
    public async Task GetTicketsSold()
    {
        await _namesGetter.GetAllExhibitionNames();
     
        Console.Write("Введите название выставки: ");
        var exhibitionName = Console.ReadLine()!;
        
        var ticketsQuery =
            from ticket in _context.Tickets
            join exhibition in _context.Exhibitions on ticket.ExhibitionId equals exhibition.Id
            where exhibition.Name == exhibitionName
            select ticket;
        
        var amount = await ticketsQuery.CountAsync();

        Console.WriteLine($"Продано билетов: {amount}\n");
    }

    // На сколько уникальных выставок сходил посетитель
    public async Task GetUniqueExhibitionsVisited()
    {
        await _namesGetter.GetAllVisitorsNames();
        
        Console.Write("Введите имя посетителя: ");
        var visitorName = Console.ReadLine()!;
        
        var exhibitionsQuery =
            from ticket in _context.Tickets
            join visitor in _context.Visitors on ticket.VisitorId equals visitor.Id
            where visitor.FullName == visitorName
            select ticket.ExhibitionId;
        
        var totalSpendQuery =
            from ticket in _context.Tickets
            join visitor in _context.Visitors on ticket.VisitorId equals visitor.Id
            where visitor.FullName == visitorName
            select ticket.Price * (100 - visitor.Discount) / 100;
        
        var distinctAmount = await exhibitionsQuery
            .Distinct()
            .CountAsync();
        
        var totalAmount = await exhibitionsQuery
            .CountAsync();

        var totalSpend = await totalSpendQuery
            .SumAsync();
        
        Console.WriteLine($"Посещено уникальных выставок: {distinctAmount}");
        Console.WriteLine($"Куплено всего билетов: {totalAmount}");
        Console.WriteLine($"Всего потрачено денег на выставки: {totalSpend:0.00}\n");
        
    }

    // Средний процент скидки на выставку
    public async Task GetAverageDiscount()
    {
        await _namesGetter.GetAllExhibitionNames();
        
        Console.Write("Введите название выставки: ");
        var exhibitionName = Console.ReadLine()!;
        
        var visitorsQuery =
            from ticket in _context.Tickets
            join visitor in _context.Visitors on ticket.VisitorId equals visitor.Id
            join exhibition in _context.Exhibitions on ticket.ExhibitionId equals exhibition.Id
            where exhibition.Name == exhibitionName
            select visitor;

        var avgDiscount = await visitorsQuery
            .Distinct()
            .Select(v => v.Discount)
            .AverageAsync();
        
        Console.WriteLine($"Средняя скидка: {avgDiscount:0.00}%\n");
    }
}
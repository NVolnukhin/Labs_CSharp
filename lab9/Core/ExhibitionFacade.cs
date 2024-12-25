using DatabaseContext;
using DatabaseContext.Repositories;
using DatabaseModel;
using Microsoft.EntityFrameworkCore;

namespace Core;

public class ExhibitionFacade
{
    private readonly ExhibitionRepository _exhibitionRepository;
    private readonly TicketRepository _ticketRepository;
    private readonly VisitorRepository _visitorRepository;

    public ExhibitionFacade(AppDbContext context)
    {
        _exhibitionRepository = new ExhibitionRepository(context);
        _ticketRepository = new TicketRepository(context);
        _visitorRepository = new VisitorRepository(context);
    }
    
    public async Task AddExhibition()
    {
        try
        {
            Console.Write("Введите название выставки: ");
            var name = Console.ReadLine()!;
            if (name.Length < 3)
            {
                throw new Exception("Название не может быть короче 3 символов");
            }
            
            Console.Write("Введите дату проведения выставки (дд-мм-гггг): ");
            var date = DateTime.Parse(Console.ReadLine()!).ToUniversalTime().AddDays(1);
            
            var exhibition = Exhibition.Create(name, date);
            Console.WriteLine($"Создана выставка {exhibition.Id}");
            
            await _exhibitionRepository.Add(exhibition);
        }
        catch (FormatException)
        {
            Console.WriteLine("Неверный формат вводимых данных");
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
    }

    public async Task DeleteExhibition()
    {
        try
        {
            await GetAllExhibitions();
            
            Console.Write("Введите ID выставки: ");
            var id = Guid.Parse(Console.ReadLine()!);
            
            if (await _exhibitionRepository.GetById(id) == null)
            {
                throw new Exception("Выставка не найдена");
            }
            
            await _ticketRepository.DeleteByExhibitionId(id);
            Console.WriteLine("Билеты на выставку удалены");
            await _exhibitionRepository.Delete(id);
            Console.WriteLine("Выставка удалена");
        }
        catch (FormatException)
        {
            Console.WriteLine("Неверный формат ID");
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
    }
    
    public async Task UpdateExhibition()
    {
        try
        {
            await GetAllExhibitions();
            
            Console.Write("Введите ID выставки: ");
            var id = Guid.Parse(Console.ReadLine()!);
            if (await _exhibitionRepository.GetById(id) == null)
            {
                throw new Exception("Выставки с данным ID не существует");
            }
            
            Console.Write("Введите название выставки: ");
            var name = Console.ReadLine()!;
            if (name.Length < 3)
            {
                throw new Exception("Название не может быть короче 3 символов");
            }
            
            Console.Write("Enter Start Date (yyyy-MM-dd): ");
            var date = DateTime.Parse(Console.ReadLine()!).ToUniversalTime();
            
            await _exhibitionRepository.Update(id, name, date);
            Console.WriteLine("Выставка обновлена");
        }
        catch (FormatException)
        {
            Console.WriteLine("Неверный формат вводимых данных");
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
    }
    
    public async Task GetAllExhibitions()
    {
        Console.WriteLine("--------------------------------------------------------------------------------------------------------------------------------");
        var exhibitions = _exhibitionRepository.GetExhibitionQuery();
        
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
        await _exhibitionRepository.GetAllExhibitionNames();

        Console.Write("Введите название выставки: ");
        var exhibitionName = Console.ReadLine()!;
        var exhibitionId = await _exhibitionRepository.GetIdByName(exhibitionName);
        
        if (exhibitionId == null)
        {
            throw new Exception($"Выставки {exhibitionName} не существует");
        }

        var amount = await _ticketRepository.GetTicketAmountForExhibition(exhibitionId);

        Console.WriteLine(
            amount == 0
                ? $"На данную выставку билеты еще не были проданы\n"
                : $"Продано билетов: {amount}\n"
        );
        
    }

    // На сколько уникальных выставок сходил посетитель
    public async Task GetUniqueExhibitionsVisited()
    {
        await _visitorRepository.GetAllVisitorsNames();
        
        Console.Write("Введите имя посетителя: ");
        var visitorName = Console.ReadLine()!;
        var visitorId = await _visitorRepository.GetIdByName(visitorName);

        if (visitorId == null)
        {
            throw new Exception($"Посетитель {visitorName} не найден");
        }

        
        var ticketsQuery =
            _ticketRepository.GetTicketQuery();
    
        var totalSpendQuery = _ticketRepository.TotalSpendQuery(visitorName);
    
        var distinctAmount = await ticketsQuery
            .Where(t => t.VisitorId == visitorId)
            .GroupBy(t => t.ExhibitionId)
            .CountAsync();
    
        var totalAmount = await ticketsQuery
            .Where(t => t.VisitorId == visitorId)
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
        await _exhibitionRepository.GetAllExhibitionNames();
        
        Console.Write("Введите название выставки: ");
        var exhibitionName = Console.ReadLine()!;

        if (await _exhibitionRepository.GetIdByName(exhibitionName) == null)
        {
            throw new Exception($"Выставки {exhibitionName} не существует");
        }
        
        var visitorsQuery = _exhibitionRepository.GetExhibitionVisitorsQuery(exhibitionName);
    
        var count = await visitorsQuery.CountAsync();

        if (count == 0)
        {
            Console.WriteLine($"Посетителей выставки {exhibitionName} не найдено\n");
        }
        else
        {
            var avgDiscount = await visitorsQuery
                .Distinct()
                .Select(v => v.Discount)
                .AverageAsync();

            Console.WriteLine($"Средняя скидка: {avgDiscount:0.00}%\n");
        }
    }
}
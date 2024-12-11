using DatabaseContext;
using DatabaseContext.Repositories;
using DatabaseModel;
using Microsoft.EntityFrameworkCore;

namespace Core;

public class TicketFacade
{
    private readonly AppDbContext _context;
    private readonly ExhibitionRepository _exhibitionRepository;
    private readonly VisitorRepository _visitorRepository;
    private readonly TicketRepository _ticketRepository;
    private readonly NamesGetter _namesGetter;

    public TicketFacade(AppDbContext context)
    {
        _context = context;
        _exhibitionRepository = new ExhibitionRepository(context);
        _visitorRepository = new VisitorRepository(context);
        _ticketRepository = new TicketRepository(context);
        _namesGetter = new NamesGetter(context);
    }
    
     public async Task AddTicket() 
    {
        try
        {
            await _namesGetter.GetAllVisitorsNames();
            Console.Write("Введите полное имя посетителя: ");
            var fullName = Console.ReadLine();
            
            var allVisitorNames = await _namesGetter.GetVisitorsNamesList();
            if (allVisitorNames.All(name => name != fullName))
            {
                throw new Exception($"Пользователя {fullName} не существует");
            }
            
            await _namesGetter.GetAllExhibitionNames();
            Console.Write("Введите название выставки: ");
            var exhibitionName = Console.ReadLine();
            
            var allExhibitionNames = await _namesGetter.GetExhibitionNamesList();
            if (allExhibitionNames.All(name => name != exhibitionName))
            {
                throw new Exception($"Выставки {exhibitionName} не существует");
            }
            
            Console.Write("Введите стоимость билета: ");
            var price = double.Parse(Console.ReadLine()!);
            if (price < 0)
            {
                throw new Exception("Стоимость билета не может быть меньше 0");
            }
            
            var exhibitionId = await _exhibitionRepository.GetIdByName(exhibitionName!);
            var visitorId = await _visitorRepository.GetIdByName(fullName!);
            
            var ticket = Ticket.Create(exhibitionId, visitorId, price);
            Console.WriteLine($"Создан билет {ticket.Id}");
            
            await _ticketRepository.Add(ticket);
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

    public async Task DeleteTicket()
    {
        try
        {
            await GetAllTickets();
            Console.Write("Введите ID билета: ");
            var id = Guid.Parse(Console.ReadLine()!);
            
            var guids = await _namesGetter.GetTicketsGuidsList();
            if (guids.Any(g => g == id))
            {
                await _ticketRepository.Delete(id);
                Console.WriteLine("Билет удален");
            }
            else
            {
                Console.WriteLine("Билет не найден");
            }
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
    
    public async Task UpdateTicket()
    {
        try
        {
            await GetAllTickets();
            
            Console.Write("Введите ID билета: ");
            var id = Guid.Parse(Console.ReadLine()!);
            
            var allTicketGuids = await _namesGetter.GetTicketsGuidsList();
            if (allTicketGuids.All(g => g != id))
            {
                throw new Exception($"Такого билета не существует");
            }
            
            await _namesGetter.GetAllVisitorsNames();
            Console.Write("Введите полное имя посетителя: ");
            var fullName = Console.ReadLine();
            
            var allVisitorNames = await _namesGetter.GetVisitorsNamesList();
            if (allVisitorNames.All(name => name != fullName))
            {
                throw new Exception($"Пользователя {fullName} не существует");
            }
            
            await _namesGetter.GetAllExhibitionNames();
            Console.Write("Введите название выставки: ");
            var exhibitionName = Console.ReadLine();
            
            var allExhibitionNames = await _namesGetter.GetExhibitionNamesList();
            if (allExhibitionNames.All(name => name != exhibitionName))
            {
                throw new Exception($"Выставки {exhibitionName} не существует");
            }
            
            Console.Write("Введите стоимость билета: ");
            var price = double.Parse(Console.ReadLine()!);
            if (price < 0)
            {
                throw new Exception("Стоимость билета не может быть меньше 0");
            }
            
            var exhibitionId = await _exhibitionRepository.GetIdByName(exhibitionName!);
            var visitorId = await _visitorRepository.GetIdByName(fullName!);

            await _ticketRepository.Update(id, exhibitionId, visitorId, price);
            Console.WriteLine("Билет обновлен");
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
    
    public async Task GetAllTickets()
    {
        Console.WriteLine("------------------------------------------------------------------------------------------------------------------------------------------------------------------");
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
            Console.WriteLine(
                $"ID билета: {ticket.Id} | Выставка: {exhibition.Name,-20} | Посетитель: {visitor.FullName,-20} | Цена: {ticket.Price,-10:0.00}(Со скидкой {visitor.Discount}% - {ticket.Price * (100 - visitor.Discount) / 100:0.00})");
        }
        Console.WriteLine("------------------------------------------------------------------------------------------------------------------------------------------------------------------\n");
    }
}
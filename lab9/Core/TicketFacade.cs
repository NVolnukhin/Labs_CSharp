using DatabaseContext;
using DatabaseContext.Repositories;
using DatabaseModel;
using Microsoft.EntityFrameworkCore;

namespace Core;

public class TicketFacade
{
    private readonly ExhibitionRepository _exhibitionRepository;
    private readonly VisitorRepository _visitorRepository;
    private readonly TicketRepository _ticketRepository;

    public TicketFacade(AppDbContext context)
    {
        _exhibitionRepository = new ExhibitionRepository(context);
        _visitorRepository = new VisitorRepository(context);
        _ticketRepository = new TicketRepository(context);
    }
    
     public async Task AddTicket() 
    {
        try
        {
            await _visitorRepository.GetAllVisitorsNames();
            Console.Write("Введите полное имя посетителя: ");
            var fullName = Console.ReadLine();
            
            if (await _visitorRepository.GetByName(fullName!) == null)
            {
                throw new Exception($"Пользователя {fullName} не существует");
            }
            
            await _exhibitionRepository.GetAllExhibitionNames();
            Console.Write("Введите название выставки: ");
            var exhibitionName = Console.ReadLine();
            
            if (await _exhibitionRepository.GetByName(exhibitionName) == null)
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
            
            var ticket = await _ticketRepository.GetById(id);
            if (ticket == null)
            {
                throw new Exception($"Такого билета не существует");
            }
            
            await _ticketRepository.Delete(id);
            Console.WriteLine("Билет удален");
            
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
            
            if (await _ticketRepository.GetById(id) == null)
            {
                throw new Exception($"Такого билета не существует");
            }
            
            await _visitorRepository.GetAllVisitorsNames();
            Console.Write("Введите полное имя посетителя: ");
            var fullName = Console.ReadLine();
            if (await _visitorRepository.GetIdByName(fullName!) == null)
            {
                throw new Exception($"Пользователя {fullName} не существует");
            }
            
            await _exhibitionRepository.GetAllExhibitionNames();
            Console.Write("Введите название выставки: ");
            var exhibitionName = Console.ReadLine();
            
            if (await _exhibitionRepository.GetIdByName(exhibitionName!) == null)
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
        var tickets = _ticketRepository.GetTicketQuery();

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
                $"ID билета: {ticket.Id} | Выставка: {exhibition!.Name,-20} | Посетитель: {visitor!.FullName,-20} | Цена: {ticket.Price,-10:0.00}(Со скидкой {visitor.Discount}% - {ticket.Price * (100 - visitor.Discount) / 100:0.00})");
        }
        Console.WriteLine("------------------------------------------------------------------------------------------------------------------------------------------------------------------\n");
    }
}
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
            await _namesGetter.GetAllExhibitionNames();
            Console.Write("Введите название выставки: ");
            var exhibitionName = Console.ReadLine();
            Console.Write("Введите стоимость билета: ");
            var price = double.Parse(Console.ReadLine());
            
            var exhibition = await _exhibitionRepository.GetByName(exhibitionName);
            var visitor = await _visitorRepository.GetByName(fullName);
            
            var ticket = Ticket.Create(exhibition.Id, visitor.Id, price);
            
            
            Console.WriteLine($"Создан билет {ticket.Id}");
            
            await _ticketRepository.Add(ticket);
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
    
    public async Task UpdateTicket()
    {
        try
        {
            Console.Write("Введите ID билета: ");
            var id = Guid.Parse(Console.ReadLine());
            await _namesGetter.GetAllVisitorsNames();
            Console.Write("Введите полное имя посетителя: ");
            var fullName = Console.ReadLine();
            await _namesGetter.GetAllExhibitionNames();
            Console.Write("Введите название выставки: ");
            var exhibitionName = Console.ReadLine();
            Console.Write("Введите стоимость билета: ");
            var price = double.Parse(Console.ReadLine());
            
            var exhibition = await _exhibitionRepository.GetByName(exhibitionName);
            var visitor = await _visitorRepository.GetByName(fullName);

            await _ticketRepository.Update(id, exhibition.Id, visitor.Id, price);
            Console.WriteLine("Билет обновлен");
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
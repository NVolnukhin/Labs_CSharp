using DatabaseContext;
using DatabaseContext.Repositories;
using DatabaseModel;
using Microsoft.EntityFrameworkCore;

namespace Core;

public class VisitorFacade
{
    private readonly VisitorRepository _visitorRepository;
    private readonly TicketRepository _ticketRepository;

    public VisitorFacade(AppDbContext context)
    {
        _visitorRepository = new VisitorRepository(context);
        _ticketRepository = new TicketRepository(context);
    }
    
    
    public async Task AddVisitor()
    {
        try
        {
            Console.Write("Введите полное имя: ");
            var name = Console.ReadLine()!;
            if (name.Length < 3)
            {
                throw new Exception("Длина имени не может быть меньше 3 символов");
            }
            
            Console.Write("Введите скидку посетителя в процентах: ");
            var discount = double.Parse(Console.ReadLine()!);
            if (discount is < 0 or > 100)
            {
                throw new Exception("Скидка может быть в диапазоне от 0 до 100%");
            }
            
            var visitor = Visitor.Create(name, discount);
            Console.WriteLine($"Создан посетитель {visitor.Id}");
            
            await _visitorRepository.Add(visitor);
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

    public async Task DeleteVisitor()
    {
        try
        {
            await GetAllVisitors();
            
            Console.Write("Введите ID посетителя: ");
            var id = Guid.Parse(Console.ReadLine()!);
            
            if (await _visitorRepository.GetById(id) == null)
            {
                throw new Exception("Пользователь не найден");
            }
            
            await _ticketRepository.DeleteByVisitorId(id);
            Console.WriteLine("Билеты посетителя удалены");
            await _visitorRepository.Delete(id);
            Console.WriteLine("Посетитель удален");
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
    
    public async Task UpdateVisitor()
    {
        try
        {
            await GetAllVisitors();
            
            Console.Write("Введите ID посетителя: ");
            var id = Guid.Parse(Console.ReadLine()!);
            if (await _visitorRepository.GetById(id) == null)
            {
                throw new Exception("Посетителя с данным ID не существует");
            }
            
            Console.Write("Введите полное имя посетителя: ");
            var name = Console.ReadLine()!;
            if (name.Length < 3)
            {
                throw new Exception("Длина имени не может быть меньше 3 символов");
            }
            
            Console.Write("Введите скидку посетителя в процентах: ");
            var discount = double.Parse(Console.ReadLine()!);
            if (discount is < 0 or > 100)
            {
                throw new Exception("Скидка может быть в диапазоне от 0 до 100%");
            }
            
            await _visitorRepository.Update(id, name, discount);
            Console.WriteLine("Посетитель обновлен");
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
    
    public async Task GetAllVisitors()
    {
        Console.WriteLine("-------------------------------------------------------------------------------------------------------");
        var visitors = _visitorRepository.GetVisitorQuery();
        
        var count = await visitors.CountAsync();
        if (count == 0)
        {
            Console.WriteLine($"Нет посетителей в базе данных");
            return;
        }
        var list = await visitors.ToListAsync();

        foreach (var visitor in list)
            Console.WriteLine($"ID посетителя: {visitor.Id} | Полное имя: {visitor.FullName, -20} | Скидка: {visitor.Discount}");
        Console.WriteLine("-------------------------------------------------------------------------------------------------------\n");
    }
}
using DatabaseContext;
using DatabaseContext.Repositories;
using DatabaseModel;
using Microsoft.EntityFrameworkCore;

namespace Core;

public class VisitorFacade
{
    private readonly AppDbContext _context;
    private readonly VisitorRepository _visitorRepository;

    public VisitorFacade(AppDbContext context)
    {
        _context = context;
        _visitorRepository = new VisitorRepository(context);
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
        Console.WriteLine("-------------------------------------------------------------------------------------------------------");
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
            Console.WriteLine($"ID посетителя: {visitor.Id} | Полное имя: {visitor.FullName, -20} | Скидка: {visitor.Discount}");
        Console.WriteLine("-------------------------------------------------------------------------------------------------------\n");
    }
}
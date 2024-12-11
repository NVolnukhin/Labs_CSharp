using DatabaseModel;
using Microsoft.EntityFrameworkCore;

namespace DatabaseContext.Repositories;

public class VisitorRepository
{
    private readonly AppDbContext _appDbContext;
    
    public VisitorRepository(AppDbContext dbContext)
    {
        _appDbContext = dbContext;
    }
    
    public async Task Add(Visitor visitor)
    {
        await _appDbContext.Visitors.AddAsync(visitor);
        Console.WriteLine($"Добавлен посетитель {visitor.Id}");
        try
        {
            await _appDbContext.SaveChangesAsync();
            Console.WriteLine($"Сохранен посетитель {visitor.Id}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Не удалось сохранить пользователя {visitor.Id}. \nПричина: {ex}");
        }
        await _appDbContext.SaveChangesAsync();
    }
    
    public async Task Update(Guid visitorId, string fullName, double discount)
    {
        await _appDbContext.Visitors
            .Where(v => v.Id == visitorId)
            .ExecuteUpdateAsync(v => v
                .SetProperty(p => p.FullName, fullName)
                .SetProperty(p => p.Discount, discount)
            );
    }

    public async Task Delete(Guid visitorId)
    {
        await _appDbContext.Visitors
            .Where(v => v.Id == visitorId)
            .ExecuteDeleteAsync();
    }

    public async Task<Visitor?> GetByName(string name)
    {
        return await _appDbContext.Visitors
            .FirstOrDefaultAsync(visitor => visitor.FullName == name);
    }
    
    public async Task<Visitor?> GetById(Guid visitorId)
    {
        return await _appDbContext.Visitors
            .FirstOrDefaultAsync(visitor => visitor.Id == visitorId);
    }
    
    public async Task<Guid> GetIdByName(string name)
    {
        var visitor = await _appDbContext.Visitors
            .FirstOrDefaultAsync(visitor => visitor.FullName == name);
        
        return visitor!.Id;
    }
}
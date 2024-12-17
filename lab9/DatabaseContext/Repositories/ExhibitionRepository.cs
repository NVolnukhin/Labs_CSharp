using DatabaseContext.Interfaces;
using DatabaseModel;
using Microsoft.EntityFrameworkCore;

namespace DatabaseContext.Repositories;

public class ExhibitionRepository : IExhibitionRepository
{
    private readonly AppDbContext _appDbContext;
    
    public ExhibitionRepository(AppDbContext dbContext)
    {
        _appDbContext = dbContext;
    }
    
    public async Task Add(Exhibition exhibition)
    {
        await _appDbContext.Exhibitions.AddAsync(exhibition);
        Console.WriteLine($"Добавлена выставка {exhibition.Id}");
        try
        {
            await _appDbContext.SaveChangesAsync();
            Console.WriteLine($"Сохранена выставка {exhibition.Id}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Не удалось сохранить выставку {exhibition.Id}. \nПричина: {ex}");
        }
    }

    public async Task Update(Guid exhibitionId, string name, DateTime date)
    {
        var exhibition = await _appDbContext.Exhibitions
            .FirstAsync(e => e.Id == exhibitionId);
        
        exhibition.Name = name;
        exhibition.Date = date;
        
        await _appDbContext.SaveChangesAsync();
    }

    public async Task Delete(Guid exhibitionId)
    {
        await _appDbContext.Exhibitions
            .Where(e => e.Id == exhibitionId)
            .ExecuteDeleteAsync();
    }
    
    public async Task<Exhibition?> GetById(Guid exhibitionId)
    {
        return await _appDbContext.Exhibitions
            .Where(exhibition => exhibition.Id == exhibitionId)
            .Select(exhibition => exhibition)
            .FirstOrDefaultAsync();
    }
    
    public async Task<Exhibition?> GetByName(string name)
    {
        return await _appDbContext.Exhibitions
            .FirstOrDefaultAsync(exhibition => exhibition.Name == name);
    }
    
    public async Task<Guid> GetIdByName(string name)
    {
        var exhibition = await _appDbContext.Exhibitions
            .FirstOrDefaultAsync(exhibition => exhibition.Name == name);
        
        return exhibition!.Id;
    }
    
    public async Task GetAllExhibitionNames()
    {
        Console.WriteLine("------------ Список всех выставок----------------");
        
        var exhibitionsQuery =
            from exhibition in _appDbContext.Exhibitions
            select exhibition.Name;
        
        var exhibitions = await exhibitionsQuery.Distinct().ToListAsync();
        
        foreach(var exhibition in exhibitions)
            Console.WriteLine(exhibition);
        
        Console.WriteLine("-------------------------------------------------");
    }
    
    public IQueryable<Exhibition> GetExhibitionQuery()
    {
        return 
            from exhibition in _appDbContext.Exhibitions
            select exhibition;
    }

    public IQueryable<Visitor> GetExhibitionVisitorsQuery(string exhibitionName)
    {
        return
            from ticket in _appDbContext.Tickets
            join visitor in _appDbContext.Visitors on ticket.VisitorId equals visitor.Id
            join exhibition in _appDbContext.Exhibitions on ticket.ExhibitionId equals exhibition.Id
            where exhibition.Name == exhibitionName
            select visitor;
    }
}
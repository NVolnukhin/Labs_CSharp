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
        await _appDbContext.Exhibitions
            .Where(e => e.Id == exhibitionId)
            .ExecuteUpdateAsync(e => e
                    .SetProperty(p => p.Name, name)
                    .SetProperty(p => p.Date, date)
            );
    }

    public async Task Delete(Guid exhibitionId)
    {
        await _appDbContext.Exhibitions
            .Where(e => e.Id == exhibitionId)
            .ExecuteDeleteAsync();
    }
    
    public async Task<Exhibition?> GetByName(string name)
    {
        return await _appDbContext.Exhibitions
            .FirstOrDefaultAsync(exhibition => exhibition.Name == name);
    }
    
    public async Task<Exhibition?> GetById(Guid exhibitionId)
    {
        return await _appDbContext.Exhibitions
            .Where(exhibition => exhibition.Id == exhibitionId)
            .Select(exhibition => exhibition)
            .FirstOrDefaultAsync();
    }
}
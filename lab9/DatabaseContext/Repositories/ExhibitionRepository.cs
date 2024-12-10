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
        Console.WriteLine($"Added exhibition {exhibition.Id}");
        try
        {
            await _appDbContext.SaveChangesAsync();
            Console.WriteLine($"Saved exhibition {exhibition.Id}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to add exhibition {exhibition.Id}. \nReason: {ex}");
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
    
    public async Task<List<Guid>> GetAll()
    {
        var list = await _appDbContext.Exhibitions
            .Select(e => e.Id)
            .ToListAsync();
        return list;
    }
}
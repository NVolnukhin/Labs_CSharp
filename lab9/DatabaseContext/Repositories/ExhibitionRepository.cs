using DatabaseContext.Interfaces;
using DatabaseModel;

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
        await _appDbContext.SaveChangesAsync();
    }
}
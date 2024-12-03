using DatabaseModel;

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
        await _appDbContext.SaveChangesAsync();
    }
}
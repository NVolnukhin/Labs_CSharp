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
}
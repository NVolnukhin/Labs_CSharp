using DatabaseContext;
using Microsoft.EntityFrameworkCore;

namespace Core;

public class NamesGetter
{
    private readonly AppDbContext _context;

    public NamesGetter(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<string>> GetExhibitionNamesList()
    {
        var exhibitionNameQuery =
            from exhibition in _context.Exhibitions
            select exhibition.Name;
        
        var exhibitionNames = await exhibitionNameQuery.ToListAsync();
        
        return exhibitionNames;
    }
    
    public async Task<List<string>> GetVisitorsNamesList()
    {
        var visitorsNameQuery =
            from visitor in _context.Visitors
            select visitor.FullName;
        
        var visitorsNames = await visitorsNameQuery.ToListAsync();
        
        return visitorsNames;
    }
    
    public async Task<List<Guid>> GetVisitorGuidsList()
    {
        var visitorsGuidQuery =
            from visitor in _context.Visitors
            select visitor.Id;
        
        var visitorsGuids = await visitorsGuidQuery.ToListAsync();
        
        return visitorsGuids;
    }
    
    public async Task<List<Guid>> GetExhibitionGuidsList()
    {
        var exhibitionGuidQuery =
            from exhibition in _context.Exhibitions
            select exhibition.Id;
        
        var exhibitionGuids = await exhibitionGuidQuery.ToListAsync();
        
        return exhibitionGuids;
    }
}
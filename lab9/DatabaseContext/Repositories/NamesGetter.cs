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
    
    
    public async Task GetAllExhibitionNames()
    {
        Console.WriteLine("------------ Список всех выставок----------------");
        
        var exhibitionsQuery =
            from exhibition in _context.Exhibitions
            select exhibition.Name;
        
        var exhibitions = await exhibitionsQuery.Distinct().ToListAsync();
        
        foreach(var exhibition in exhibitions)
            Console.WriteLine(exhibition);
        
        Console.WriteLine("-------------------------------------------------");
    }
    
    public async Task GetAllVisitorsNames()
    {
        Console.WriteLine("----------- Список всех посетителей---------------");
        
        var visitorsQuery =
            from visitor in _context.Visitors
            select visitor.FullName;
        
        var visitors = await visitorsQuery.Distinct().ToListAsync();
        
        foreach(var visitor in visitors)
            Console.WriteLine(visitor);
        
        Console.WriteLine("-------------------------------------------------");
    }
}
using DatabaseContext.Interfaces;
using DatabaseModel;

namespace Core.Services;

public class VisitorService
{
    private readonly IVisitorRepository _visitorRepository;

    public VisitorService(IVisitorRepository visitorRepository)
    {
        _visitorRepository = visitorRepository;
    }
    
    public async Task Add(string fullName, double discount)
    {
        var visitor = Visitor.Create(fullName, discount);

        await _visitorRepository.Add(visitor);
    }
}
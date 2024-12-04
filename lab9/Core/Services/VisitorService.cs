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
    
    public async Task Update(Guid visitorId, string fullName, double discount)
    {
        await _visitorRepository.Update(visitorId, fullName, discount);
    }

    public async Task Delete(Guid visitorId)
    {
        await _visitorRepository.Delete(visitorId);
    }
}
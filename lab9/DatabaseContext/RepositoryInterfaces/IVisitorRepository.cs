using DatabaseModel;

namespace DatabaseContext.Interfaces;

public interface IVisitorRepository
{
    public Task Add(Visitor visitor);
    public Task Update(Guid visitorId, string fullName, double discount);
    public Task Delete(Guid visitorId);
}
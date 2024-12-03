using DatabaseModel;

namespace DatabaseContext.Interfaces;

public interface IVisitorRepository
{
    public Task Add(Visitor visitor);
}
using DatabaseContext.Repositories;
using DatabaseModel;

namespace DatabaseContext.Interfaces;

public interface IExhibitionRepository
{
    public Task Add(Exhibition exhibition);
    public Task Update(Guid exhibitionId, string name, DateTime date);
    public Task Delete(Guid exhibitionId);
}
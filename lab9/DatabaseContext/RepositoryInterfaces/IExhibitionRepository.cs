using DatabaseContext.Repositories;
using DatabaseModel;

namespace DatabaseContext.Interfaces;

public interface IExhibitionRepository
{
    public Task Add(Exhibition exhibition);
}
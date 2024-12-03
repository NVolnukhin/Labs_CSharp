using DatabaseModel;

namespace DatabaseContext.Interfaces;

public interface ITicketRepository
{
    public Task Add(Ticket ticket);
}
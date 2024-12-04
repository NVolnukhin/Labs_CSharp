using DatabaseModel;

namespace DatabaseContext.Interfaces;

public interface ITicketRepository
{
    public Task Add(Ticket ticket);
    public Task Update(Guid ticketId, Exhibition exhibition, Guid visitorId, Visitor visitor);
    public Task Delete(Guid ticketId);
}
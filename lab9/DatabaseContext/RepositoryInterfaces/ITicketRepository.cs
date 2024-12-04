using DatabaseModel;

namespace DatabaseContext.Interfaces;

public interface ITicketRepository
{
    public Task Add(Ticket ticket);
    public Task Update(Guid ticketId, Guid exhibitionId, Exhibition exhibition, Guid visitorId, Visitor visitor);
    public Task Delete(Guid ticketId);
}
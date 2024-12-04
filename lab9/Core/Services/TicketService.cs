using DatabaseContext.Interfaces;
using DatabaseModel;

namespace Core.Services;

public class TicketService
{
    private readonly ITicketRepository _ticketRepository;

    public TicketService(ITicketRepository ticketRepository)
    {
        _ticketRepository = ticketRepository;
    }
    
    public async Task Add(Guid exhibitionId, Exhibition exhibition, Guid visitorId, Visitor visitor, double price)
    {
        var ticket = Ticket.Create(exhibitionId, exhibition, visitorId, visitor, price);

        await _ticketRepository.Add(ticket);
    }
    
    public async Task Update(Guid ticketId, Guid exhibitionId, Exhibition exhibition, Guid visitorId, Visitor visitor, double price)
    {
        await _ticketRepository.Update(ticketId, exhibitionId, exhibition, visitorId, visitor, price);
    }

    public async Task Delete(Guid ticketId)
    {
        await _ticketRepository.Delete(ticketId);
    }
}
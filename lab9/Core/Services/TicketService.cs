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
    
    public async Task Add(Guid exhibitionId, Exhibition exhibition, Guid visitorId, Visitor visitor)
    {
        var ticket = Ticket.Create(exhibitionId, exhibition, visitorId, visitor);

        await _ticketRepository.Add(ticket);
    }
}
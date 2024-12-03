using DatabaseModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DatabaseContext.Configurations;

public class TicketConfiguration : IEntityTypeConfiguration<Ticket>
{
    public void Configure(EntityTypeBuilder<Ticket> builder)
    {
        builder
            .HasOne(t => t.Exhibition)
            .WithMany(e => e.Tickets)
            .HasForeignKey(t => t.ExhibitionId);

        builder
            .HasOne(t => t.Visitor)
            .WithMany(v => v.Tickets)
            .HasForeignKey(t => t.VisitorId);
    }
}
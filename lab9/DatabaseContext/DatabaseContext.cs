using DatabaseModel;
using Microsoft.EntityFrameworkCore;


namespace DatabaseContext;

public class DatabaseContext : DbContext
{
    public DbSet<Exhibition> Exhibitions { get; set; }
    public DbSet<Visitor> Visitors { get; set; }
    public DbSet<Ticket> Tickets { get; set; }

    public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Настройка связи Ticket -> Exhibition и Visitor
        modelBuilder.Entity<Ticket>()
            .HasOne(t => t.Exhibition)
            .WithMany(e => e.Tickets)
            .HasForeignKey(t => t.ExhibitionId);

        modelBuilder.Entity<Ticket>()
            .HasOne(t => t.Visitor)
            .WithMany(v => v.Tickets)
            .HasForeignKey(t => t.VisitorId);
    }
}
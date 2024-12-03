using DatabaseModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;


namespace DatabaseContext;

public class AppDbContext(DbContextOptions<AppDbContext> options) 
    : DbContext(options)
{
    public AppDbContext() : this(new DbContextOptionsBuilder<AppDbContext>().UseNpgsql("Host=localhost;Port=5432;Database=lab9db;Username=postgres;Password=123").Options) {}
    public DbSet<Exhibition> Exhibitions { get; set; }
    public DbSet<Visitor> Visitors { get; set; }
    public DbSet<Ticket> Tickets { get; set; }

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
        
        base.OnModelCreating(modelBuilder);
    }
    
    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseNpgsql("Host=localhost;Port=5432;Database=lab9db;Username=postgres;Password=123");
}
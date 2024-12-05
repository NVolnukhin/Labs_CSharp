using DatabaseContext.Configurations;
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
    public AppDbContext GetAppDbContext() => this;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Настройка связи Ticket -> Exhibition и Visitor
        modelBuilder.ApplyConfiguration(new TicketConfiguration());
        
        base.OnModelCreating(modelBuilder);
    }
}
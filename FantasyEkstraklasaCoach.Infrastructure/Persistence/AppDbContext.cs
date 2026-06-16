using FantasyEkstraklasaCoach.Domain.Example;
using Microsoft.EntityFrameworkCore;

namespace FantasyEkstraklasaCoach.Infrastructure.Persistence;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<ExampleAggregate> Examples => Set<ExampleAggregate>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }
}

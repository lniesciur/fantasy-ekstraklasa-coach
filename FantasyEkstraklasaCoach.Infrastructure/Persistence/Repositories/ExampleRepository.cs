using FantasyEkstraklasaCoach.Domain.Example;

namespace FantasyEkstraklasaCoach.Infrastructure.Persistence.Repositories;

public class ExampleRepository(AppDbContext context) : IExampleRepository
{
    public async Task AddAsync(ExampleAggregate example, CancellationToken ct = default) =>
        await context.Examples.AddAsync(example, ct);
}

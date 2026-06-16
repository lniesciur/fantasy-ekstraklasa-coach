using FantasyEkstraklasaCoach.Domain.Primitives;

namespace FantasyEkstraklasaCoach.Application.Abstractions;

public interface IDomainEventDispatcher
{
    Task DispatchAsync(IDomainEvent domainEvent, CancellationToken ct = default);
}

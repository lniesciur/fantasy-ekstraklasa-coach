using FantasyEkstraklasaCoach.Application.Abstractions;
using FantasyEkstraklasaCoach.Domain.Primitives;

namespace FantasyEkstraklasaCoach.Infrastructure.DomainEvents;

public class NullDomainEventDispatcher : IDomainEventDispatcher
{
    public Task DispatchAsync(IDomainEvent domainEvent, CancellationToken ct = default) =>
        Task.CompletedTask;
}

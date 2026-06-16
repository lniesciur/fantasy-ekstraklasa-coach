namespace FantasyEkstraklasaCoach.Domain.Primitives;

public interface IHasDomainEvents
{
    IReadOnlyList<IDomainEvent> PopDomainEvents();
}

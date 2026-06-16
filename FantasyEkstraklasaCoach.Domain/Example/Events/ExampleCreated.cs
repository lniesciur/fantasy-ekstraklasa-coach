using FantasyEkstraklasaCoach.Domain.Primitives;

namespace FantasyEkstraklasaCoach.Domain.Example.Events;

public record ExampleCreated(ExampleId Id, string Name) : IDomainEvent;

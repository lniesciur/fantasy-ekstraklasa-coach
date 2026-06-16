using FantasyEkstraklasaCoach.Domain.Example.Events;
using FantasyEkstraklasaCoach.Domain.Primitives;

namespace FantasyEkstraklasaCoach.Domain.Example;

public class ExampleAggregate : AggregateRoot<ExampleId>
{
    private ExampleAggregate(ExampleId id, string name) : base(id)
    {
        Name = name;
    }

    public string Name { get; private set; }

    public static ExampleAggregate Create(string name)
    {
        var example = new ExampleAggregate(ExampleId.New(), name);
        example.RaiseDomainEvent(new ExampleCreated(example.Id, example.Name));
        return example;
    }
}

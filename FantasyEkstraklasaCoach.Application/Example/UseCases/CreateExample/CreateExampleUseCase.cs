using FantasyEkstraklasaCoach.Application.Abstractions;
using FantasyEkstraklasaCoach.Contracts.Example;
using FantasyEkstraklasaCoach.Domain.Example;
using FantasyEkstraklasaCoach.Domain.Primitives;

namespace FantasyEkstraklasaCoach.Application.Example.UseCases.CreateExample;

public class CreateExampleUseCase(IExampleRepository repository, IUnitOfWork unitOfWork)
{
    public async Task<Result<CreateExampleResponse>> ExecuteAsync(string name, CancellationToken ct)
    {
        var example = ExampleAggregate.Create(name);

        await repository.AddAsync(example, ct);
        await unitOfWork.SaveChangesAsync(ct);

        return Result<CreateExampleResponse>.Ok(new CreateExampleResponse(example.Id.Value, example.Name));
    }
}

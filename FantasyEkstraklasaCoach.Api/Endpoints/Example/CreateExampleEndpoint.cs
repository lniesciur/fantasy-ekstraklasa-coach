using FantasyEkstraklasaCoach.Application.Example.UseCases.CreateExample;
using FantasyEkstraklasaCoach.Contracts.Example;
using FastEndpoints;

namespace FantasyEkstraklasaCoach.Api.Endpoints.Example;

public class CreateExampleEndpoint(CreateExampleUseCase useCase) : Endpoint<CreateExampleRequest, CreateExampleResponse>
{
    public override void Configure()
    {
        Post("/examples");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CreateExampleRequest req, CancellationToken ct)
    {
        var result = await useCase.ExecuteAsync(req.Name, ct);

        if (result.IsFailure)
        {
            AddError(result.Error!);
            await Send.ErrorsAsync(409, ct);
            return;
        }

        await Send.CreatedAtAsync<CreateExampleEndpoint>(null, result.Value!, cancellation: ct);
    }
}

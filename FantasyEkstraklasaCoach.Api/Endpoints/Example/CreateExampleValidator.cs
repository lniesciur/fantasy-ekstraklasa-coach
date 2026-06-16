using FantasyEkstraklasaCoach.Contracts.Example;
using FastEndpoints;
using FluentValidation;

namespace FantasyEkstraklasaCoach.Api.Endpoints.Example;

public class CreateExampleValidator : Validator<CreateExampleRequest>
{
    public CreateExampleValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(100);
    }
}

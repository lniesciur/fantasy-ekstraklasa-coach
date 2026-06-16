using FantasyEkstraklasaCoach.Application;
using FantasyEkstraklasaCoach.Infrastructure;
using FastEndpoints;
using FastEndpoints.Swagger;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddFastEndpoints()
    .SwaggerDocument(o =>
    {
        o.DocumentSettings = s =>
        {
            s.Title = "FantasyEkstraklasaCoach API";
            s.Version = "v1";
        };
    });

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

app.UseStaticFiles();

app.UseFastEndpoints(c =>
{
    c.Endpoints.RoutePrefix = "api";
    c.Errors.UseProblemDetails();
});
app.UseSwaggerGen(uiConfig: ui =>
{
    ui.DocExpansion = "list";
    ui.DefaultModelsExpandDepth = 1;
});

app.Run();

public partial class Program;


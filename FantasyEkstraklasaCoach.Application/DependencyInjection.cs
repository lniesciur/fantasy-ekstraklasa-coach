using Microsoft.Extensions.DependencyInjection;

namespace FantasyEkstraklasaCoach.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.Scan(scan => scan
            .FromAssemblies(typeof(DependencyInjection).Assembly)
            .AddClasses(classes => classes.Where(t => t.Name.EndsWith("UseCase")))
            .AsSelf()
            .WithScopedLifetime());

        return services;
    }
}

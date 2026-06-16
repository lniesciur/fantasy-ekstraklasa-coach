using FantasyEkstraklasaCoach.Application.Abstractions;
using FantasyEkstraklasaCoach.Infrastructure.DomainEvents;
using FantasyEkstraklasaCoach.Infrastructure.Persistence;
using FantasyEkstraklasaCoach.Infrastructure.Storage;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FantasyEkstraklasaCoach.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(o =>
            o.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

        services.Configure<SupabaseOptions>(configuration.GetSection("Supabase"));

        services.AddSingleton(sp =>
        {
            var opts = configuration.GetSection("Supabase").Get<SupabaseOptions>()!;
            return new Supabase.Client(opts.Url, opts.Key, new Supabase.SupabaseOptions
            {
                AutoConnectRealtime = false
            });
        });

        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IDomainEventDispatcher, NullDomainEventDispatcher>();

        services.Scan(scan => scan
            .FromAssemblies(typeof(DependencyInjection).Assembly)
            .AddClasses(classes => classes.Where(t => t.Name.EndsWith("Repository")))
            .AsImplementedInterfaces()
            .WithScopedLifetime()
            .AddClasses(classes => classes.Where(t => t.Name.EndsWith("Service")))
            .AsImplementedInterfaces()
            .WithScopedLifetime());

        return services;
    }
}

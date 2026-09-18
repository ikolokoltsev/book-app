using BookApp.Infrastructure;

using Microsoft.EntityFrameworkCore;

namespace BookApp.Api.Extensions;

public static class PersistenceExtensions
{
    public static IServiceCollection AddPersistence(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        var connectionString =
            configuration.GetConnectionString("Default")
            ?? throw new InvalidOperationException("Connection string is not configured");

        services.AddDbContext<AppDbContext>(options =>
            options
                .UseNpgsql(connectionString)
                .UseSeeding((context, _) => SeedData.Seed(context))
                .UseAsyncSeeding((context, _, ct) => SeedData.SeedAsync(context, ct))
        );

        return services;
    }
}
namespace BookApp.Api.Extensions;

public static class CorsExtensions
{
    public const string PolicyName = "WebApp";

    public static IServiceCollection AddCorsPolicy(
        this IServiceCollection services,
         IConfiguration configuration
    )
    {
        var allowedOrigin = configuration["Cors:AllowedOrigin"]
            ?? throw new InvalidOperationException("Cors is not configured.");

        services.AddCors(options => options.AddPolicy(PolicyName, policy => policy.WithOrigins(allowedOrigin).AllowAnyHeader().AllowAnyMethod()));

        return services;
    }
}
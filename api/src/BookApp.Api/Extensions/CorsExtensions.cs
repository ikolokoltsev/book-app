namespace BookApp.Api.Extensions;

public static class CorsExtensions
{
    public const string PolicyName = "WebApp";

    public static IServiceCollection AddCorsPolicy(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        var allowedOrigins = (configuration["Cors:AllowedOrigin"]
            ?? throw new InvalidOperationException("Cors is not configured."))
            .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

        services.AddCors(options =>
            options.AddPolicy(PolicyName,
                policy => policy.WithOrigins(allowedOrigins).AllowAnyHeader().AllowAnyMethod()));

        return services;
    }
}
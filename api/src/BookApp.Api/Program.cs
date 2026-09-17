using BookApp.Api.Common;
using BookApp.Api.Extensions;
using BookApp.Api.Features.Auth;
using BookApp.Api.Features.Books;
using BookApp.Api.Features.Quotes;

using Microsoft.AspNetCore.HttpOverrides;

using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddJwtAuth(builder.Configuration);
builder.Services.AddHealthChecks();
builder.Services.AddScoped<TokenService>();
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<BookService>();
builder.Services.AddScoped<QuoteService>();
builder.Services.AddCorsPolicy(builder.Configuration);
builder.Services.AddControllers();
builder.Services.AddPersistence(builder.Configuration);
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
    options.KnownNetworks.Clear();
    options.KnownProxies.Clear();
});
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ICurrentUser, CurrentUser>();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseForwardedHeaders();
app.UseCors(CorsExtensions.PolicyName);
app.UseAuthentication();
app.UseAuthorization();

app.MapOpenApi();
app.MapScalarApiReference(options =>
    options.WithOpenApiRoutePattern("/openapi/{documentName}.json"));
app.MapHealthChecks("/health");
app.MapControllers();

app.Run();
public partial class Program;
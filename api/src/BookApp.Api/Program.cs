using BookApp.Api.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddHealthChecks();
builder.Services.AddCorsPolicy(builder.Configuration);
builder.Services.AddControllers();
builder.Services.AddPersistence(builder.Configuration);
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseHttpsRedirection();
}
app.UseCors(CorsExtensions.PolicyName);
app.UseAuthorization();

app.MapHealthChecks("/health");
app.MapControllers();

app.Run();

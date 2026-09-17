using System.ComponentModel.DataAnnotations;

namespace BookApp.Api.Features;

public record QuoteResponse(Guid Id, string Text, string? Author, DateTime CreatedAt);

public record QuoteRequest(
    [Required, MaxLength(1000)] string Text,
    [MaxLength(200)] string? Author
);

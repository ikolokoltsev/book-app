using System.ComponentModel.DataAnnotations;

namespace BookApp.Api.Features.Books;

public record BookResponse(
    Guid Id,
    string Title,
    string Author,
    DateOnly PublishedOn,
    DateTime CreatedAt
);

public record BookRequest(
    [Required, MaxLength(200)] string Title,
    [Required, MaxLength(200)] string Author,
    DateOnly PublishedOn
);
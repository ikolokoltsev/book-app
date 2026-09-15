namespace BookApp.Domain;

public class Quote
{
    public Guid Id { get; set; }
    public required string Text { get; set; }
    public string? Author { get; set; }
    public Guid UserId { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
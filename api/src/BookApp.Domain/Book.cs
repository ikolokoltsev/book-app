namespace BookApp.Domain;

public class Book
{
    public Guid Id { get; set; }
    public required string Title { get; set; }
    public required string Author { get; set; }
    public DateOnly PublishedOn { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

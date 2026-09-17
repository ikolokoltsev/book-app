using BookApp.Domain;
using BookApp.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace BookApp.Api.Features.Books;

public class BookService(AppDbContext db)
{
    public async Task<IReadOnlyList<BookResponse>> GetAllBooksAsync(CancellationToken ct) =>
        await db
            .Books.AsNoTracking()
            .OrderByDescending(b => b.CreatedAt)
            .Select(b => new BookResponse(b.Id, b.Title, b.Author, b.PublishedOn, b.CreatedAt))
            .ToListAsync(ct);

    public async Task<BookResponse?> GetBookByIdAsync(Guid id, CancellationToken ct) =>
        await db
            .Books.AsNoTracking()
            .Where(b => b.Id == id)
            .Select(b => new BookResponse(b.Id, b.Title, b.Author, b.PublishedOn, b.CreatedAt))
            .FirstOrDefaultAsync(ct);

    public async Task<BookResponse> CreateBookAsync(BookRequest request, CancellationToken ct)
    {
        var book = new Book
        {
            Title = request.Title,
            Author = request.Author,
            PublishedOn = request.PublishedOn,
        };

        db.Books.Add(book);
        await db.SaveChangesAsync(ct);

        return new BookResponse(book.Id, book.Title, book.Author, book.PublishedOn, book.CreatedAt);
    }

    public async Task<BookResponse?> UpdateBookAsync(
        Guid id,
        BookRequest request,
        CancellationToken ct
    )
    {
        var book = await db.Books.FirstOrDefaultAsync(b => b.Id == id, ct);
        if (book is null)
            return null;

        book.Title = request.Title;
        book.Author = request.Author;
        book.PublishedOn = request.PublishedOn;

        await db.SaveChangesAsync(ct);

        return new BookResponse(book.Id, book.Title, book.Author, book.PublishedOn, book.CreatedAt);
    }

    public async Task<bool> DeleteBookAsync(Guid id, CancellationToken ct)
    {
        var rows = await db.Books.Where(b => b.Id == id).ExecuteDeleteAsync(ct);
        return rows > 0;
    }
}

using BookApp.Api.Common;
using BookApp.Domain;
using BookApp.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace BookApp.Api.Features.Quotes;

public class QuoteService(AppDbContext db, ICurrentUser currentUser)
{
    public async Task<IReadOnlyList<QuoteResponse>> GetAllQuotesAsync(CancellationToken ct) =>
        await db
            .Quotes.AsNoTracking()
            .Where(q => q.UserId == currentUser.UserId)
            .OrderByDescending(q => q.CreatedAt)
            .Select(q => new QuoteResponse(q.Id, q.Text, q.Author, q.CreatedAt))
            .ToListAsync(ct);

    public async Task<QuoteResponse?> GetQuoteByIdAsync(Guid id, CancellationToken ct) =>
        await db
            .Quotes.AsNoTracking()
            .Where(q => q.Id == id && q.UserId == currentUser.UserId)
            .Select(q => new QuoteResponse(q.Id, q.Text, q.Author, q.CreatedAt))
            .FirstOrDefaultAsync(ct);

    public async Task<QuoteResponse> CreateQuoteAsync(QuoteRequest request, CancellationToken ct)
    {
        var quote = new Quote
        {
            Text = request.Text,
            Author = request.Author,
            UserId = currentUser.UserId,
        };

        db.Quotes.Add(quote);
        await db.SaveChangesAsync(ct);

        return new QuoteResponse(quote.Id, quote.Text, quote.Author, quote.CreatedAt);
    }

    public async Task<QuoteResponse?> UpdateQuoteAsync(
        Guid id,
        QuoteRequest request,
        CancellationToken ct
    )
    {
        var quote = await db.Quotes.FirstOrDefaultAsync(
            q => q.Id == id && q.UserId == currentUser.UserId,
            ct
        );

        if (quote is null)
            return null;

        quote.Text = request.Text;
        quote.Author = request.Author;

        await db.SaveChangesAsync(ct);

        return new QuoteResponse(quote.Id, quote.Text, quote.Author, quote.CreatedAt);
    }

    public async Task<bool> DeleteQuoteAsync(Guid id, CancellationToken ct)
    {
        var rows = await db
            .Quotes.Where(q => q.Id == id && q.UserId == currentUser.UserId)
            .ExecuteDeleteAsync(ct);

        return rows > 0;
    }
}

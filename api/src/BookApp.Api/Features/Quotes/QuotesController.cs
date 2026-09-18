using BookApp.Api.Features.Quotes;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookApp.Api.Features.Quotes;

[ApiController]
[Authorize]
[Route("api/quotes")]
public class QuotesController(QuoteService quoteService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<QuoteResponse>>> GetAllQuotes(
        CancellationToken ct
    ) => Ok(await quoteService.GetAllQuotesAsync(ct));

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<QuoteResponse>> GetQuoteById(Guid id, CancellationToken ct)
    {
        var quote = await quoteService.GetQuoteByIdAsync(id, ct);
        return quote is null ? NotFound() : Ok(quote);
    }

    [HttpPost]
    public async Task<ActionResult<QuoteResponse>> CreateQuote(
        QuoteRequest request,
        CancellationToken ct
    )
    {
        var created = await quoteService.CreateQuoteAsync(request, ct);
        return CreatedAtAction(nameof(GetQuoteById), new { id = created.Id }, created);
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<QuoteResponse>> UpdateQuote(
        Guid id,
        QuoteRequest request,
        CancellationToken ct
    )
    {
        var updated = await quoteService.UpdateQuoteAsync(id, request, ct);
        return updated is null ? NotFound() : Ok(updated);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteBook(Guid id, CancellationToken ct) =>
        await quoteService.DeleteQuoteAsync(id, ct) ? NoContent() : NotFound();
}
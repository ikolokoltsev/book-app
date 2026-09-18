using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookApp.Api.Features.Books;

[ApiController]
[Authorize]
[Route("api/books")]
public class BooksController(BookService bookService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<BookResponse>>> GetAllBooks(
        CancellationToken ct
    ) => Ok(await bookService.GetAllBooksAsync(ct));

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<BookResponse>> GetBookById(Guid id, CancellationToken ct)
    {
        var book = await bookService.GetBookByIdAsync(id, ct);
        return book is null ? NotFound() : Ok(book);
    }

    [HttpPost]
    public async Task<ActionResult<BookResponse>> CreateBook(
        BookRequest request,
        CancellationToken ct
    )
    {
        var created = await bookService.CreateBookAsync(request, ct);
        return CreatedAtAction(nameof(GetBookById), new { id = created.Id }, created);
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<BookResponse>> UpdateBook(
        Guid id,
        BookRequest request,
        CancellationToken ct
    )
    {
        var updated = await bookService.UpdateBookAsync(id, request, ct);
        return updated is null ? NotFound() : Ok(updated);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteBook(Guid id, CancellationToken ct) =>
        await bookService.DeleteBookAsync(id, ct) ? NoContent() : NotFound();
}
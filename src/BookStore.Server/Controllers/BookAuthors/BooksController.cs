using Asp.Versioning;
using BookStore.Application.BookAuthors;
using BookStore.Application.BookAuthors.Books.Commands.CreateBook;
using BookStore.Application.BookAuthors.Books.Commands.DeleteBook;
using BookStore.Application.BookAuthors.Books.Commands.UpdateBook;
using BookStore.Application.BookAuthors.Books.Queries.GetBookAuthors;
using BookStore.Application.BookAuthors.Books.Queries.GetBookById;
using BookStore.Contracts.Books;
using BookStore.Domain.BookAuthors.ValueObjects;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.Server.Controllers.BookAuthors;

[ApiController]
[Authorize]
[ApiVersion(ApiVersions.V1)]
[Route("api/v{version:apiVersion}/books")]
public class BooksController(ISender sender) : ControllerBase
{
    #region GET

    /// <summary>Paged list with search / filter / sort.</summary>
    [HttpGet]
    public async Task<IActionResult> GetBooks(
        [FromQuery] BookQueryParameters parameters,
        CancellationToken ct)
    {
        var result = await sender.Send(new GetBookAuthorsQuery(parameters), ct);
        return result.IsSuccess
            ? Ok(result.Value)              // 200
            : BadRequest(result.Error);     // 400
    }

    /// <summary>Get a single book by ID.</summary>
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetBookById(Guid id, CancellationToken ct)
    {
        var result = await sender.Send(new GetBookByIdQuery(BookId.Create(id)), ct);
        return result.IsSuccess
            ? Ok(result.Value)              // 200
            : NotFound(result.Error);       // 404
    }

    #endregion
    #region POST

    /// <summary>Create a new book.</summary>
    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreateBookRequest request,
        CancellationToken ct)
    {
        // Mapster converts DTO → command
        var command = request.Adapt<CreateBookCommand>();
        var result = await sender.Send(command, ct);

        if (result.IsFailure)
            return BadRequest(result.Error);           // 400

        // 201 + Location: /api/v1/books/{id}
        return CreatedAtAction(nameof(GetBookById),
            new { id = result.Value, version = HttpContext.GetRequestedApiVersion()!.ToString() },
            result.Value);
    }

    #endregion
    #region PUT

    /// <summary>Full update of an existing book.</summary>
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(
        Guid id,
        [FromBody] UpdateBookRequest request,
        CancellationToken ct)
    {
        if (id != request.BookId)
            return BadRequest("Route id does not match body BookId.");

        var command = request.Adapt<UpdateBookCommand>();
        var result = await sender.Send(command, ct);

        return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
    }

    #endregion
    #region DELETE

    /// <summary>Delete (hard-remove) a book.</summary>
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        var result = await sender.Send(new DeleteBookCommand(id), ct);
        return result.IsSuccess ? NoContent() : BadRequest(result.Error);
    }

    #endregion
}
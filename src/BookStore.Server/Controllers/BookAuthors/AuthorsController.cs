using Asp.Versioning;
using BookStore.Application.BookAuthors;
using BookStore.Application.BookAuthors.Authors.Commands.CreateAuthor;
using BookStore.Application.BookAuthors.Authors.Commands.DeleteAuthor;
using BookStore.Application.BookAuthors.Authors.Commands.UpdateAuthor;
using BookStore.Application.BookAuthors.Authors.Queries.GetAuthorById;
using BookStore.Application.BookAuthors.Authors.Queries.GetAuthors;
using BookStore.Contracts.Authors;
using BookStore.Domain.BookAuthors.ValueObjects;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.Server.Controllers.BookAuthors;

[ApiController]
[Authorize]
[ApiVersion(ApiVersions.V1)]
[Route("api/v{version:apiVersion}/authors")]
public class AuthorsController(ISender sender) : ControllerBase
{
    #region GET

    [HttpGet]
    public async Task<IActionResult> GetAuthors(
        [FromQuery] AuthorQueryParameters parameters,
        CancellationToken ct)
    {
        var result = await sender.Send(new GetAuthorsQuery(parameters), ct);
        return result.IsSuccess
            ? Ok(result.Value)           
            : BadRequest(result.Error);   
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetAuthorById(
        Guid id,
        CancellationToken ct)
    {
        var result = await sender.Send(new GetAuthorByIdQuery(id), ct);
        return result.IsSuccess
            ? Ok(result.Value)           
            : NotFound(result.Error);     
    }

    #endregion
    #region POST

    /// <summary>Create a new author.</summary>
    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreateAuthorRequest request,
        CancellationToken ct)
    {
        var command = request.Adapt<CreateAuthorCommand>();
        var result = await sender.Send(command, ct);

        if (result.IsFailure)
            return BadRequest(result.Error); 

        return CreatedAtAction(
            nameof(GetAuthorById),
            new { id = result.Value, version = HttpContext.GetRequestedApiVersion()!.ToString() },
            result.Value);               
    }

    #endregion
    #region PUT

    /// <summary>Update an existing author.</summary>
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(
        Guid id,
        [FromBody] UpdateAuthorRequest request,
        CancellationToken ct)
    {
        if (id != request.AuthorId)
            return BadRequest("Route ID does not match body AuthorId.");

        var command = request.Adapt<UpdateAuthorCommand>();
        var result = await sender.Send(command, ct);

        return result.IsSuccess
            ? Ok(result.Value)             // 200 OK with updated AuthorId (Guid)
            : BadRequest(result.Error);    // 400 Bad Request
    }

    #endregion
    #region DELETE

    /// <summary>Delete an author permanently.</summary>
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(
        Guid id,
        CancellationToken ct)
    {
        var result = await sender.Send(new DeleteAuthorCommand(id), ct);
        return result.IsSuccess
            ? NoContent()                  // 204 No Content
            : BadRequest(result.Error);    // 400 Bad Request
    }

    #endregion
}
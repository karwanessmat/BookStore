// /Server/Controllers/ShoppingCarts/CartController.cs

using Asp.Versioning;
using BookStore.Application.ShoppingCards.Commands.AddCartItem;
using BookStore.Application.ShoppingCards.Commands.RemoveCartItem;
using BookStore.Application.ShoppingCards.Commands.UpdateCartItem;
using BookStore.Application.ShoppingCards.Queries.GetCart;
using BookStore.Contracts.ShoppingCards;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.Server.Controllers.Shopping;

[ApiController]
[Authorize]
[ApiVersion(ApiVersions.V1)]
[Route("api/v{version:apiVersion}/cart")]
public sealed class CartController(ISender sender) : ControllerBase
{
    // GET /cart
    [HttpGet]
    public async Task<IActionResult> Get(CancellationToken ct)
    {
        var result = await sender.Send(new GetCartQuery(), ct);
        return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
    }

    // POST /cart/items
    [HttpPost("items")]
    public async Task<IActionResult> Add(AddCartItemRequest req, CancellationToken ct)
    {
        var cmd = new AddCartItemCommand(req);
        var res = await sender.Send(cmd, ct);
        return res.IsSuccess ? Ok(res.Value) : BadRequest(res.Error);
    }

    // PUT /cart/items/{id}
    [HttpPut("items/{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] int quantity, CancellationToken ct)
    {
        var req = new UpdateCartItemRequest(id, quantity);
        var cmd = new UpdateCartItemCommand(req);
        var res = await sender.Send(cmd, ct);
        return res.IsSuccess ? Ok() : BadRequest(res.Error);
    }

    // DELETE /cart/items/{id}
    [HttpDelete("items/{id:guid}")]
    public async Task<IActionResult> Remove(Guid id, CancellationToken ct)
    {
        var req = new RemoveCartItemRequest(id);
        var cmd = new RemoveCartItemCommand(req);
        var res = await sender.Send(cmd, ct);
        return res.IsSuccess ? NoContent() : BadRequest(res.Error);
    }
}

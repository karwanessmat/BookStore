using Asp.Versioning;
using BookStore.Application.Orders.Commands.PlaceOrder;
using BookStore.Application.Orders.Queries.GetOrderById;
using BookStore.Application.Orders.Queries.ListOrders;
using BookStore.Contracts.Orders;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.Server.Controllers.Shopping;

[ApiController]
[Authorize]
[ApiVersion(ApiVersions.V1)]
[Route("api/v{version:apiVersion}/orders")]
public sealed class OrdersController(ISender sender) : ControllerBase
{
    /// POST /orders/checkout
    [HttpPost("checkout")]
    public async Task<IActionResult> Checkout([FromBody] CheckoutRequest req, CancellationToken ct)
    {
        var cmd = new PlaceOrderCommand(req);
        var result = await sender.Send(cmd, ct);

        if (result.IsFailure)
            return BadRequest(result.Error);

        return CreatedAtAction(nameof(GetById), new { id = result.Value, version = HttpContext.GetRequestedApiVersion()!.ToString() }, result.Value);
    }

    /// GET /orders/{id}
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
    {
        var res = await sender.Send(new GetOrderByIdQuery(id), ct); 
        return res.IsSuccess ? Ok(res.Value) : NotFound(res.Error);
    }

    /// GET /orders
    [HttpGet]
    public async Task<IActionResult> List(CancellationToken ct)
    {
        var res = await sender.Send(new ListOrdersQuery(), ct);   
        return Ok(res.Value);
    }
}
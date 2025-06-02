using BookStore.Application.Abstractions.Authentication;
using BookStore.Application.Abstractions.Interfaces.Persistence.Base;
using BookStore.Application.Abstractions.Messaging;
using BookStore.Contracts.Orders;
using BookStore.Domain.Orders.Errors;
using BookStore.Domain.Orders.ValueObjects.Ids;
using BookStore.SharedKernel.Abstractions;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Application.Orders.Queries.GetOrderById;

internal sealed class GetOrderByIdQueryHandler(
    IRepositoryManager repositories,
    ICurrentUserProvider user,
    IMapper mapper)
    : IQueryHandler<GetOrderByIdQuery, OrderResponse>
{
    public async Task<Result<OrderResponse>> Handle(GetOrderByIdQuery q, CancellationToken ct)
    {
        var userId = user.GetUserId;

        var order = await repositories.Orders
            .GetFilteredAsync(o => o.Id == OrderId.Create(q.OrderId)          
                                   && o.UserId == userId,
                trackChanges: false)
            .Include(o => o.Items)
            .ThenInclude(i => i.Book)
            .FirstOrDefaultAsync(ct);

        if (order is null)
            return Result.Failure<OrderResponse>(OrderErrors.OrderNotFound);

        var response = mapper.Map<OrderResponse>(order);
        return Result.Success(response);
    }
}
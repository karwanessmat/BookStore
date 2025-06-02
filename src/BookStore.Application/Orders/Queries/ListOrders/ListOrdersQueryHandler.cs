using BookStore.Application.Abstractions.Authentication;
using BookStore.Application.Abstractions.Interfaces.Persistence.Base;
using BookStore.Application.Abstractions.Messaging;
using BookStore.Contracts.Orders;
using BookStore.SharedKernel.Abstractions;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Application.Orders.Queries.ListOrders;

internal sealed class ListOrdersQueryHandler(
    IRepositoryManager repositories,
    ICurrentUserProvider user,
    IMapper mapper)
    : IQueryHandler<ListOrdersQuery, IEnumerable<OrderResponse>>
{
    public async Task<Result<IEnumerable<OrderResponse>>> Handle(
        ListOrdersQuery q, CancellationToken ct)
    {
        var userId = user.GetUserId;

        var orders = await repositories.Orders
            .GetFilteredAsync(o => o.UserId == userId, trackChanges: false)
            .Include(o => o.Items)
                .ThenInclude(i => i.Book)
            .OrderByDescending(o => o.OrderedDate)
            .ToListAsync(ct);

        var response = mapper.Map<List<OrderResponse>>(orders);
        return response;
    }
}
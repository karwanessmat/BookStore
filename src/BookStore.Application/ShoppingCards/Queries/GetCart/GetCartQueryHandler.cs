using BookStore.Application.Abstractions.Authentication;
using BookStore.Application.Abstractions.Interfaces.Persistence.Base;
using BookStore.Application.Abstractions.Messaging;
using BookStore.Contracts.ShoppingCards;
using BookStore.SharedKernel.Abstractions;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Application.ShoppingCards.Queries.GetCart;

internal sealed class GetCartQueryHandler(
    IRepositoryManager repositories,
    ICurrentUserProvider user,
    IMapper mapper)
    : IQueryHandler<GetCartQuery, CartResponse>
{
    public async Task<Result<CartResponse>> Handle(GetCartQuery q, CancellationToken ct)
    {
        var userId = user.GetUserId;
        var cart = await repositories.Carts.GetFilteredAsync(x => x.UserId == userId, true)
            .Include(x => x.Items)
            .ThenInclude(x=>x.Book)
            .FirstOrDefaultAsync(ct);

        var response = mapper.Map<CartResponse>(cart);

        return Result.Success(response);
    }
}
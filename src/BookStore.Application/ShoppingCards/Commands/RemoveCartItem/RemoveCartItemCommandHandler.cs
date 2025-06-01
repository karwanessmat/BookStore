using BookStore.Application.Abstractions.Authentication;
using BookStore.Application.Abstractions.Interfaces.Persistence.Base;
using BookStore.Application.Abstractions.Messaging;
using BookStore.Domain.ShoppingCards.Errors;
using BookStore.Domain.ShoppingCards.ValueObjects;
using BookStore.SharedKernel.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Application.ShoppingCards.Commands.RemoveCartItem;

internal sealed class RemoveCartItemCommandHandler(
    IRepositoryManager repositories,
    IUnitOfWork uow,
    ICurrentUserProvider user) : ICommandHandler<RemoveCartItemCommand>
{
    public async Task<Result> Handle(RemoveCartItemCommand cmd, CancellationToken ct)
    {
        var userId = user.GetUserId;
        var cart = await repositories.Carts.GetFilteredAsync(x=>x.UserId ==userId,true)
            .Include(x=>x.Items)
            .FirstOrDefaultAsync(ct);
        if (cart is null)
        {
            return Result.Failure<Guid>(CartErrors.NotFound);
        }

        cart.RemoveItem(CartItemId.Create(cmd.Request.CartItemId));

        await uow.SaveChangesAsync(ct);
        return Result.Success();
    }
}
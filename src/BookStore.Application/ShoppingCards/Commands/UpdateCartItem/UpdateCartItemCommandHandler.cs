using BookStore.Application.Abstractions.Authentication;
using BookStore.Application.Abstractions.Interfaces.Persistence.Base;
using BookStore.Application.Abstractions.Messaging;
using BookStore.Domain.ShoppingCards.Errors;
using BookStore.Domain.ShoppingCards.ValueObjects;
using BookStore.SharedKernel.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Application.ShoppingCards.Commands.UpdateCartItem;

internal sealed class UpdateCartItemCommandHandler(
    IRepositoryManager repositories,
    IUnitOfWork uow,
    ICurrentUserProvider user) : ICommandHandler<UpdateCartItemCommand>
{
    public async Task<Result> Handle(UpdateCartItemCommand cmd, CancellationToken ct)
    {
        var userId = user.GetUserId;
        var cart = await repositories.Carts.GetFilteredAsync(x => x.UserId == userId, true)
            .Include(x => x.Items)
            .FirstOrDefaultAsync(ct);
        if (cart is null)
        {
            return Result.Failure<Guid>(CartErrors.CartNotFound);
        }

        cart.UpdateQuantity(
            CartItemId.Create(cmd.Request.CartItemId),
            cmd.Request.Quantity);

        await uow.SaveChangesAsync(ct);
        return Result.Success();
    }
}
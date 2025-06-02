using BookStore.Application.Abstractions.Authentication;
using BookStore.Application.Abstractions.Interfaces.Persistence.Base;
using BookStore.Application.Abstractions.Messaging;
using BookStore.Domain.Orders;
using BookStore.Domain.Orders.ValueObjects;
using BookStore.Domain.ShoppingCards.Errors;
using BookStore.SharedKernel.Abstractions;
using BookStore.SharedKernel.Abstractions.IServices;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Application.Orders.Commands.PlaceOrder;

internal sealed class PlaceOrderCommandHandler(
    IRepositoryManager repositories,
    IUnitOfWork uow,
    ICurrentUserProvider user,
    IDateTimeProvider time) : ICommandHandler<PlaceOrderCommand, Guid>
{
    public async Task<Result<Guid>> Handle(PlaceOrderCommand cmd, CancellationToken ct)
    {
        var userId = user.GetUserId;

        var cart = await repositories.Carts
            .GetFilteredAsync(c => c.UserId == userId, trackChanges: true)
            .Include(c => c.Items)
                .ThenInclude(i => i.Book)
            .FirstOrDefaultAsync(ct);

        if (cart is null || !cart.Items.Any())
            return Result.Failure<Guid>(CartErrors.CartNotFound);

        var shippingAddress = ShippingAddress.Create(
        cmd.Request.FullName,
        cmd.Request.Line1,
        cmd.Request.Line2,
        cmd.Request.City,
        cmd.Request.State,
        cmd.Request.PostalCode,
        cmd.Request.Country);


        var nowUtc = time.DefaultUtcNow;
        var order = Order.CreateFromCart(
            cart,
            nowUtc,
            shippingAddress,
            cmd.Request.ShippingCost); 
        
        await repositories.Orders.AddAsync(order, ct);

        foreach (var item in cart.Items)
        {
            item.Book.DecreaseStock(item.Quantity);
        }


        repositories.Carts.Remove(cart);
        await uow.SaveChangesAsync(ct);
        return order.Id.Value;
    }
}
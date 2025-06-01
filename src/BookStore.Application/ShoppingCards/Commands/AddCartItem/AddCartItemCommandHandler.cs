using BookStore.Application.Abstractions.Authentication;
using BookStore.Application.Abstractions.Interfaces.Persistence.Base;
using BookStore.Application.Abstractions.Messaging;
using BookStore.Domain.BookAuthors.ValueObjects;
using BookStore.Domain.ShoppingCards;
using BookStore.Domain.ShoppingCards.Errors;
using BookStore.SharedKernel.Abstractions;
using BookStore.SharedKernel.Abstractions.IServices;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Application.ShoppingCards.Commands.AddCartItem;


internal sealed class AddCartItemCommandHandler(
    IRepositoryManager repositories,
    IUnitOfWork uow,
    ICurrentUserProvider user,
    IDateTimeProvider timeProvider)
    : ICommandHandler<AddCartItemCommand, Guid>
{
    public async Task<Result<Guid>> Handle(AddCartItemCommand cmd, CancellationToken ct)
    {
        var userId = user.GetUserId;

      
        var cart = await repositories.Carts
            .GetFilteredAsync(c => c.UserId == userId, trackChanges: true)
            .Include(c => c.Items)
                .ThenInclude(x=>x.Book)
            .FirstOrDefaultAsync(ct);

       
        if (cart is null)
        {
            var currentDateTime = timeProvider.DefaultUtcNow;
            cart = Cart.Create(currentDateTime, userId);
            await repositories.Carts.AddAsync(cart, ct);  
        }

        // Check the requested book
        var book = await repositories.Books.GetByIdAsync(BookId.Create(cmd.Request.BookId), ct);
        if (book is null)
            return Result.Failure<Guid>(CartErrors.BookNotFound);

        // Add / merge item
        cart.AddItem(book, book.Title, book.Price, cmd.Request.Quantity);

        await uow.SaveChangesAsync(ct);         
        return cart.Id.Value;                 
    }
}



//internal sealed class AddCartItemCommandHandler(
//    IRepositoryManager repositories,
//    IUnitOfWork uow,
//    ICurrentUserProvider user)
//    : ICommandHandler<AddCartItemCommand, Guid>
//{
//    public async Task<Result<Guid>> Handle(AddCartItemCommand cmd, CancellationToken ct)
//    {
//        var userId = user.GetUserId;
//        var cart = await repositories.Carts.GetFilteredAsync(x => x.UserId == userId, true)
//            .Include(x => x.Items)
//            .FirstOrDefaultAsync(ct);
//        if (cart is null)
//        {
//            return Result.Failure<Guid>(CartErrors.NotFound);
//        }


//        var book = await repositories.Books.GetByIdAsync(BookId.Create(cmd.Request.BookId), ct);
//        if (book is null)
//            return Result.Failure<Guid>(CartErrors.NotFound);

//        cart.AddItem(book, book.Title, book.Price, cmd.Request.Quantity);

//        await uow.SaveChangesAsync(ct);
//        return cart.Id.Value;
//    }
//}
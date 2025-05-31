using BookStore.Application.Abstractions.Interfaces.Persistence.Base;
using BookStore.Application.Abstractions.Messaging;
using BookStore.Domain.BookAuthors.Errors;
using BookStore.Domain.BookAuthors.ValueObjects;
using BookStore.SharedKernel.Abstractions;
using BookStore.SharedKernel.Abstractions.IServices;

namespace BookStore.Application.BookAuthors.Books.Commands.UpdateBook;

internal sealed class UpdateBookCommandHandler(
    IRepositoryManager repositoryManager,
    IDateTimeProvider dateTimeProvider,
    IUnitOfWork unitOfWork) : ICommandHandler<UpdateBookCommand, Guid>
{
    public async Task<Result<Guid>> Handle(UpdateBookCommand request, CancellationToken cancellationToken)
    {
        var bookId = BookId.Create(request.Request.BookId);
        var bookToUpdate = await repositoryManager.Books.GetByIdAsync(bookId, cancellationToken);

        if (bookToUpdate is null)
        {
            return Result.Failure<Guid>(BookErrors.NotFound);
        }

        bookToUpdate.Update(
            newTitle: request.Request.Title,
            newPrice: request.Request.Price,
            newStockQuantity: request.Request.StockQuantity,
            newDescription: request.Request.Description,
            newPublishedDate: request.Request.PublishedDate,
            newIsbn: request.Request.Isbn,
            newCoverImageUrl: request.Request.CoverImageUrl
        );


        repositoryManager.Books.Update(bookToUpdate);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return bookToUpdate.Id.Value;
    }
}
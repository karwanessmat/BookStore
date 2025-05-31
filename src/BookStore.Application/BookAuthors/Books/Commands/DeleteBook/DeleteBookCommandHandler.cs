using BookStore.Application.Abstractions.Interfaces.Persistence.Base;
using BookStore.Application.Abstractions.Messaging;
using BookStore.Domain.BookAuthors.Errors;
using BookStore.Domain.BookAuthors.ValueObjects;
using BookStore.SharedKernel.Abstractions;

namespace BookStore.Application.BookAuthors.Books.Commands.DeleteBook;

internal sealed class DeleteBookCommandHandler(
    IRepositoryManager repositoryManager,
    IUnitOfWork unitOfWork) : ICommandHandler<DeleteBookCommand>
{
    public async Task<Result> Handle(DeleteBookCommand request, CancellationToken cancellationToken)
    {
        var bookId = BookId.Create(request.BookId);
        var bookToDelete = await repositoryManager.Books.GetByIdAsync(bookId, cancellationToken);

        if (bookToDelete is null)
        {
            return Result.Failure<Guid>(BookErrors.NotFound);
        }

        repositoryManager.Books.Remove(bookToDelete);

        try
        {
            await unitOfWork.SaveChangesAsync(cancellationToken);
        }
        catch
        {
            return Result.Failure<Guid>(BookErrors.DeletionFailed);
        }

        return Result.Success();
    }
}
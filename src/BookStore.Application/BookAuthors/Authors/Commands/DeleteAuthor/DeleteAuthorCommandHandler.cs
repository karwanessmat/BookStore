using BookStore.Application.Abstractions.Interfaces.Persistence.Base;
using BookStore.Application.Abstractions.Messaging;
using BookStore.Domain.BookAuthors.Errors;
using BookStore.Domain.BookAuthors.ValueObjects;
using BookStore.SharedKernel.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Application.BookAuthors.Authors.Commands.DeleteAuthor
{
    internal sealed class DeleteAuthorCommandHandler(
        IRepositoryManager repositoryManager,
        IUnitOfWork unitOfWork
    ) : ICommandHandler<DeleteAuthorCommand>
    {
        public async Task<Result> Handle(
            DeleteAuthorCommand request,
            CancellationToken cancellationToken)
        {
            var authorId = AuthorId.Create(request.AuthorId);

            var author = await repositoryManager.Authors
                .GetByIdAsync(authorId, cancellationToken);

            if (author is null)
                return Result.Failure(AuthorErrors.NotFound);

            var hasBooks = await repositoryManager.Books
                .GetAllAsync()
                .AnyAsync(b => b.Authors.Any(a => a.Id == authorId), cancellationToken);

            if (hasBooks)
                return Result.Failure(AuthorErrors.DeletionFailed);

            repositoryManager.Authors.Remove(author);

            try
            {
                await unitOfWork.SaveChangesAsync(cancellationToken);
            }
            catch
            {
                return Result.Failure(AuthorErrors.HasLinkedBooks);
            }

            return Result.Success();
        }
    }
}

using BookStore.Application.Abstractions.Interfaces.Persistence.Base;
using BookStore.Application.Abstractions.Messaging;
using BookStore.Domain.BookAuthors.Errors;
using BookStore.Domain.BookAuthors.ValueObjects;
using BookStore.SharedKernel.Abstractions;

namespace BookStore.Application.BookAuthors.Authors.Commands.UpdateAuthor;

internal sealed class UpdateAuthorCommandHandler(
    IRepositoryManager repositoryManager,
    IUnitOfWork unitOfWork) : ICommandHandler<UpdateAuthorCommand, Guid>
{
    public async Task<Result<Guid>> Handle(
        UpdateAuthorCommand request,
        CancellationToken cancellationToken)
    {
        var dto = request.Request;
        var authorId = AuthorId.Create(dto.AuthorId);

        var author = await repositoryManager.Authors
            .GetByIdAsync(authorId, cancellationToken);

        if (author is null)
        {
            return Result.Failure<Guid>(AuthorErrors.NotFound);
        }

        author.Update(
            newName: dto.Name,
            newGender: dto.Gender,
            newBio: dto.Bio
        );

        repositoryManager.Authors.Update(author);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return author.Id.Value;
    }
}
using BookStore.Application.Abstractions.Interfaces.Persistence.Base;
using BookStore.Application.Abstractions.Messaging;
using BookStore.Domain.BookAuthors;
using BookStore.SharedKernel.Abstractions;
using BookStore.SharedKernel.Abstractions.IServices;

namespace BookStore.Application.BookAuthors.Authors.Commands.CreateAuthor;

internal sealed class CreateAuthorCommandHandler(
    IRepositoryManager repositoryManager,
    IDateTimeProvider dateTimeProvider,
    IUnitOfWork unitOfWork) : ICommandHandler<CreateAuthorCommand, Guid>
{
    public async Task<Result<Guid>> Handle(CreateAuthorCommand cmd, CancellationToken cancellationToken)
    {
        var author = Author.Create(
            created: dateTimeProvider.DefaultUtcNow,
            name: cmd.Request.Name,
            gender: cmd.Request.Gender,
            bio: cmd.Request.Bio
        );

        await repositoryManager.Authors.AddAsync(author, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return author.Id.Value;
    }
}
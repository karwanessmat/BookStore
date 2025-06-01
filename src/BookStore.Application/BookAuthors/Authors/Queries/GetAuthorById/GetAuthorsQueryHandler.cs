using BookStore.Application.Abstractions.Interfaces.Persistence.Base;
using BookStore.Application.Abstractions.Messaging;
using BookStore.Contracts.Authors;
using BookStore.Domain.BookAuthors.Errors;
using BookStore.Domain.BookAuthors.ValueObjects;
using BookStore.SharedKernel.Abstractions;
using MapsterMapper;

namespace BookStore.Application.BookAuthors.Authors.Queries.GetAuthorById;
internal sealed class GetAuthorByIdQueryHandler(
    IRepositoryManager repositoryManager,
    IMapper mapper
) : IQueryHandler<GetAuthorByIdQuery, AuthorResponse>
{
    public async Task<Result<AuthorResponse>> Handle(
        GetAuthorByIdQuery request,
        CancellationToken cancellationToken)
    {
        var authorId = AuthorId.Create(request.AuthorId);

        var author = await repositoryManager.Authors
            .GetByIdAsync(authorId, cancellationToken);

        if (author is null)
        {
            return Result.Failure<AuthorResponse>(AuthorErrors.NotFound);
        }

        var response = mapper.Map<AuthorResponse>(author);
        return response;
    }
}
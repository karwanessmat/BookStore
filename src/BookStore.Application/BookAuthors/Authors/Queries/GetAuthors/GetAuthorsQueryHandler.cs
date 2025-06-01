using BookStore.Application.Abstractions.Helpers;
using BookStore.Application.Abstractions.Interfaces.Persistence.Base;
using BookStore.Application.Abstractions.Messaging;
using BookStore.Contracts.Authors;
using BookStore.Domain.BookAuthors;
using BookStore.SharedKernel.Abstractions;
using BookStore.SharedKernel.Books;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Application.BookAuthors.Authors.Queries.GetAuthors;

internal sealed class GetAuthorsQueryHandler(
    IRepositoryManager repositoryManager,
    IMapper mapper) : IQueryHandler<GetAuthorsQuery, PagedList<AuthorResponse>>
{
    public async Task<Result<PagedList<AuthorResponse>>> Handle(
        GetAuthorsQuery request,
        CancellationToken cancellationToken)
    {
        IQueryable<Author> source = repositoryManager.Authors
            .GetAllAsync()
            .Include(x=>x.Books);

        if (!string.IsNullOrWhiteSpace(request.Parameters.SearchTerm))
        {
            var term = request.Parameters.SearchTerm.Trim().ToLowerInvariant();
            source = source.Where(a =>
                EF.Functions.Like(a.Name.ToLower(), $"%{term}%")
                || (!string.IsNullOrWhiteSpace(a.Bio) &&
                    EF.Functions.Like(a.Bio.ToLower(), $"%{term}%"))
            );
        }

        if (request.Parameters.Gender is { } gender)
        {
            source = source.Where(a => a.Gender == gender);
        }

        var sortDescending = request.Parameters.SortOrder == AuthorOrderBy.NameDescending;
        source = sortDescending
            ? source.OrderByDescending(a => a.Name)
            : source.OrderBy(a => a.Name);

        var query = source.Select(a => mapper.Map<AuthorResponse>(a));

        var result = await PagedList<AuthorResponse>.ToCreatePageListAsync(
            query,
            request.Parameters.PageNumber,
            request.Parameters.PageSize
        );

        return result;
    }
}
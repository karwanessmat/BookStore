using BookStore.Application.Abstractions.Helpers;
using BookStore.Application.Abstractions.Interfaces.Persistence.Base;
using BookStore.Application.Abstractions.Messaging;
using BookStore.Contracts.Books;
using BookStore.Domain.BookAuthors;
using BookStore.SharedKernel.Abstractions;
using BookStore.SharedKernel.Books;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
// Book & Author aggregates

namespace BookStore.Application.BookAuthors.Books.Queries.GetBookAuthors;

internal sealed class GetBooksQueryHandler(
    IRepositoryManager repositoryManager,
    IMapper mapper)
    : IQueryHandler<GetBookAuthorsQuery, PagedList<BookResponse>>
{
    public async Task<Result<PagedList<BookResponse>>> Handle(
        GetBookAuthorsQuery request,
        CancellationToken cancellationToken)
    {
        IQueryable<Book> source = repositoryManager.Books
            .GetAllAsync()
            .AsNoTracking()
            .Include(b => b.Authors);            

        if (!string.IsNullOrWhiteSpace(request.Parameters.SearchTerm))
        {
            string term = request.Parameters.SearchTerm.Trim()
                                                     .ToLowerInvariant();

            source = source.Where(b =>
                   EF.Functions.Like(b.Title.ToLower(), $"%{term}%")
                || b.Authors.Any(a => EF.Functions.Like(a.Name.ToLower(), $"%{term}%")));
        }

        if (request.Parameters.Gender is { } gender)
        {
            source = source.Where(b => b.Authors.Any(a => a.Gender == gender));
        }

        if (!string.IsNullOrEmpty(request.Parameters.Author))
        {
            var authorName = request.Parameters.Author.Trim().ToLowerInvariant();
            source = source.Where(b => b.Authors.Any(a => a.Name.ToLower().Contains(authorName)));
        }

        if (request.Parameters.IsAvailable is { } isAvail)
        {
            source = isAvail
                ? source.Where(b => b.StockQuantity > 0)
                : source.Where(b => b.StockQuantity == 0);
        }

        BookOrderBy sort = request.Parameters.SortOrder ?? BookOrderBy.TitleAscending;

        source = sort switch
        {
            BookOrderBy.TitleAscending => source.OrderBy(b => b.Title),
            BookOrderBy.TitleDescending => source.OrderByDescending(b => b.Title),
            BookOrderBy.AuthorAscending => source.OrderBy(b => b.Authors.Min(a => a.Name)),
            BookOrderBy.AuthorDescending => source.OrderByDescending(b => b.Authors.Min(a => a.Name)),
            BookOrderBy.PriceAscending => source.OrderBy(b => b.Price),
            BookOrderBy.PriceDescending => source.OrderByDescending(b => b.Price),
            BookOrderBy.QuantityAscending => source.OrderBy(b => b.StockQuantity),
            BookOrderBy.QuantityDescending => source.OrderByDescending(b => b.StockQuantity),
            _ => source.OrderBy(b => b.Title)
        };
        
        var query = source.Select(b => mapper.Map<BookResponse>(b));

        var result = await PagedList<BookResponse>.ToCreatePageListAsync(
            query,
            request.Parameters.PageNumber,
            request.Parameters.PageSize);

        return result;         
    }
}

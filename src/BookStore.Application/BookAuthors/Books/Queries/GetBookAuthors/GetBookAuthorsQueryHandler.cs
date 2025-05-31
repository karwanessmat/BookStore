using BookStore.Application.Abstractions.Helpers;
using BookStore.Application.Abstractions.Interfaces.Persistence.Base;
using BookStore.Application.Abstractions.Messaging;
using BookStore.Application.BookAuthors.Books.Queries.GetBookAuthors;
using BookStore.Contracts.Books;
using BookStore.Domain.BookAuthors;          // Book & Author aggregates
using BookStore.SharedKernel.Abstractions;
using BookStore.SharedKernel.Books;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace BookStore.Application.BookAuthors.Books.Queries.GetBooks;

internal sealed class GetBooksQueryHandler(
    IRepositoryManager repositoryManager,
    IMapper mapper)
    : IQueryHandler<GetBookAuthorsQuery, PagedList<BookResponse>>
{
    public async Task<Result<PagedList<BookResponse>>> Handle(
        GetBookAuthorsQuery request,
        CancellationToken cancellationToken)
    {
        //----------------------------------------------------------
        // 1. Base query – include authors
        //----------------------------------------------------------
        IQueryable<Book> source = repositoryManager.Books
            .GetAllAsync()
            .AsNoTracking()
            .Include(b => b.Authors);            // EF many-to-many

        //----------------------------------------------------------
        // 2. Search term (book title OR author name)
        //----------------------------------------------------------
        if (!string.IsNullOrWhiteSpace(request.Parameters.SearchTerm))
        {
            string term = request.Parameters.SearchTerm.Trim()
                                                     .ToLowerInvariant();

            source = source.Where(b =>
                   EF.Functions.Like(b.Title.ToLower(), $"%{term}%")
                || b.Authors.Any(a => EF.Functions.Like(a.Name.ToLower(), $"%{term}%")));
        }

        //----------------------------------------------------------
        // 3. Filter by author gender
        //----------------------------------------------------------
        if (request.Parameters.Gender is { } gender)
        {
            source = source.Where(b => b.Authors.Any(a => a.Gender == gender));
        }

        //----------------------------------------------------------
        // 4. Filter by availability
        //----------------------------------------------------------
        if (request.Parameters.IsAvailable is { } isAvail)
        {
            source = isAvail
                ? source.Where(b => b.StockQuantity > 0)
                : source.Where(b => b.StockQuantity == 0);
        }

        //----------------------------------------------------------
        // 5. Sorting
        //----------------------------------------------------------
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

        //----------------------------------------------------------
        // 6. Projection → DTO + pagination
        //----------------------------------------------------------
        var query = source.Select(b => mapper.Map<BookResponse>(b));

        var page = await PagedList<BookResponse>.ToCreatePageListAsync(
            query,
            request.Parameters.PageNumber,
            request.Parameters.PageSize);

        return page;            // wrap in Result.Success if your convention requires it
    }
}

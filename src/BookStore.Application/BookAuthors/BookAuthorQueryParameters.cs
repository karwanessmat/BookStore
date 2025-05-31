using BookStore.SharedKernel.Abstractions.Helpers;
using BookStore.SharedKernel.Books;

namespace BookStore.Application.BookAuthors;

public class BookAuthorQueryParameters : QueryParameters
{
    public string? SearchTerm { get; set; }
    public string? Author { get; set; }
    public decimal? MinPrice { get; set; }
    public decimal? MaxPrice { get; set; }
    public Gender? Gender { get; set; }
    public bool? IsAvailable { get; set; }
    public BookOrderBy? SortOrder { get; set; } = BookOrderBy.TitleAscending;
}
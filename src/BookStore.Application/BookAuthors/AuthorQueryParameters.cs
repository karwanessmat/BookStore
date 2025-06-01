using BookStore.SharedKernel.Abstractions.Helpers;
using BookStore.SharedKernel.Books;

namespace BookStore.Application.BookAuthors;

public class AuthorQueryParameters : QueryParameters
{
    public AuthorQueryParameters()
    {
        
    }
    public Gender? Gender { get; set; }
    public AuthorOrderBy? SortOrder { get; set; } = AuthorOrderBy.NameAscending;
}
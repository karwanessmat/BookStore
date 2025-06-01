using BookStore.SharedKernel.Books;

namespace BookStore.Contracts.Authors;

public record AuthorResponse(
    Guid AuthorId,
    string Name, 
    Gender Gender,
    DateTimeOffset CreatedDate,         
    string? Bio,
    List<AuthorBooksResponse> BooksResponses);


public record AuthorBooksResponse(Guid BookId, string Title);


public class AuthorResponseFromSql
{
    public Guid AuthorId { get; set; }
    public string Name { get; set; } 
    public int Gender { get; set; }
    public DateTimeOffset CreatedDate { get; set; }
    public string? Bio { get; set; }
    public List<AuthorBooksResponse> BooksResponses { get; set; } = [];
}


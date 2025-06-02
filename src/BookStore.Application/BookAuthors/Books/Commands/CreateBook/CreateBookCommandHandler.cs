using BookStore.Application.Abstractions.Interfaces.Persistence.Base;
using BookStore.Application.Abstractions.Messaging;
using BookStore.Domain.BookAuthors;
using BookStore.Domain.BookAuthors.Errors;
using BookStore.Domain.BookAuthors.ValueObjects;
using BookStore.SharedKernel.Abstractions;
using BookStore.SharedKernel.Abstractions.IServices;
using MapsterMapper;

namespace BookStore.Application.BookAuthors.Books.Commands.CreateBook;

internal sealed class CreateBookCommandHandler(
    IRepositoryManager repositoryManager,
    IDateTimeProvider dateTimeProvider,
    IUnitOfWork unitOfWork,
    IMapper mapper) : ICommandHandler<CreateBookCommand, Guid>
{
    public async Task<Result<Guid>> Handle(CreateBookCommand cmd, CancellationToken cancellationToken)
    {

        // 1) Grab the raw GUIDs
        var incomingGuids = cmd.Request.AuthorIds.Select(AuthorId.Create).ToList();


        if (!incomingGuids.Any())
            return Result.Failure<Guid>(AuthorErrors.AuthorIdRequired);

        var bookAuthorLinks = new List<Author>();
        foreach (var id in incomingGuids)
        {
            var author = await repositoryManager.Authors
                .FirstOrDefaultAsync(x =>x.Id == id, cancellationToken);
            if (author is not null)
            {
                bookAuthorLinks.Add(author);
            }
        }


        if (bookAuthorLinks.Count != cmd.Request.AuthorIds.Count())
            return Result.Failure<Guid>(AuthorErrors.NotFound);

        // Create AuthorId value objects only when needed
        var authorIds = cmd.Request.AuthorIds.Select(AuthorId.Create).ToList();


        var book = Book.Create(
            created: dateTimeProvider.DefaultUtcNow,
            title: cmd.Request.Title,
            price: cmd.Request.Price,
            stockQuantity: cmd.Request.StockQuantity,
            authors: bookAuthorLinks,
            description: cmd.Request.Description,
            publishedDate: cmd.Request.PublishedDate,
            isbn: cmd.Request.Isbn,
            coverImageUrl: cmd.Request.CoverImageUrl);


        await repositoryManager.Books.AddAsync(book, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return book.Id.Value;
    }
}


//var authorIds = string.Join(',', cmd.Request.AuthorIds.Select(s => $"'{s}'").ToList());

//if (!authorIds.Any())
//    return Result.Failure<Guid>(AuthorErrors.AuthorIdRequired);

//var sqlQuery = $"""
//                SELECT AuthorId, CreatedDate, Name, Gender, Bio 
//                FROM Authors
//                WHERE AuthorId in ({authorIds})
//                """;
//var query = await repositoryManager
//    .Authors
//    .SelectSqlQueryListAsync<AuthorResponseFromSql>(sqlQuery);

//var authors = query.Select(mapper.Map<Author>).ToList();
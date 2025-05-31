using BookStore.Application.Abstractions.Interfaces.Persistence.Base;
using BookStore.Application.Abstractions.Messaging;
using BookStore.Domain.BookAuthors;
using BookStore.Domain.BookAuthors.Errors;
using BookStore.Domain.BookAuthors.ValueObjects;
using BookStore.SharedKernel.Abstractions;
using BookStore.SharedKernel.Abstractions.IServices;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Application.BookAuthors.Books.Commands.CreateBook;

internal sealed class CreateBookCommandHandler(
    IRepositoryManager repositoryManager,
    IDateTimeProvider dateTimeProvider,
    IUnitOfWork unitOfWork) : ICommandHandler<CreateBookCommand, Guid>
{
    public async Task<Result<Guid>> Handle(CreateBookCommand request, CancellationToken cancellationToken)
    {
        var authorIds = request.Request.AuthorIds?
            .Select(AuthorId.Create)
            .ToList() ?? new List<AuthorId>();

        if (!authorIds.Any())
            return Result.Failure<Guid>(AuthorErrors.AuthorIdRequired);

        var bookAuthorLinks = await repositoryManager.Authors
            .GetFilteredAsync(a => authorIds.Contains(a.Id))
            .ToListAsync(cancellationToken);


        var authors = bookAuthorLinks
            .DistinctBy(a => a.Id)     
            .ToList();

        if (authors.Count != authorIds.Count)
            return Result.Failure<Guid>(AuthorErrors.NotFound);

        var createdDate = dateTimeProvider.DefaultUtcNow;

        // 2) Call Book.Create with createdDate first, then all other fields from the command.
        var book = Book.Create(
            created: dateTimeProvider.DefaultUtcNow,
            title: request.Request.Title,
            price: request.Request.Price,
            stockQuantity: request.Request.StockQuantity,
            authors: authors,
            description: request.Request.Description,
            publishedDate: request.Request.PublishedDate,
            isbn: request.Request.Isbn,
            coverImageUrl: request.Request.CoverImageUrl);


        await repositoryManager.Books.AddAsync(book, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return book.Id.Value;
    }
}
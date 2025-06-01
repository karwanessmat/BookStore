
using BookStore.Application.Abstractions.Interfaces.Persistence.Base;
using BookStore.Application.Abstractions.Messaging;
using BookStore.Contracts.Books;
using BookStore.Domain.BookAuthors.Errors;
using BookStore.Domain.BookAuthors.ValueObjects;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using BookStore.SharedKernel.Abstractions;

namespace BookStore.Application.BookAuthors.Books.Queries.GetBookById
{
    internal sealed class GetBookByIdQueryHandler(
        IRepositoryManager repositoryManager,
        IMapper mapper) : IQueryHandler<GetBookByIdQuery, BookResponse>
    {
        public async Task<Result<BookResponse>> Handle(
            GetBookByIdQuery request,
            CancellationToken cancellationToken)
        {
            var bookId = BookId.Create(request.BookId.Value);

            var book = await repositoryManager.Books
                .GetAllAsync()              
                .Include(x=>x.Authors)
                .FirstOrDefaultAsync(b => b.Id == bookId, cancellationToken);

            if (book is null)
            {
                return Result.Failure<BookResponse>(BookErrors.NotFound);
            }

            var response = mapper.Map<BookResponse>(book);

            return response;
        }
    }
}

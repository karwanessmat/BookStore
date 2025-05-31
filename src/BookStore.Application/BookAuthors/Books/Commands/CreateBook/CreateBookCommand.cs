using BookStore.Application.Abstractions.Messaging;
using BookStore.Contracts.Books;

namespace BookStore.Application.BookAuthors.Books.Commands.CreateBook;

public record CreateBookCommand(CreateBookRequest Request) : ICommand<Guid>;
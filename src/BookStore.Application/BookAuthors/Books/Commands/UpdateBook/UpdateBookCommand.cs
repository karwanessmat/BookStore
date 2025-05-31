using BookStore.Application.Abstractions.Messaging;
using BookStore.Contracts.Books;

namespace BookStore.Application.BookAuthors.Books.Commands.UpdateBook;

public record UpdateBookCommand(UpdateBookRequest Request) : ICommand<Guid>;
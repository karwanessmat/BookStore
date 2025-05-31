using BookStore.Application.Abstractions.Messaging;

namespace BookStore.Application.BookAuthors.Books.Commands.DeleteBook;

public record DeleteBookCommand(Guid BookId) : ICommand;
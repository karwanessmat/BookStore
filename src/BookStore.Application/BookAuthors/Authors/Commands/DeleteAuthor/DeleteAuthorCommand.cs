using BookStore.Application.Abstractions.Messaging;

namespace BookStore.Application.BookAuthors.Authors.Commands.DeleteAuthor;

public record DeleteAuthorCommand(Guid AuthorId) : ICommand;
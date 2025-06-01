using BookStore.Application.Abstractions.Messaging;
using BookStore.Contracts.Authors;

namespace BookStore.Application.BookAuthors.Authors.Commands.UpdateAuthor;

public record UpdateAuthorCommand(UpdateAuthorRequest Request) : ICommand<Guid>;
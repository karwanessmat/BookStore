using BookStore.Application.Abstractions.Messaging;
using BookStore.Contracts.Authors;

namespace BookStore.Application.BookAuthors.Authors.Commands.CreateAuthor;

public record CreateAuthorCommand(CreateAuthorRequest Request) : ICommand<Guid>;
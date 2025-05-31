using BookStore.Application.Abstractions.Messaging;
using BookStore.Contracts.Users;

namespace BookStore.Application.Users.User.Commands.Register;

public sealed record RegisterCommand(RegisterRequest Request) : ICommand<Guid>;

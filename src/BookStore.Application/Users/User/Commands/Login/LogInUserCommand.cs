using BookStore.Application.Abstractions.Messaging;

namespace BookStore.Application.Users.User.Commands.Login;

public sealed record LogInUserCommand(string Email, string Password) : ICommand<string>;

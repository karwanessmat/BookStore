using BookStore.Application.Abstractions.Messaging;

namespace BookStore.Application.Users.User.Commands.Logout;

public sealed record LogoutCommand(string AccessToken) : ICommand<bool>;     

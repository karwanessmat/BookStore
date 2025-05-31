using Asp.Versioning;
using BookStore.Application.Abstractions.Interfaces.Authentication;
using BookStore.Application.Users.User.Commands.Login;
using BookStore.Application.Users.User.Commands.Logout;
using BookStore.Application.Users.User.Commands.Register;
using BookStore.Contracts.Users;
using BookStore.SharedKernel.Abstractions;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.Server.Controllers.User;

[ApiController]
[ApiVersion(ApiVersions.V1)]
[Route("api/v{version:apiVersion}/users")]

public class UsersController (ISender sender, ITokenAccessor tokenAccessor) : ControllerBase
{

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest registerRequest)
    {
        var command = registerRequest.Adapt<RegisterCommand>();
        var result = await sender.Send(command);
        if (result.IsFailure)
        {
            return BadRequest(result.Error);
        }
        return Ok(result.Value);
    }


    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LogInUserRequest request,
        CancellationToken cancellationToken)
    {
        var command = new LogInUserCommand(request.Email, request.Password);
        Result<string> result = await sender.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            return Unauthorized(result.Error);
        }

        var response = new AccessTokenResponse(result.Value);
        return Ok(response);
    }

    [HttpPost("logout")]
    [Authorize]
    public async Task<IActionResult> Logout(CancellationToken ct)
    {
        var token = tokenAccessor.GetAccessToken();
        if (string.IsNullOrEmpty(token))
        {
            return Unauthorized("No token provided");
        }
        var result = await sender.Send(new LogoutCommand(token), ct);

        if (result.IsFailure)
        {
            return Unauthorized(result.Error);
        }
        return Ok(result.Value);


    }
}

using System.Reflection;
using BookStore.Application.Abstractions.Authentication;
using BookStore.Application.Abstractions.Authorization;
using BookStore.Application.Abstractions.Interfaces.Persistence.User;
using BookStore.SharedKernel.Abstractions;
using BookStore.SharedKernel.Abstractions.Errors;
using BookStore.SharedKernel.User;
using MediatR;

namespace BookStore.Application.Abstractions.Behaviors;

internal sealed class BookstoreAuthorizationBehavior<TRequest, TResponse>(
    ICurrentUserProvider currentUserProvider,
    IUserRepository userRepository)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : Result // Explicitly define TResponse as Result
{
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var authorizationAttributes = request.GetType()
            .GetCustomAttributes<BookstoreAuthorizeAttribute>()
            .ToList();

        if (!authorizationAttributes.Any())
        {
            return await next();
        }

        var currentUser = currentUserProvider.GetCurrentUser();

        var requiredRoles = authorizationAttributes
            .SelectMany(attr => attr.Roles?.Split(',') ?? [])
            .ToList();

        if (requiredRoles.Any() && currentUser.Roles.Contains(RoleName.Admin))
        {
            return await next();
        }

        // Check for required roles
        if (requiredRoles.Any() && !requiredRoles.Intersect(currentUser.Roles).Any())
        {
            return CreateFailureResult("User is forbidden from taking this action");
        }

        var requiredPermissions = authorizationAttributes
            .SelectMany(attr => attr.Permissions?.Split(',') ?? Array.Empty<string>())
            .ToList();
        
        return await next();
    }


    private TResponse CreateFailureResult(string errorMessage)
    {
        var error = Error.Forbidden(errorMessage);

        Type resultType = typeof(Result<>).MakeGenericType(typeof(TResponse).GetGenericArguments().First());
        object? resultInstance = Activator.CreateInstance(resultType, null, false, error);

        return (TResponse)resultInstance;
    }
}

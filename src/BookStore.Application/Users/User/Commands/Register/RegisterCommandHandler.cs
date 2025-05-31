using BookStore.Application.Abstractions.Interfaces.Persistence.User;
using BookStore.Application.Abstractions.Messaging;
using BookStore.Domain.ApplicationUsers;
using BookStore.Domain.ApplicationUsers.Errors;
using BookStore.SharedKernel.Abstractions;
using BookStore.SharedKernel.User;

namespace BookStore.Application.Users.User.Commands.Register;

internal sealed class RegisterCommandHandler(IUserRepository userRepository) : ICommandHandler<RegisterCommand, Guid>
{
    public async Task<Result<Guid>> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {

        var appUser = await userRepository.GetUserByEmail(request.Request.Email);
        if (appUser is not null)
        {
            return Result.Failure<Guid>(UserErrors.DuplicateEmail);
        }

        var (fullName, email, phoneNumber,password) = (request.Request.FullName, 
                                                                         request.Request.Email,
                                                                         request.Request.PhoneNumber,
                                                                         request.Request.Password);

        var newUser = ApplicationUser.Create(fullName, email, phoneNumber);

        if (newUser.IsFailure)
        {
            return Result.Failure<Guid>(UserErrors.RegistrationFailed);

        }
        var (createdNewUser, succeeded) = await userRepository.Register(newUser.Value, password);
        if (!succeeded)
        {
            return Result.Failure<Guid>(UserErrors.RegistrationFailed);
        }

        var addRoleCreatedNewUser= await userRepository.AddRoleAsync(createdNewUser, RoleName.Customer);
        return !addRoleCreatedNewUser ? Result.Failure<Guid>(RoleErrors.AssignmentFailed) : createdNewUser!.Id;
    }
}

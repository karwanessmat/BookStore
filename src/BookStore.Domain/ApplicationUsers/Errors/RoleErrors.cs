using BookStore.SharedKernel.Abstractions.Errors;

namespace BookStore.Domain.ApplicationUsers.Errors;

public static class RoleErrors
{
    // Retrieval Errors
    public static readonly Error NotFound = new(
        "Role.NotFound",
        "The role with the specified identifier or name was not found",
        ErrorType.NotFound);

    // Creation Errors
    public static readonly Error DuplicateName = new(
        "Role.DuplicateName",
        "A role with the specified name already exists",
        ErrorType.Conflict);

    public static readonly Error CreationFailed = new(
        "Role.CreationFailed",
        "Failed to create the role due to an internal error",
        ErrorType.Failure);

    public static readonly Error InvalidName = new(
        "Role.InvalidName",
        "The provided role name is invalid or does not meet naming requirements",
        ErrorType.Invalid);

    // Update Errors
    public static readonly Error UpdateFailed = new(
        "Role.UpdateFailed",
        "Failed to update the role",
        ErrorType.Failure);

    public static readonly Error NameConflictOnUpdate = new(
        "Role.NameConflictOnUpdate",
        "Cannot update role: the new name conflicts with an existing role",
        ErrorType.Conflict);

    // Deletion Errors
    public static readonly Error DeletionFailed = new(
        "Role.DeletionFailed",
        "Failed to delete the role",
        ErrorType.Failure);

    public static readonly Error RoleInUse = new(
        "Role.InUse",
        "The role is currently assigned to one or more users and cannot be deleted",
        ErrorType.Conflict);

    public static readonly Error UnauthorizedDeletion = new(
        "Role.UnauthorizedDeletion",
        "You do not have permission to delete this role",
        ErrorType.Unauthorized);

    // Assignment Errors
    public static readonly Error UserAlreadyAssigned = new(
        "Role.UserAlreadyAssigned",
        "The user is already assigned to this role",
        ErrorType.Conflict);

    public static readonly Error UserNotAssigned = new(
        "Role.UserNotAssigned",
        "The user is not assigned to the specified role",
        ErrorType.NotFound);

    public static readonly Error AssignmentFailed = new(
        "Role.AssignmentFailed",
        "Failed to assign the user to the role",
        ErrorType.Failure);

    public static readonly Error RemovalFailed = new(
        "Role.RemovalFailed",
        "Failed to remove the user from the role",
        ErrorType.Failure);

    // Authorization Errors
    public static readonly Error UnauthorizedAccess = new(
        "Role.UnauthorizedAccess",
        "You do not have permission to perform this operation on roles",
        ErrorType.Unauthorized);

    public static readonly Error ForbiddenAction = new(
        "Role.ForbiddenAction",
        "This action on the role is forbidden",
        ErrorType.Forbidden);

    // Miscellaneous Errors
    public static readonly Error OperationFailed = new(
        "Role.OperationFailed",
        "The requested operation on the role could not be completed",
        ErrorType.Failure);

    public static readonly Error ServiceUnavailable = new(
        "Role.ServiceUnavailable",
        "The role service is currently unavailable. Please try again later",
        ErrorType.Invalid);

    public static readonly Error TooManyRequests = new(
        "Role.TooManyRequests",
        "Too many requests have been made to the role service. Please wait and try again",
        ErrorType.Forbidden);
}

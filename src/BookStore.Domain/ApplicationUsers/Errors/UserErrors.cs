using BookStore.SharedKernel.Abstractions.Errors;

namespace BookStore.Domain.ApplicationUsers.Errors;
public static class UserErrors
{
    // General Errors
    public static readonly Error NotFound = new(
        "User.NotFound",
        "The user with the specified identifier was not found",
        ErrorType.NotFound);

    public static readonly Error InvalidCredentials = new(
        "User.InvalidCredentials",
        "The provided credentials were invalid",
        ErrorType.NotFound);

    public static readonly Error DuplicateEmail = new(
        "User.DuplicateEmail",
        "This email address is already in use",
        ErrorType.Conflict);

    public static readonly Error InactiveUser = new(
        "User.InactiveUser",
        "This user ID is inactive",
        ErrorType.Conflict);

    public static readonly Error UnauthorizedAccess = new(
        "User.UnauthorizedAccess",
        "You are not authorized to view details of other users",
        ErrorType.Unauthorized);

    public static readonly Error PhoneNumberAlreadyRegistered = new(
        "User.PhoneNumberAlreadyRegistered",
        "This phone number has already been registered",
        ErrorType.Conflict);

    public static readonly Error Forbidden = new(
        "User.Forbidden",
        "You are not authorized to view this resource",
        ErrorType.Forbidden);

    public static readonly Error InvalidResetCode = new(
        "User.InvalidResetCode",
        "The reset code does not match the specified email",
        ErrorType.Invalid);

    // Update Errors
    public static readonly Error NotUpdated = new(
        "User.NotUpdated",
        "The user details could not be updated",
        ErrorType.Failure);

    public static readonly Error EmailUpdateFailed = new(
        "User.EmailUpdateFailed",
        "Failed to update the email address",
        ErrorType.Failure);

    public static readonly Error PasswordUpdateFailed = new(
        "User.PasswordUpdateFailed",
        "Failed to update the password",
        ErrorType.Failure);

    public static readonly Error ProfileUpdateFailed = new(
        "User.ProfileUpdateFailed",
        "Failed to update the user profile details",
        ErrorType.Failure);

    // Deletion Errors
    public static readonly Error NotDeleted = new(
        "User.NotDeleted",
        "The user could not be deleted",
        ErrorType.Failure);

    public static readonly Error UnauthorizedDeletion = new(
        "User.UnauthorizedDeletion",
        "You do not have the required permissions to delete this user",
        ErrorType.Unauthorized);

    // Creation Errors
    public static readonly Error NotCreated = new(
        "User.NotCreated",
        "The user could not be created",
        ErrorType.Failure);

    public static readonly Error InvalidData = new(
        "User.InvalidData",
        "The provided user data is invalid",
        ErrorType.Invalid);

    public static readonly Error DuplicateUsername = new(
        "User.DuplicateUsername",
        "This username is already in use",
        ErrorType.Conflict);

    // ─── New: Registration Failure Error ───────────────────────
    public static readonly Error RegistrationFailed = new(
        "User.RegistrationFailed",
        "Registration failed due to an unexpected error",
        ErrorType.Failure);

    // Authentication and Authorization Errors
    public static readonly Error AuthenticationFailed = new(
        "User.AuthenticationFailed",
        "Authentication failed due to invalid credentials",
        ErrorType.Unauthorized);

    public static readonly Error SessionExpired = new(
        "User.SessionExpired",
        "Your session has expired. Please log in again",
        ErrorType.Unauthorized);

    public static readonly Error AccessRevoked = new(
        "User.AccessRevoked",
        "Your access to the system has been revoked",
        ErrorType.Forbidden);

    // Miscellaneous Errors
    public static readonly Error OperationFailed = new(
        "User.OperationFailed",
        "The requested operation could not be completed",
        ErrorType.Failure);

    public static readonly Error ServiceUnavailable = new(
        "User.ServiceUnavailable",
        "The user service is currently unavailable. Please try again later",
        ErrorType.Invalid);

    public static readonly Error InvalidInput = new(
        "User.InvalidInput",
        "The provided input is invalid",
        ErrorType.Invalid);

    public static readonly Error TooManyRequests = new(
        "User.TooManyRequests",
        "Too many requests have been made. Please wait and try again",
        ErrorType.Forbidden);

    public static readonly Error PasswordResetFailed = new(
        "User.PasswordResetFailed",
        "Failed to reset the password",
        ErrorType.Failure);

    public static readonly Error EmailNotVerified = new(
        "User.EmailNotVerified",
        "The email address is not verified",
        ErrorType.Conflict);

    public static readonly Error ActionProhibited = new(
        "User.ActionProhibited",
        "This action is not allowed for the current user",
        ErrorType.Forbidden);
}

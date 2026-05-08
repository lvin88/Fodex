using Fodex.Common.Domain.Errors;

namespace Fodex.Modules.Identity.Domain.Users;

/// <summary>Error constants for the <see cref="User"/> aggregate.</summary>
public static class UserErrors
{
    public static readonly Error NotFound =
        Error.NotFound("User.NotFound", "User was not found.");

    public static readonly Error InvalidCredentials =
        Error.Unauthorized("User.InvalidCredentials", "The provided credentials are invalid.");

    public static readonly Error NumberAlreadyInUse =
        Error.Conflict("User.NumberAlreadyInUse", "A user with this number already exists.");

    public static readonly Error NumberEmpty =
        Error.Validation("User.NumberEmpty", "User number cannot be empty.");

    public static readonly Error NumberTooLong =
        Error.Validation("User.NumberTooLong", $"User number cannot exceed {User.NumberMaxLength} characters.");

    public static readonly Error PasswordEmpty =
        Error.Validation("User.PasswordEmpty", "Password hash cannot be empty.");

    public static readonly Error PasswordHashTooLong =
        Error.Validation("User.PasswordHashTooLong", $"Password hash cannot exceed {User.PasswordHashMaxLength} characters.");

    public static readonly Error AccountInactive =
        Error.Forbidden("User.AccountInactive", "The account is inactive.");

    public static readonly Error AccountSuspended =
        Error.Forbidden("User.AccountSuspended", "The account is suspended.");

    public static readonly Error TypeMismatch =
        Error.Failure("User.TypeMismatch", "The profile type does not match the user type.");
}

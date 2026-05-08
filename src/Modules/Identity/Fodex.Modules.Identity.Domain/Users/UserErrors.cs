using Fodex.Common.Domain.Errors;

namespace Fodex.Modules.Identity.Domain.Users;

/// <summary>Error factory methods for the <see cref="User"/> aggregate.</summary>
public static class UserErrors
{
    public static Error NotFound(UserId id) =>
        Error.NotFound("User.NotFound", $"User '{id}' was not found.");

    public static Error InvalidCredentials() =>
        Error.Unauthorized("User.InvalidCredentials", "The provided credentials are invalid.");

    public static Error NumberAlreadyInUse(string number) =>
        Error.Conflict("User.NumberAlreadyInUse", $"A user with number '{number}' already exists.");

    public static Error NumberEmpty() =>
        Error.Validation("User.NumberEmpty", "User number cannot be empty.");

    public static Error NumberTooLong() =>
        Error.Validation("User.NumberTooLong", $"User number cannot exceed {User.NumberMaxLength} characters.");

    public static Error PasswordEmpty() =>
        Error.Validation("User.PasswordEmpty", "Password hash cannot be empty.");

    public static Error PasswordHashTooLong() =>
        Error.Validation("User.PasswordHashTooLong", $"Password hash cannot exceed {User.PasswordHashMaxLength} characters.");

    public static Error AccountInactive() =>
        Error.Forbidden("User.AccountInactive", "The account is inactive.");

    public static Error AccountSuspended() =>
        Error.Forbidden("User.AccountSuspended", "The account is suspended.");

    public static Error TypeMismatch() =>
        Error.Failure("User.TypeMismatch", "The profile type does not match the user type.");
}

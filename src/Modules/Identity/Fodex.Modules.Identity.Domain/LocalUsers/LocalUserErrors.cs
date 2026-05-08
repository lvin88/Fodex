using Fodex.Common.Domain.Errors;

namespace Fodex.Modules.Identity.Domain.LocalUsers;

/// <summary>Error constants for the <see cref="LocalUser"/> aggregate.</summary>
public static class LocalUserErrors
{
    public static readonly Error NotFound =
        Error.NotFound("LocalUser.NotFound", "Local user was not found.");

    public static readonly Error PhoneAlreadyInUse =
        Error.Conflict("LocalUser.PhoneAlreadyInUse", "A local user with this phone number already exists.");

    public static readonly Error FirstNameEmpty =
        Error.Validation("LocalUser.FirstNameEmpty", "First name cannot be empty.");

    public static readonly Error FirstNameTooLong =
        Error.Validation("LocalUser.FirstNameTooLong", $"First name cannot exceed {LocalUser.NameMaxLength} characters.");

    public static readonly Error LastNameEmpty =
        Error.Validation("LocalUser.LastNameEmpty", "Last name cannot be empty.");

    public static readonly Error LastNameTooLong =
        Error.Validation("LocalUser.LastNameTooLong", $"Last name cannot exceed {LocalUser.NameMaxLength} characters.");

    public static readonly Error FatherNameEmpty =
        Error.Validation("LocalUser.FatherNameEmpty", "Father name cannot be empty.");

    public static readonly Error FatherNameTooLong =
        Error.Validation("LocalUser.FatherNameTooLong", $"Father name cannot exceed {LocalUser.NameMaxLength} characters.");
}

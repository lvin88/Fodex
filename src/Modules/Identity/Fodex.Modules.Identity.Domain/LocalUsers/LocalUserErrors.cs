using Fodex.Common.Domain.Errors;

namespace Fodex.Modules.Identity.Domain.LocalUsers;

/// <summary>Error factory methods for the <see cref="LocalUser"/> aggregate.</summary>
public static class LocalUserErrors
{
    public static Error NotFound(LocalUserId id) =>
        Error.NotFound("LocalUser.NotFound", $"Local user '{id}' was not found.");

    public static Error PhoneAlreadyInUse(string phone) =>
        Error.Conflict("LocalUser.PhoneAlreadyInUse", $"A local user with phone number '{phone}' already exists.");

    public static Error FirstNameEmpty() =>
        Error.Validation("LocalUser.FirstNameEmpty", "First name cannot be empty.");

    public static Error FirstNameTooLong() =>
        Error.Validation("LocalUser.FirstNameTooLong", $"First name cannot exceed {LocalUser.NameMaxLength} characters.");

    public static Error LastNameEmpty() =>
        Error.Validation("LocalUser.LastNameEmpty", "Last name cannot be empty.");

    public static Error LastNameTooLong() =>
        Error.Validation("LocalUser.LastNameTooLong", $"Last name cannot exceed {LocalUser.NameMaxLength} characters.");

    public static Error FatherNameEmpty() =>
        Error.Validation("LocalUser.FatherNameEmpty", "Father name cannot be empty.");

    public static Error FatherNameTooLong() =>
        Error.Validation("LocalUser.FatherNameTooLong", $"Father name cannot exceed {LocalUser.NameMaxLength} characters.");
}

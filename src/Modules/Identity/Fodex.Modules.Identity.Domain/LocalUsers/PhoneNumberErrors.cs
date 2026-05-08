using Fodex.Common.Domain.Errors;

namespace Fodex.Modules.Identity.Domain.LocalUsers;

/// <summary>Error constants for <see cref="PhoneNumber"/> value object validation.</summary>
public static class PhoneNumberErrors
{
    public static readonly Error Empty =
        Error.Validation("PhoneNumber.Empty", "Phone number cannot be empty.");

    public static readonly Error TooLong =
        Error.Validation("PhoneNumber.TooLong", $"Phone number cannot exceed {PhoneNumber.MaxLength} characters.");

    public static readonly Error InvalidFormat =
        Error.Validation("PhoneNumber.InvalidFormat", "Phone number is not in a valid format.");
}

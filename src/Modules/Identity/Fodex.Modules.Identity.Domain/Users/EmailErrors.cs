using Fodex.Common.Domain.Errors;

namespace Fodex.Modules.Identity.Domain.Users;

/// <summary>Error factory methods for <see cref="Email"/> value object validation.</summary>
public static class EmailErrors
{
    public static Error Empty() =>
        Error.Validation("Email.Empty", "Email address cannot be empty.");

    public static Error TooLong() =>
        Error.Validation("Email.TooLong", $"Email address cannot exceed {Email.MaxLength} characters.");

    public static Error InvalidFormat() =>
        Error.Validation("Email.InvalidFormat", "Email address is not in a valid format.");
}

using Fodex.Common.Domain.Errors;

namespace Fodex.Modules.Identity.Domain.Users;

/// <summary>Error constants for <see cref="Email"/> value object validation.</summary>
public static class EmailErrors
{
    /// <summary>Input is null or whitespace.</summary>
    public static readonly Error Empty =
        Error.Validation("Email.Empty", "Email address cannot be empty.");

    /// <summary>Input exceeds <see cref="Email.MaxLength"/> characters.</summary>
    public static readonly Error TooLong =
        Error.Validation("Email.TooLong", $"Email address cannot exceed {Email.MaxLength} characters.");

    /// <summary>Input does not conform to a valid e-mail format.</summary>
    public static readonly Error InvalidFormat =
        Error.Validation("Email.InvalidFormat", "Email address is not in a valid format.");
}

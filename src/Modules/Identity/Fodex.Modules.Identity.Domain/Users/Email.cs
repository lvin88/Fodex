using System.Text.RegularExpressions;
using Fodex.Common.Domain.Abstractions;
using Fodex.Common.Domain.Errors;

namespace Fodex.Modules.Identity.Domain.Users;

/// <summary>Email address value object with format validation.</summary>
public sealed record Email : IValueObject
{
    public const int MaxLength = 254;

    private static readonly Regex EmailRegex = new(
        @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
        RegexOptions.Compiled | RegexOptions.IgnoreCase,
        TimeSpan.FromMilliseconds(100));

    public string Value { get; }

    private Email(string value)
    {
        Value = value;
    }

    public static Result<Email> Create(string? input)
    {
        if (string.IsNullOrWhiteSpace(input))
        {
            return Result.Failure<Email>(EmailErrors.Empty);
        }

        string normalized = input.Trim().ToLowerInvariant();

        if (normalized.Length > MaxLength)
        {
            return Result.Failure<Email>(EmailErrors.TooLong);
        }

        if (!EmailRegex.IsMatch(normalized))
        {
            return Result.Failure<Email>(EmailErrors.InvalidFormat);
        }

        return new Email(normalized);
    }
}

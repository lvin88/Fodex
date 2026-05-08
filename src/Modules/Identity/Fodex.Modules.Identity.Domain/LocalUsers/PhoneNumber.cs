using System.Text.RegularExpressions;
using Fodex.Common.Domain.Abstractions;
using Fodex.Common.Domain.Errors;

namespace Fodex.Modules.Identity.Domain.LocalUsers;

/// <summary>Phone number value object with basic format validation.</summary>
public sealed record PhoneNumber : IValueObject
{
    public const int MaxLength = 20;

    private static readonly Regex _phoneRegex = new(
        @"^\+?[0-9\s\-\(\)]{7,20}$",
        RegexOptions.Compiled,
        TimeSpan.FromMilliseconds(100));

    public string Value { get; }

    private PhoneNumber(string value) => Value = value;

    public static Result<PhoneNumber> Create(string? input)
    {
        if (string.IsNullOrWhiteSpace(input))
        {
            return Result.Failure<PhoneNumber>(PhoneNumberErrors.Empty);
        }

        var normalized = input.Trim();

        if (normalized.Length > MaxLength)
        {
            return Result.Failure<PhoneNumber>(PhoneNumberErrors.TooLong);
        }

        if (!_phoneRegex.IsMatch(normalized))
        {
            return Result.Failure<PhoneNumber>(PhoneNumberErrors.InvalidFormat);
        }

        return new PhoneNumber(normalized);
    }
}

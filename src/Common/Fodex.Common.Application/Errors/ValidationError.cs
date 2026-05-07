using Fodex.Common.Domain.Errors;

namespace Fodex.Common.Application.Errors;

/// <summary>
/// A specialized <see cref="Error"/> that aggregates multiple individual
/// validation failures into a single error suitable for <c>Result.Failure</c>.
/// </summary>
/// <remarks>
/// <para>
/// While most domain errors carry a single code/description pair, validation
/// of a command or query input commonly produces multiple failures at once
/// (e.g., "Email is required" + "Password too short"). Returning them as one
/// aggregated error preserves the Result pattern while allowing the API layer
/// to render a proper validation-problem response (RFC 7807).
/// </para>
/// <para>
/// Use <see>
///     <cref>FromResults</cref>
/// </see>
/// to construct a <see cref="ValidationError"/>
/// from a collection of FluentValidation <c>ValidationFailure</c>s (mapped to
/// <see cref="Error"/> in <c>ValidationBehavior</c>).
/// </para>
/// </remarks>
public sealed record ValidationError : Error
{
    /// <summary>
    /// The individual validation failures that occurred.
    /// Never empty: a <see cref="ValidationError"/> implies at least one failure.
    /// </summary>
    public IReadOnlyCollection<Error> Errors { get; }

    private ValidationError(IReadOnlyCollection<Error> errors)
        : base(
            Code: "Validation.General",
            Description: "One or more validation errors occurred.",
            Type: ErrorType.Validation)
    {
        Errors = errors;
    }

    /// <summary>
    /// Creates a <see cref="ValidationError"/> from a non-empty collection of
    /// individual validation failures.
    /// </summary>
    /// <param name="errors">The validation errors. Must contain at least one item.</param>
    /// <exception cref="ArgumentException">If <paramref name="errors"/> is empty.</exception>
    public static ValidationError FromErrors(IReadOnlyCollection<Error> errors)
    {
        ArgumentNullException.ThrowIfNull(errors);

        if (errors.Count == 0)
        {
            throw new ArgumentException(
                "A ValidationError requires at least one underlying error.",
                nameof(errors));
        }

        return new ValidationError(errors);
    }
}

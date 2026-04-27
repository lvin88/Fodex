namespace Fodex.Common.Domain.Abstractions;

/// <summary>
/// Marker interface identifying a domain value object.
/// </summary>
/// <remarks>
/// <para>
/// A value object is an immutable type whose identity is defined entirely by
/// its values, not by a reference or surrogate identifier. Two value objects
/// are equal when all their components are equal (structural equality).
/// </para>
/// <para>
/// In Fodex, value objects are implemented as <c>sealed record</c> types using
/// the smart-constructor pattern: a private constructor plus a static
/// <c>Create</c> factory that returns <c>Result&lt;TValueObject&gt;</c>.
/// This guarantees that an instance can never exist in an invalid state.
/// </para>
/// <para>
/// Example:
/// <code>
/// public sealed record Email : IValueObject
/// {
///     public string Value { get; }
///
///     private Email(string value) { Value = value; }
///
///     public static Result&lt;Email&gt; Create(string input)
///     {
///         if (string.IsNullOrWhiteSpace(input))
///             return Result.Failure&lt;Email&gt;(EmailErrors.Empty);
///         if (!IsValidFormat(input))
///             return Result.Failure&lt;Email&gt;(EmailErrors.InvalidFormat);
///
///         return new Email(input.Trim().ToLowerInvariant());
///     }
/// }
/// </code>
/// </para>
/// <para>
/// Architecture tests enforce that types implementing <see cref="IValueObject"/>
/// have no public constructors and expose only init-only or get-only members.
/// </para>
/// </remarks>
public interface IValueObject;

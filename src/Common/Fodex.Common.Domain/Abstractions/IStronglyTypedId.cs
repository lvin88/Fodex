namespace Fodex.Common.Domain.Abstractions;

/// <summary>
/// Marker interface for strongly typed identifiers.
/// </summary>
/// <typeparam name="TValue">
/// The underlying primitive value type (e.g., <see cref="Guid"/>, <see cref="long"/>, <see cref="string"/>).
/// </typeparam>
/// <remarks>
/// <para>
/// Strongly typed identifiers prevent a common class of bugs where two
/// semantically different IDs (e.g., <c>UserId</c> and <c>OrderId</c>) of the
/// same primitive type are accidentally interchanged at the call site.
/// With strongly typed IDs, <c>order.UserId = customer. Id</c> become a
/// compile-time error if <c>customer.ID</c> is a <c>CustomerId</c>.
/// </para>
/// <para>
/// Concrete IDs are typically <c>readonly record struct</c> wrappers:
/// <code>
/// public readonly record struct UserId(Guid Value): IStronglyTypedId
/// {
///     public static UserId New() =&gt; new(StronglyTypedIdHelpers.NewSequentialId());
/// }
/// </code>
/// </para>
/// <para>
/// Infrastructure adapters (EF Core converters, JSON serializers, route binders)
/// can discover and register all implementations of this interface via reflection.
/// </para>
/// </remarks>
public interface IStronglyTypedId<out TValue>
    where TValue : notnull
{
    /// <summary>
    /// The underlying primitive value of this identifier.
    /// </summary>
    public TValue Value { get; }
}

/// <summary>
/// Marker interface for strongly typed identifiers backed by a <see cref="Guid"/>.
/// This is the default identifier shape across the Fodex domain.
/// </summary>
public interface IStronglyTypedId : IStronglyTypedId<Guid>;

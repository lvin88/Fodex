namespace Fodex.Common.Domain.Primitives;

/// <summary>
/// Marker base class for aggregate roots in the Domain-Driven Design sense.
/// </summary>
/// <typeparam name="TId">
/// The strongly typed identifier of the aggregate (e.g., <c>UserId</c>, <c>OrderId</c>).
/// </typeparam>
/// <remarks>
/// <para>
/// An aggregate root is the entry point of a transactional consistency boundary.
/// All modifications to entities inside the aggregate must go through the root.
/// Repositories operate exclusively on aggregate roots; child entities are
/// loaded and persisted through their parent aggregate.
/// </para>
/// <para>
/// At the moment this class adds no behavior on top of <see cref="Entity{TId}"/>;
/// it exists primarily as a marker so that:
/// <list type="bullet">
///   <item>Repositories can constrain their generic parameters to roots only.</item>
///   <item>Architecture tests can enforce "only aggregate roots have repositories" rules.</item>
///   <item>Future cross-cutting concerns (concurrency tokens, version numbers,
///   audit metadata) have a single place to land.</item>
/// </list>
/// </para>
/// </remarks>
public abstract class AggregateRoot<TId> : Entity<TId>
    where TId : notnull
{
    /// <summary>
    /// Protected default constructor for ORMs and serializers.
    /// </summary>
    protected AggregateRoot() { }

    /// <summary>
    /// Constructs an aggregate root with the given identifier.
    /// </summary>
    /// <param name="id">The strongly typed identifier.</param>
    protected AggregateRoot(TId id) : base(id) { }
}

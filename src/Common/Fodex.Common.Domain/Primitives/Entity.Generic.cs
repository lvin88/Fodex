namespace Fodex.Common.Domain.Primitives;

/// <summary>
/// Generic base class for entities with a strongly typed identifier.
/// Equality is identifier-based: two entities are equal iff their
/// concrete types match and their <see cref="Id"/> values are equal.
/// </summary>
/// <typeparam name="TId">
/// The strongly typed identifier (e.g., <c>UserId</c>, <c>OrderId</c>).
/// Must be non-null.
/// </typeparam>
public abstract class Entity<TId> : Entity, IEquatable<Entity<TId>>
    where TId : notnull
{
    /// <summary>
    /// The unique identifier of this entity.
    /// Set by the constructor and never changes for the entity's lifetime.
    /// </summary>
    private TId? Id { get; }

    /// <summary>
    /// Protected default constructor for ORMs (e.g., EF Core) and serializers.
    /// </summary>
    protected Entity() { }

    /// <summary>
    /// Constructs an entity with the given identifier.
    /// </summary>
    /// <param name="id">The strongly typed identifier.</param>
    protected Entity(TId id)
    {
        ArgumentNullException.ThrowIfNull(id);
        Id = id;
    }

    /// <summary>Determines whether this entity equals the specified entity by identifier.</summary>
    public bool Equals(Entity<TId>? other)
    {
        if (other is null)
        {
            return false;
        }

        if (ReferenceEquals(this, other))
        {
            return true;
        }

        // Different concrete types are never equal, even if IDs match.
        return GetType() == other.GetType() && EqualityComparer<TId>.Default.Equals(Id, other.Id);
    }

    /// <inheritdoc />
    public override bool Equals(object? obj) => obj is Entity<TId> entity && Equals(entity);

    /// <inheritdoc />
    public override int GetHashCode() => HashCode.Combine(GetType(), Id);

    public static bool operator ==(Entity<TId>? left, Entity<TId>? right) =>
        left?.Equals(right) ?? right is null;

    public static bool operator !=(Entity<TId>? left, Entity<TId>? right) => !(left == right);
}

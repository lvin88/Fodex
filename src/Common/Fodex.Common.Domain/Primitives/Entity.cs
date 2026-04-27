using Fodex.Common.Domain.Abstractions;

namespace Fodex.Common.Domain.Primitives;

/// <summary>
/// Non-generic base class for all domain entities.
/// Holds the domain-event machinery so that infrastructure layers
/// (e.g., EF Core <c>SaveChanges</c> interceptors) can pick up events
/// without knowing the concrete <c>TId</c> of every entity.
/// </summary>
/// <remarks>
/// <para>
/// Do not inherit from this class directly. Inherit from
/// <see cref="Entity{TId}"/> or <see cref="AggregateRoot{TId}"/> instead.
/// </para>
/// <para>
/// Equality is intentionally not implemented here; it lives in
/// <see cref="Entity{TId}"/> where the strongly typed identifier is known.
/// </para>
/// </remarks>
public abstract class Entity
{
    private readonly List<IDomainEvent> _domainEvents = [];

    /// <summary>
    /// The domain events raised by this entity since its last persistence.
    /// Read-only to callers; entities mutate via <see cref="RaiseDomainEvent"/>.
    /// </summary>
    public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    /// <summary>
    /// Records a domain event to be dispatched after persistence.
    /// </summary>
    /// <param name="domainEvent">The event to raise.</param>
    /// <exception cref="ArgumentNullException">If <paramref name="domainEvent"/> is null.</exception>
    protected void RaiseDomainEvent(IDomainEvent domainEvent)
    {
        ArgumentNullException.ThrowIfNull(domainEvent);
        _domainEvents.Add(domainEvent);
    }

    /// <summary>
    /// Clears all pending domain events.
    /// Called by the infrastructure after successful dispatch.
    /// </summary>
    public void ClearDomainEvents() => _domainEvents.Clear();
}

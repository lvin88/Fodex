using Fodex.Common.Domain.Abstractions;

namespace Fodex.Common.Domain.Primitives;

/// <summary>
/// Base record for all domain events.
/// Provides default implementations of <see cref="IDomainEvent.EventId"/>
/// (UUIDv7 for natural ordering) and <see cref="IDomainEvent.OccurredOnUtc"/>
/// (current UTC time at construction).
/// </summary>
/// <remarks>
/// Concrete events should declare their domain-relevant data and inherit
/// from this base. Example:
/// <code>
/// public sealed record OrderPlacedDomainEvent(OrderId, decimal Total)
///     : DomainEventBase;
/// </code>
/// </remarks>
public abstract record DomainEventBase : IDomainEvent
{
    /// <summary>
    /// Unique identifier of this event instance.
    /// Generated as UUIDv7 to ensure both uniqueness and chronological ordering
    /// (essential for outbox/event log scans).
    /// </summary>
    public Guid EventId { get; init; } = Guid.CreateVersion7();

    /// <summary>
    /// UTC timestamp captured at construction time, i.e., when the aggregate
    /// state change occurred (not when the event is dispatched).
    /// </summary>
    public DateTime OccurredOnUtc { get; init; } = DateTime.UtcNow;
}

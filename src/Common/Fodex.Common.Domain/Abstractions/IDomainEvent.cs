namespace Fodex.Common.Domain.Abstractions;

/// <summary>
/// Marker interface for domain events.
/// A domain event represents a meaningful occurrence in the domain
/// (e.g., <c>UserRegistered</c>, <c>OrderPlaced</c>) and is used to
/// decouple aggregates from the side effects their changes trigger.
/// </summary>
/// <remarks>
/// <para>
/// Domain events are dispatched <i>after</i> the aggregate's transaction
/// has been successfully persisted (via an EF Core SaveChanges interceptor),
/// to ensure event handlers never observe inconsistent state.
/// </para>
/// <para>
/// This interface is intentionally pure: it has no dependency on Mediator,
/// MassTransit, or any infrastructure concern. Adapters in the application
/// layer wrap domain events into Mediator notifications when needed.
/// </para>
/// </remarks>
public interface IDomainEvent
{
    /// <summary>
    /// Unique identifier of this event instance.
    /// Used for idempotency, audit trails, and distributed tracing.
    /// </summary>
    public Guid EventId { get; }

    /// <summary>
    /// UTC timestamp when the event occurred (i.e., when the
    /// aggregate state change happened, not when it was dispatched).
    /// </summary>
    public DateTime OccurredOnUtc { get; }
}

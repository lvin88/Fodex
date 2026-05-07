namespace Fodex.Common.Application.Abstractions.Messaging;

/// <summary>
/// Marks a public contract event published by one module and consumed by others.
/// </summary>
/// <remarks>
/// <para>
/// Integration events are the public language modules use to communicate.
/// They are dispatched asynchronously via the outbox pattern: written to the
/// database in the same transaction as the originating change, then relayed
/// to the message broker (RabbitMQ) by a background processor.
/// </para>
/// <para>
/// <b>Critical rule:</b> Integration event payloads must use only primitive
/// types (Guid, string, int, decimal, DateTime, etc.). Strongly-typed IDs
/// (e.g., <c>UserId</c>) and value objects (e.g., <c>Email</c>) MUST be
/// unwrapped to their underlying primitives. Consuming modules cannot reference
/// the publisher's internal domain types.
/// </para>
/// <para>
/// Each module owns a dedicated project (e.g., <c>Fodex.Modules.Identity.IntegrationEvents</c>)
/// containing its public events. Other modules reference that project to consume.
/// </para>
/// <para>
/// Example:
/// <code>
/// // In Fodex.Modules.Identity.IntegrationEvents:
/// public sealed record UserRegisteredIntegrationEvent(
///     Guid UserId,                  // primitive, NOT UserId
///     string Email,                  // primitive, NOT Email value object
///     string FullName,
///     DateTime RegisteredAtUtc) : IIntegrationEvent
/// {
///     public Guid EventId { get; init; } = Guid.CreateVersion7();
///     public DateTime OccurredOnUtc { get; init; } = DateTime.UtcNow;
/// }
/// </code>
/// </para>
/// </remarks>
public interface IIntegrationEvent
{
    /// <summary>
    /// Unique identifier of this event instance.
    /// Used for idempotency (consumers track processed event IDs to handle
    /// at-least-once delivery), audit, and distributed tracing correlation.
    /// </summary>
    public Guid EventId { get; }

    /// <summary>
    /// UTC timestamp when the event occurred in the publishing module.
    /// Useful for chronological replay and event ordering across consumers.
    /// </summary>
    public DateTime OccurredOnUtc { get; }
}

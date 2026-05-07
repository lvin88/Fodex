using Fodex.Common.Domain.Abstractions;
using Mediator;

namespace Fodex.Common.Application.Abstractions.Events;

/// <summary>
/// Mediator notification wrapper around a domain event of type
/// <typeparamref name="TDomainEvent"/>.
/// </summary>
/// <typeparam name="TDomainEvent">The wrapped domain event type.</typeparam>
/// <remarks>
/// <para>
/// This adapter is the bridge between the pure domain layer (which has no
/// knowledge of Mediator) and the application layer's dispatch infrastructure.
/// </para>
/// <para>
/// Domain events raised by aggregates are pure data objects implementing
/// <see cref="IDomainEvent"/>. After successful persistence, an EF Core
/// SaveChanges interceptor wraps each event into a
/// <c>DomainEventNotification</c> implementation and publishes it via
/// Mediator. Handlers subscribe to the wrapped notification, not the raw event.
/// </para>
/// <para>
/// Example handler:
/// <code>
/// public sealed class WelcomeEmailDomainEventHandler
///     : INotificationHandler&lt;DomainEventNotification&lt;UserRegisteredDomainEvent&gt;&gt;
/// {
///     public async ValueTask Handle(
///         DomainEventNotification&lt;UserRegisteredDomainEvent&gt; notification,
///         CancellationToken cancellationToken)
///     {
///         var domainEvent = notification.DomainEvent;
///         // ... send welcome email using domainEvent.Email
///     }
/// }
/// </code>
/// </para>
/// </remarks>
public interface IDomainEventNotification<out TDomainEvent> : INotification
    where TDomainEvent : IDomainEvent
{
    /// <summary>
    /// The wrapped domain event.
    /// </summary>
    public TDomainEvent DomainEvent { get; }
}

using Fodex.Common.Application.Abstractions.Events;
using Fodex.Common.Domain.Abstractions;
using Mediator;

namespace Fodex.Common.Infrastructure.Events;

/// <summary>
/// Default implementation of <see cref="IDomainEventNotification{TDomainEvent}"/>.
/// A simple, immutable wrapper carrying a domain event through the Mediator pipeline.
/// </summary>
/// <typeparam name="TDomainEvent">The wrapped domain event type.</typeparam>
/// <param name="DomainEvent">The wrapped domain event.</param>
public sealed record DomainEventNotification<TDomainEvent>(TDomainEvent DomainEvent)
    : IDomainEventNotification<TDomainEvent>
    where TDomainEvent : IDomainEvent;

/// <summary>
/// Factory helpers for creating <see cref="DomainEventNotification{TDomainEvent}"/>
/// instances from a runtime <see cref="IDomainEvent"/> reference.
/// </summary>
/// <remarks>
/// <para>
/// When dispatching domain events from a SaveChanges interceptor, the events
/// are typed as <see cref="IDomainEvent"/> (the base interface), not their
/// concrete types. Reflection is needed to construct the correctly-typed
/// generic wrapper. This helper centralizes that reflection logic so the
/// interceptor stays clean.
/// </para>
/// </remarks>
public static class DomainEventNotification
{
    /// <summary>
    /// Wraps the given domain event in a <see cref="DomainEventNotification{TDomainEvent}"/>
    /// of the event's runtime type.
    /// </summary>
    /// <param name="domainEvent">The domain event to wrap.</param>
    /// <returns>An <see cref="INotification"/>-compatible wrapper.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="domainEvent"/> is null.</exception>
    public static object? Create(IDomainEvent domainEvent)
    {
        ArgumentNullException.ThrowIfNull(domainEvent);

        var notificationType = typeof(DomainEventNotification<>)
            .MakeGenericType(domainEvent.GetType());

        return Activator.CreateInstance(notificationType, domainEvent);
    }
}

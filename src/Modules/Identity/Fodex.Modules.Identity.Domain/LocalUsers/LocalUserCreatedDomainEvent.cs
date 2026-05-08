using Fodex.Common.Domain.Primitives;

namespace Fodex.Modules.Identity.Domain.LocalUsers;

/// <summary>Raised when a new <see cref="LocalUser"/> profile is created.</summary>
public sealed record LocalUserCreatedDomainEvent(LocalUserId LocalUserId) : DomainEventBase;

using Fodex.Common.Domain.Primitives;

namespace Fodex.Modules.Identity.Domain.Users;

/// <summary>Raised when a new <see cref="User"/> authentication account is created.</summary>
public sealed record UserCreatedDomainEvent(UserId UserId, UserType UserType) : DomainEventBase;

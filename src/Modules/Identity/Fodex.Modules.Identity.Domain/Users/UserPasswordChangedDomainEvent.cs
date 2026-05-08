using Fodex.Common.Domain.Primitives;

namespace Fodex.Modules.Identity.Domain.Users;

/// <summary>Raised when a <see cref="User"/>'s password is changed.</summary>
public sealed record UserPasswordChangedDomainEvent(UserId UserId) : DomainEventBase;

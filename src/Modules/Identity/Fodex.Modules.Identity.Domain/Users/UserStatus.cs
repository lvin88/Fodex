namespace Fodex.Modules.Identity.Domain.Users;

/// <summary>Lifecycle status of a <see cref="User"/> authentication account.</summary>
public enum UserStatus
{
    Active = 1,
    Inactive,
    Suspended,
}

using Fodex.Common.Domain.Errors;
using Fodex.Common.Domain.Primitives;
using Fodex.Modules.Identity.Domain.LocalUsers;

namespace Fodex.Modules.Identity.Domain.Users;

/// <summary>Authentication and authorization account aggregate.</summary>
public sealed class User : AggregateRoot<UserId>
{
    public const int NumberMaxLength = 20;
    public const int PasswordHashMaxLength = 100;

    public string Number { get; private set; } = string.Empty;
    public string PasswordHash { get; private set; } = string.Empty;
    public UserType Type { get; private set; }
    public UserStatus Status { get; private set; }
    public bool IsPasswordChangeRequired { get; private set; }

    /// <summary>Set when <see cref="UserType"/> is <see cref="UserType.LocalUser"/>.</summary>
    public LocalUserId? LocalUserId { get; private set; }

    /// <summary>Set when <see cref="UserType"/> is <see cref="UserType.Distributor"/>. Raw Guid — Distributor lives in another module.</summary>
    public Guid? DistributorId { get; private set; }

    /// <summary>Set when <see cref="UserType"/> is <see cref="UserType.Dealer"/>.</summary>
    public Guid? DealerId { get; private set; }

    /// <summary>Set when <see cref="UserType"/> is <see cref="UserType.SubDealer"/>.</summary>
    public Guid? SubDealerId { get; private set; }

    /// <summary>Set when <see cref="UserType"/> is <see cref="UserType.DistributorUser"/>.</summary>
    public Guid? DistributorUserId { get; private set; }

    /// <summary>Set when <see cref="UserType"/> is <see cref="UserType.DistributorWareHouseUser"/>.</summary>
    public Guid? DistributorWareHouseUserId { get; private set; }

    public string? MobilePasswordHash { get; private set; }

    private User() { }

    private User(
        UserId id,
        string number,
        string passwordHash,
        UserType type,
        UserStatus status) : base(id)
    {
        Number = number;
        PasswordHash = passwordHash;
        Type = type;
        Status = status;
        IsPasswordChangeRequired = true;
    }

    public static Result<User> Create(
        string number,
        string passwordHash,
        UserType type,
        UserStatus status = UserStatus.Active)
    {
        if (string.IsNullOrWhiteSpace(number))
        {
            return Result.Failure<User>(UserErrors.NumberEmpty());
        }

        if (number.Trim().Length > NumberMaxLength)
        {
            return Result.Failure<User>(UserErrors.NumberTooLong());
        }

        if (string.IsNullOrWhiteSpace(passwordHash))
        {
            return Result.Failure<User>(UserErrors.PasswordEmpty());
        }

        if (passwordHash.Length > PasswordHashMaxLength)
        {
            return Result.Failure<User>(UserErrors.PasswordHashTooLong());
        }

        var user = new User(UserId.New(), number.Trim(), passwordHash, type, status);

        user.RaiseDomainEvent(new UserCreatedDomainEvent(user.Id, type));

        return user;
    }

    public Result AssignLocalUser(LocalUserId localUserId)
    {
        if (Type != UserType.LocalUser)
        {
            return Result.Failure(UserErrors.TypeMismatch());
        }

        LocalUserId = localUserId;
        return Result.Success();
    }

    public Result ChangePassword(string newPasswordHash, bool requireChangeOnNextLogin = false)
    {
        if (string.IsNullOrWhiteSpace(newPasswordHash))
        {
            return Result.Failure(UserErrors.PasswordEmpty());
        }

        PasswordHash = newPasswordHash;
        IsPasswordChangeRequired = requireChangeOnNextLogin;

        RaiseDomainEvent(new UserPasswordChangedDomainEvent(Id));

        return Result.Success();
    }

    public void Activate() => Status = UserStatus.Active;

    public void Deactivate() => Status = UserStatus.Inactive;

    public void Suspend() => Status = UserStatus.Suspended;
}

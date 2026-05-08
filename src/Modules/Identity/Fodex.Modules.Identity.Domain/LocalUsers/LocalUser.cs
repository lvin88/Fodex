using Fodex.Common.Domain.Errors;
using Fodex.Common.Domain.Primitives;

namespace Fodex.Modules.Identity.Domain.LocalUsers;

/// <summary>Internal company employee profile aggregate (staff within the distributor organisation).</summary>
public sealed class LocalUser : AggregateRoot<LocalUserId>
{
    public const int NameMaxLength = 100;

    public string FirstName { get; private set; } = string.Empty;
    public string LastName { get; private set; } = string.Empty;
    public string FatherName { get; private set; } = string.Empty;
    public PhoneNumber PhoneNumber { get; private set; } = null!;
    public LocalUserType Type { get; private set; }
    public DateTime CreatedAtUtc { get; private set; }
    public DateTime LastModifiedAtUtc { get; private set; }

    private LocalUser() { }

    private LocalUser(
        LocalUserId id,
        string firstName,
        string lastName,
        string fatherName,
        PhoneNumber phoneNumber,
        LocalUserType type,
        DateTime createdAtUtc) : base(id)
    {
        FirstName = firstName;
        LastName = lastName;
        FatherName = fatherName;
        PhoneNumber = phoneNumber;
        Type = type;
        CreatedAtUtc = createdAtUtc;
        LastModifiedAtUtc = createdAtUtc;
    }

    public static Result<LocalUser> Create(
        string firstName,
        string lastName,
        string fatherName,
        string phoneNumber,
        LocalUserType type,
        DateTime createdAtUtc)
    {
        if (string.IsNullOrWhiteSpace(firstName))
        {
            return Result.Failure<LocalUser>(LocalUserErrors.FirstNameEmpty);
        }

        if (firstName.Trim().Length > NameMaxLength)
        {
            return Result.Failure<LocalUser>(LocalUserErrors.FirstNameTooLong);
        }

        if (string.IsNullOrWhiteSpace(lastName))
        {
            return Result.Failure<LocalUser>(LocalUserErrors.LastNameEmpty);
        }

        if (lastName.Trim().Length > NameMaxLength)
        {
            return Result.Failure<LocalUser>(LocalUserErrors.LastNameTooLong);
        }

        if (string.IsNullOrWhiteSpace(fatherName))
        {
            return Result.Failure<LocalUser>(LocalUserErrors.FatherNameEmpty);
        }

        if (fatherName.Trim().Length > NameMaxLength)
        {
            return Result.Failure<LocalUser>(LocalUserErrors.FatherNameTooLong);
        }

        var phoneResult = PhoneNumber.Create(phoneNumber);
        if (phoneResult.IsFailure)
        {
            return Result.Failure<LocalUser>(phoneResult.Error);
        }

        var localUser = new LocalUser(
            LocalUserId.New(),
            firstName.Trim(),
            lastName.Trim(),
            fatherName.Trim(),
            phoneResult.Value!,
            type,
            createdAtUtc);

        localUser.RaiseDomainEvent(new LocalUserCreatedDomainEvent(localUser.Id));

        return localUser;
    }

    public Result Update(
        string firstName,
        string lastName,
        string fatherName,
        string phoneNumber,
        LocalUserType type,
        DateTime modifiedAtUtc)
    {
        if (string.IsNullOrWhiteSpace(firstName))
        {
            return Result.Failure(LocalUserErrors.FirstNameEmpty);
        }

        if (firstName.Trim().Length > NameMaxLength)
        {
            return Result.Failure(LocalUserErrors.FirstNameTooLong);
        }

        if (string.IsNullOrWhiteSpace(lastName))
        {
            return Result.Failure(LocalUserErrors.LastNameEmpty);
        }

        if (lastName.Trim().Length > NameMaxLength)
        {
            return Result.Failure(LocalUserErrors.LastNameTooLong);
        }

        if (string.IsNullOrWhiteSpace(fatherName))
        {
            return Result.Failure(LocalUserErrors.FatherNameEmpty);
        }

        if (fatherName.Trim().Length > NameMaxLength)
        {
            return Result.Failure(LocalUserErrors.FatherNameTooLong);
        }

        var phoneResult = PhoneNumber.Create(phoneNumber);
        if (phoneResult.IsFailure)
        {
            return Result.Failure(phoneResult.Error);
        }

        FirstName = firstName.Trim();
        LastName = lastName.Trim();
        FatherName = fatherName.Trim();
        PhoneNumber = phoneResult.Value!;
        Type = type;
        LastModifiedAtUtc = modifiedAtUtc;

        return Result.Success();
    }
}

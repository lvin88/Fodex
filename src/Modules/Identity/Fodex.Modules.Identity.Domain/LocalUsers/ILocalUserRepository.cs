namespace Fodex.Modules.Identity.Domain.LocalUsers;

/// <summary>Repository contract for <see cref="LocalUser"/> aggregate persistence.</summary>
public interface ILocalUserRepository
{
    public Task<LocalUser?> GetByIdAsync(LocalUserId id, CancellationToken cancellationToken = default);
    public Task<bool> ExistsByPhoneAsync(string phoneNumber, CancellationToken cancellationToken = default);
    public void Add(LocalUser localUser);
}

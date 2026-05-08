namespace Fodex.Modules.Identity.Domain.Users;

/// <summary>Repository contract for <see cref="User"/> aggregate persistence.</summary>
public interface IUserRepository
{
    public Task<User?> GetByIdAsync(UserId id, CancellationToken cancellationToken = default);
    public Task<User?> GetByNumberAsync(string number, CancellationToken cancellationToken = default);
    public Task<bool> ExistsByNumberAsync(string number, CancellationToken cancellationToken = default);
    public void Add(User user);
}

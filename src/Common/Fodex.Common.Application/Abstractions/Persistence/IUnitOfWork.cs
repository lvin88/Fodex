namespace Fodex.Common.Application.Abstractions.Persistence;

/// <summary>
/// Coordinates the saving of one or more aggregate changes as a single atomic
/// unit of work (transaction).
/// </summary>
/// <remarks>
/// <para>
/// In a typical command handler, repositories <i>register</i> aggregate changes
/// in the underlying tracker (e.g., EF Core <c>ChangeTracker</c>), and a final
/// <see cref="SaveChangesAsync"/> call flushes them in a single transaction.
/// This guarantees that either all changes are persisted, or none are
/// (e.g., user creation and outbox message must succeed together).
/// </para>
/// <para>
/// Domain events raised during the unit of work are dispatched <i>after</i> a
/// successful save (handled by the EF Core <c>SaveChanges</c> interceptor in
/// the infrastructure layer).
/// </para>
/// </remarks>
public interface IUnitOfWork
{
    /// <summary>
    /// Persists all tracked changes as a single transaction.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The number of state entries written to the underlying store.</returns>
    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}

using Fodex.Common.Domain.Errors;
using Mediator;

namespace Fodex.Common.Application.Abstractions.Messaging;

/// <summary>
/// Handles a command that does not return a value (only success/failure).
/// </summary>
/// <typeparam name="TCommand">
/// The concrete command type. Must implement <see cref="ICommand"/>.
/// </typeparam>
/// <remarks>
/// <para>
/// Each command has exactly one handler. The handler contains the use-case
/// logic: validation, domain operations, persistence, and event raising.
/// </para>
/// <para>
/// Handlers are stateless and registered transiently in the DI container by
/// the Mediator source generator. Inject any required dependencies (repositories,
/// <see cref="Clock.IClock"/>, <see cref="Persistence.IUnitOfWork"/>) via the constructor.
/// </para>
/// <para>
/// Example:
/// <code>
/// public sealed class DeleteUserCommandHandler : ICommandHandler&lt;DeleteUserCommand&gt;
/// {
///     private readonly IUserRepository _users;
///     private readonly IUnitOfWork _uow;
///
///     public DeleteUserCommandHandler(IUserRepository users, IUnitOfWork uow)
///     {
///         _users = users;
///         _uow = uow;
///     }
///
///     public async ValueTask&lt;Result&gt; Handle(DeleteUserCommand cmd, CancellationToken ct)
///     {
///         var user = await _users.GetByIdAsync(cmd.UserId, ct);
///         if (user is null)
///             return Result.Failure(UserErrors.NotFound);
///
///         _users.Remove(user);
///         await _uow.SaveChangesAsync(ct);
///         return Result.Success();
///     }
/// }
/// </code>
/// </para>
/// </remarks>
public interface ICommandHandler<in TCommand> : IRequestHandler<TCommand, Result>
    where TCommand : ICommand;

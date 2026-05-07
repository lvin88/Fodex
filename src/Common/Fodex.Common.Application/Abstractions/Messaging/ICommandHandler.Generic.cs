using Fodex.Common.Domain.Errors;
using Mediator;

namespace Fodex.Common.Application.Abstractions.Messaging;

/// <summary>
/// Handles a command that returns a value of type <typeparamref name="TResponse"/>
/// on success.
/// </summary>
/// <typeparam name="TCommand">
/// The concrete command type. Must implement <see cref="ICommand{TResponse}"/>.
/// </typeparam>
/// <typeparam name="TResponse">The type of the value returned on success.</typeparam>
/// <remarks>
/// <para>
/// Use this handler for commands that produce a result — typically the
/// identifier of a newly created aggregate (e.g., <c>UserId</c> on registration)
/// or a generated artifact (e.g., a token, an invoice number).
/// </para>
/// <para>
/// Example:
/// <code>
/// public sealed class RegisterUserCommandHandler
///     : ICommandHandler&lt;RegisterUserCommand, UserId&gt;
/// {
///     // ... dependencies
///
///     public async ValueTask&lt;Result&lt;UserId&gt;&gt; Handle(
///         RegisterUserCommand cmd, CancellationToken ct)
///     {
///         var emailResult = Email.Create(cmd.Email);
///         if (emailResult.IsFailure)
///             return Result.Failure&lt;UserId&gt;(emailResult.Error);
///
///         var user = User.Register(emailResult.Value, cmd.FullName);
///         await _users.AddAsync(user, ct);
///         await _uow.SaveChangesAsync(ct);
///
///         return user.Id;  // implicit conversion to Result&lt;UserId&gt;
///     }
/// }
/// </code>
/// </para>
/// </remarks>
public interface ICommandHandler<in TCommand, TResponse>
    : IRequestHandler<TCommand, Result<TResponse>>
    where TCommand : ICommand<TResponse>;

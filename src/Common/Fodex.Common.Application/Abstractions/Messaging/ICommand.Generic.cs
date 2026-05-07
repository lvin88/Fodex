using Fodex.Common.Domain.Errors;
using Mediator;

namespace Fodex.Common.Application.Abstractions.Messaging;

/// <summary>
/// Marks a CQRS command that returns a value of type <typeparamref name="TResponse"/>
/// on success.
/// </summary>
/// <typeparam name="TResponse">
/// The type of the value returned on success (e.g., the newly created entity's ID).
/// </typeparam>
/// <remarks>
/// <para>
/// Use this marker when a command needs to return data — typically the identifier
/// of a newly created aggregate, or a generated token. The handler returns
/// <c>Result&lt;TResponse&gt;</c>: success carries the value, failure carries an error.
/// </para>
/// <para>
/// Example:
/// <code>
/// public sealed record RegisterUserCommand(string Email, string Password)
///     : ICommand&lt;UserId&gt;;
/// </code>
/// </para>
/// </remarks>
public interface ICommand<TResponse> : IRequest<Result<TResponse>>;

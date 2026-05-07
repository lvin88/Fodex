using Fodex.Common.Domain.Errors;
using Mediator;

namespace Fodex.Common.Application.Abstractions.Messaging;

/// <summary>
/// Marks a CQRS command that does not return a value.
/// Commands represent intent to change state (e.g., <c>DeleteUser</c>,
/// <c>ApproveInvoice</c>) and always return a <see cref="Result"/>
/// indicating success or the reason for failure.
/// </summary>
/// <remarks>
/// <para>
/// Commands are typically named with imperative verbs ending in <c>Command</c>:
/// <c>RegisterUserCommand</c>, <c>PlaceOrderCommand</c>, <c>CancelSubscriptionCommand</c>.
/// </para>
/// <para>
/// Concrete commands are usually <c>sealed record</c> types so that all input
/// is immutable once received from the API layer.
/// </para>
/// </remarks>
public interface ICommand : IRequest<Result>;

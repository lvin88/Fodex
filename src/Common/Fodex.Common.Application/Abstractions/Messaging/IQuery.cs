using Fodex.Common.Domain.Errors;
using Mediator;

namespace Fodex.Common.Application.Abstractions.Messaging;

/// <summary>
/// Marks a CQRS query that returns a value of type <typeparamref name="TResponse"/>
/// without modifying state.
/// </summary>
/// <typeparam name="TResponse">The type of the value returned by the query.</typeparam>
/// <remarks>
/// <para>
/// Queries are read-only operations that never change state. They return DTOs
/// (data transfer objects) shaped specifically for the consumer (the API, UI,
/// or another module), <i>not</i> domain aggregates.
/// </para>
/// <para>
/// Queries are typically named with descriptive nouns ending in <c>Query</c>:
/// <c>GetUserByIdQuery</c>, <c>ListActiveOrdersQuery</c>, <c>SearchDealersQuery</c>.
/// </para>
/// <para>
/// Example:
/// <code>
/// public sealed record GetUserByIdQuery(UserId UserId) : IQuery&lt;UserDto&gt;;
/// </code>
/// </para>
/// </remarks>
public interface IQuery<TResponse> : IRequest<Result<TResponse>>;

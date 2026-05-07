using Fodex.Common.Domain.Errors;
using Mediator;

namespace Fodex.Common.Application.Abstractions.Messaging;

/// <summary>
/// Handles a query that returns a value of type <typeparamref name="TResponse"/>
/// without modifying state.
/// </summary>
/// <typeparam name="TQuery">
/// The concrete query type. Must implement <see cref="IQuery{TResponse}"/>.
/// </typeparam>
/// <typeparam name="TResponse">The type of the value returned by the query.</typeparam>
/// <remarks>
/// <para>
/// Query handlers are read-only by contract. They typically project domain
/// data into shape-specific DTOs (e.g., <c>UserDto</c>, <c>OrderListItemDto</c>)
/// rather than returning aggregate roots, which protects internal domain
/// invariants from leaking into the API.
/// </para>
/// <para>
/// For performance-critical queries, handlers may bypass the ORM and use
/// raw SQL or Dapper to project directly into DTOs. The repository pattern
/// is for write paths; queries can be straightforward.
/// </para>
/// <para>
/// Example:
/// <code>
/// public sealed class GetUserByIdQueryHandler
///     : IQueryHandler&lt;GetUserByIdQuery, UserDto&gt;
/// {
///     private readonly IDbConnection _db;
///
///     public GetUserByIdQueryHandler(IDbConnection db) { _db = db; }
///
///     public async ValueTask&lt;Result&lt;UserDto&gt;&gt; Handle(
///         GetUserByIdQuery query, CancellationToken ct)
///     {
///         var user = await _db.QuerySingleOrDefaultAsync&lt;UserDto&gt;(
///             "SELECT id, email, full_name FROM identity.users WHERE id = @Id",
///             new { Id = query.UserId.Value });
///
///         return user is null
///             ? Result.Failure&lt;UserDto&gt;(UserErrors.NotFound)
///             : user;
///     }
/// }
/// </code>
/// </para>
/// </remarks>
public interface IQueryHandler<in TQuery, TResponse>
    : IRequestHandler<TQuery, Result<TResponse>>
    where TQuery : IQuery<TResponse>;

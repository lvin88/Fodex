using FluentValidation;
using FluentValidation.Results;
using Fodex.Common.Application.Errors;
using Fodex.Common.Domain.Errors;
using Mediator;
using ICommand = Fodex.Common.Application.Abstractions.Messaging.ICommand;

namespace Fodex.Common.Infrastructure.Behaviors;

/// <summary>
/// Mediator pipeline behavior that runs FluentValidation validators against
/// commands that do not return a value (<see cref="ICommand"/>), short-circuiting
/// the pipeline with a <see cref="Result.Failure"/> when validation fails.
/// </summary>
/// <typeparam name="TRequest">The command type.</typeparam>
/// <remarks>
/// <para>
/// All <see cref="IValidator{T}"/> instances registered for <typeparamref name="TRequest"/>
/// are resolved from DI and executed in parallel. If any validator reports
/// failures, the handler is never invoked; instead, a <see cref="ValidationError"/>
/// aggregating all failures is returned.
/// </para>
/// </remarks>
public sealed class ValidationBehavior<TRequest>(IEnumerable<IValidator<TRequest>> validators)
    : IPipelineBehavior<TRequest, Result>
    where TRequest : IMessage, ICommand
{
    private readonly IValidator<TRequest>[] _validators = validators.ToArray();

    public async ValueTask<Result> Handle(
        TRequest message,
        MessageHandlerDelegate<TRequest, Result> next,
        CancellationToken cancellationToken)
    {
        if (_validators.Length == 0)
        {
            return await next(message, cancellationToken);
        }

        var context = new ValidationContext<TRequest>(message);

        var results = await Task.WhenAll(
            _validators.Select(v => v.ValidateAsync(context, cancellationToken)));

        var failures = results
            .SelectMany(r => r.Errors)
            .Where(f => f is not null)
            .ToArray();

        if (failures.Length == 0)
        {
            return await next(message, cancellationToken);
        }

        return Result.Failure(ToValidationError(failures));
    }

    private static ValidationError ToValidationError(IReadOnlyCollection<ValidationFailure> failures)
    {
        var errors = failures
            .Select(f => Error.Validation(f.PropertyName, f.ErrorMessage))
            .ToArray();

        return ValidationError.FromErrors(errors);
    }
}

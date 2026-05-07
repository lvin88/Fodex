using FluentValidation;
using FluentValidation.Results;
using Fodex.Common.Application.Errors;
using Fodex.Common.Domain.Errors;
using Mediator;

namespace Fodex.Common.Infrastructure.Behaviors;

/// <summary>
/// Mediator pipeline behavior that runs FluentValidation validators against
/// requests returning <see cref="Result{TResponse}"/> (commands and queries),
/// short-circuiting the pipeline with a failure result when validation fails.
/// </summary>
/// <typeparam name="TRequest">The command or query type.</typeparam>
/// <typeparam name="TResponse">The success-value type carried by the result.</typeparam>
public sealed class ValidationBehavior<TRequest, TResponse>(
    IEnumerable<IValidator<TRequest>> validators)
    : IPipelineBehavior<TRequest, Result<TResponse>>
    where TRequest : IMessage
{
    private readonly IValidator<TRequest>[] _validators = validators.ToArray();

    public async ValueTask<Result<TResponse>> Handle(
        TRequest message,
        MessageHandlerDelegate<TRequest, Result<TResponse>> next,
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

        return Result.Failure<TResponse>(ToValidationError(failures));
    }

    private static ValidationError ToValidationError(IReadOnlyCollection<ValidationFailure> failures)
    {
        var errors = failures
            .Select(f => Error.Validation(f.PropertyName, f.ErrorMessage))
            .ToArray();

        return ValidationError.FromErrors(errors);
    }
}

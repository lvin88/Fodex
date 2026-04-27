namespace Fodex.Common.Domain.Errors;

/// <summary>
/// Represents the outcome of an operation that does not return a value.
/// Either succeeds (<see cref="IsSuccess"/> = true) or fails with an <see cref="Error"/>.
/// </summary>
public class Result
{
    /// <summary>True if the operation succeeded.</summary>
    public bool IsSuccess { get; }

    /// <summary>True if the operation failed.</summary>
    public bool IsFailure => !IsSuccess;

    /// <summary>
    /// The error describing why the operation failed.
    /// Equal to <see cref="Error.None"/> on success.
    /// </summary>
    // ReSharper disable once UnusedAutoPropertyAccessor.Global
    public Error Error { get; }

    protected Result(bool isSuccess, Error error)
    {
        switch (isSuccess)
        {
            // Defensive checks: a result must be internally consistent.
            case true when error != Error.None:
                throw new InvalidOperationException(
                    "A successful result cannot carry an error.");
            case false when error == Error.None:
                throw new InvalidOperationException(
                    "A failed result must carry an error.");
            default:
                IsSuccess = isSuccess;
                Error = error;
                break;
        }
    }

    /// <summary>Creates a successful result.</summary>
    public static Result Success() => new(true, Error.None);

    /// <summary>Creates a failed result with the given error.</summary>
    public static Result Failure(Error error) => new(false, error);

    /// <summary>Creates a successful generic result carrying a value.</summary>
    protected static Result<TValue> Success<TValue>(TValue value) =>
        new(value, true, Error.None);

    /// <summary>Creates a failed generic result with the given error.</summary>
    public static Result<TValue> Failure<TValue>(Error error) =>
        new(default, false, error);
}

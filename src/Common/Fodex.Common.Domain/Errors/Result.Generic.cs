namespace Fodex.Common.Domain.Errors;

/// <summary>
/// Represents the outcome of an operation that returns a value of type <typeparamref name="TValue"/>.
/// On success the <see cref="Value"/> is available; on failure only <see cref="Result.Error"/> is populated.
/// </summary>
/// <typeparam name="TValue">The type of the value returned on success.</typeparam>
public class Result<TValue> : Result
{
    private readonly TValue? _value;

    protected internal Result(TValue? value, bool isSuccess, Error error)
        : base(isSuccess, error)
    {
        _value = value;
    }

    /// <summary>
    /// The value returned by the operation. Only valid when <see cref="Result.IsSuccess"/> is true.
    /// Accessing this on a failed result throws an <see cref="InvalidOperationException"/>.
    /// </summary>
    public TValue? Value => IsSuccess
        ? _value
        : throw new InvalidOperationException(
            "Cannot access the value of a failed result. Check IsSuccess first.");

    /// <summary>
    /// Implicit conversion from a value to a successful result.
    /// Allows: <c>Result&lt;User&gt; result = user;</c>
    /// </summary>
    public static implicit operator Result<TValue>(TValue value) =>
        Success(value);
}

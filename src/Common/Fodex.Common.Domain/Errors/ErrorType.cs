namespace Fodex.Common.Domain.Errors;

public enum ErrorType
{
    /// <summary>Generic domain or business failure.</summary>
    Failure = 0,

    /// <summary>The requested resource was not found.</summary>
    NotFound = 1,

    /// <summary>Input validation failed (bad request data).</summary>
    Validation = 2,

    /// <summary>The operation conflicts with the current state (e.g., duplicate key).</summary>
    Conflict = 3,

    /// <summary>The user is not authenticated.</summary>
    Unauthorized = 4,

    /// <summary>The user is authenticated but lacks permission.</summary>
    Forbidden = 5
}

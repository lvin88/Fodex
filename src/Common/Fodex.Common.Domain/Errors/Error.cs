namespace Fodex.Common.Domain.Errors;

/// <summary>
/// Represents a domain or application error with a code, human-readable description,
/// and a category (<see cref="ErrorType"/>) used for transport-layer mapping.
/// </summary>
/// <param name="Code">
/// Stable, programmatic identifier in the form "Module.ErrorName"
/// (e.g., "User.NotFound", "Order.InsufficientStock"). Used in tests and clients.
/// </param>
/// <param name="Description">Human-readable description (English by default).</param>
/// <param name="Type">Error category for transport mapping.</param>
public sealed record Error(string Code, string Description, ErrorType Type)
{
    /// <summary>
    /// Sentinel value representing the absence of an error.
    /// Used internally by <c>Result</c> when the operation succeeds.
    /// </summary>
    public static readonly Error None = new(
        Code: string.Empty,
        Description: string.Empty,
        Type: ErrorType.Failure);

    /// <summary>Creates a generic failure error.</summary>
    public static Error Failure(string code, string description) =>
        new(code, description, ErrorType.Failure);

    /// <summary>Creates a "not found" error (maps to HTTP 404).</summary>
    public static Error NotFound(string code, string description) =>
        new(code, description, ErrorType.NotFound);

    /// <summary>Creates a validation error (maps to HTTP 400).</summary>
    public static Error Validation(string code, string description) =>
        new(code, description, ErrorType.Validation);

    /// <summary>Creates a conflict error (maps to HTTP 409).</summary>
    public static Error Conflict(string code, string description) =>
        new(code, description, ErrorType.Conflict);

    /// <summary>Creates an unauthorized error (maps to HTTP 401).</summary>
    public static Error Unauthorized(string code, string description) =>
        new(code, description, ErrorType.Unauthorized);

    /// <summary>Creates a forbidden error (maps to HTTP 403).</summary>
    public static Error Forbidden(string code, string description) =>
        new(code, description, ErrorType.Forbidden);
}

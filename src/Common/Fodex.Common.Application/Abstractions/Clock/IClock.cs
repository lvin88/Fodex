namespace Fodex.Common.Application.Abstractions.Clock;

/// <summary>
/// Provides the current UTC date and time.
/// Abstracted to enable deterministic unit tests
/// (production uses a system clock; tests inject a fake).
/// </summary>
/// <remarks>
/// <para>
/// Application and domain code should never read <see cref="DateTime.UtcNow"/>
/// directly. Always inject <see cref="IClock"/> instead. This makes time-dependent
/// behavior (token expiry, scheduled jobs, audit timestamps) trivially testable.
/// </para>
/// <para>
/// Always returns UTC. Local-time formatting is a presentation concern handled
/// by the API/UI layer.
/// </para>
/// </remarks>
public interface IClock
{
    /// <summary>
    /// The current UTC instant.
    /// </summary>
    public DateTime UtcNow { get; }
}

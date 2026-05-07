using Fodex.Common.Application.Abstractions.Clock;

namespace Fodex.Common.Infrastructure.Clock;

/// <summary>
/// Production implementation of <see cref="IClock"/> backed by the operating
/// system's UTC clock.
/// </summary>
/// <remarks>
/// <para>
/// Registered as a singleton in DI: there is no per-request state and
/// <see cref="DateTime.UtcNow"/> is thread-safe. Tests inject a fake
/// implementation instead of this one.
/// </para>
/// </remarks>
public sealed class SystemClock : IClock
{
    /// <inheritdoc />
    public DateTime UtcNow => DateTime.UtcNow;
}

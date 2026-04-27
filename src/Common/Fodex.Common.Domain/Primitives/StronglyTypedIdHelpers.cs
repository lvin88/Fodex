namespace Fodex.Common.Domain.Primitives;

/// <summary>
/// Helpers for generating strongly typed identifier values.
/// Centralizes ID generation strategy so that future changes
/// (e.g., custom epoch, counter-based IDs) require a single edit.
/// </summary>
public static class StronglyTypedIdHelpers
{
    /// <summary>
    /// Creates a new sequential <see cref="Guid"/> (UUIDv7).
    /// </summary>
    /// <remarks>
    /// <para>
    /// UUIDv7 embeds a Unix-millisecond timestamp in the leading 48 bits,
    /// producing identifiers that are both globally unique and monotonically
    /// increasing within a process. This is critical for B-tree indexes
    /// (PostgreSQL, SQL Server) where random GUIDs cause page splits and
    /// fragmentation, hurting insert performance and bloating storage.
    /// </para>
    /// <para>
    /// Unlike auto-increment integers, UUIDv7 can be generated client-side
    /// without a database round-trip and is safe in distributed/multi-region
    /// deployments.
    /// </para>
    /// </remarks>
    /// <returns>A new UUIDv7 value.</returns>
    public static Guid NewSequentialId() => Guid.CreateVersion7();
}

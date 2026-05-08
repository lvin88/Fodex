using Fodex.Common.Domain.Abstractions;
using Fodex.Common.Domain.Primitives;

namespace Fodex.Modules.Identity.Domain.Users;

/// <summary>Strongly typed identifier for the <see cref="User"/> aggregate.</summary>
public readonly record struct UserId(Guid Value) : IStronglyTypedId
{
    /// <summary>Creates a new <see cref="UserId"/> backed by a UUIDv7.</summary>
    public static UserId New() => new(StronglyTypedIdHelpers.NewSequentialId());

    /// <inheritdoc />
    public override string ToString() => Value.ToString();
}

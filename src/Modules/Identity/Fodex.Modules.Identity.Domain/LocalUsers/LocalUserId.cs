using Fodex.Common.Domain.Abstractions;
using Fodex.Common.Domain.Primitives;

namespace Fodex.Modules.Identity.Domain.LocalUsers;

/// <summary>Strongly typed identifier for the <see cref="LocalUser"/> aggregate.</summary>
public readonly record struct LocalUserId(Guid Value) : IStronglyTypedId
{
    /// <summary>Creates a new <see cref="LocalUserId"/> backed by a UUIDv7.</summary>
    public static LocalUserId New() => new(StronglyTypedIdHelpers.NewSequentialId());

    /// <inheritdoc />
    public override string ToString() => Value.ToString();
}

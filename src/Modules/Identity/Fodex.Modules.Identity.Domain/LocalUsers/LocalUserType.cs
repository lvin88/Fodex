namespace Fodex.Modules.Identity.Domain.LocalUsers;

/// <summary>Classifies an internal <see cref="LocalUser"/> by their operational role.</summary>
public enum LocalUserType
{
    Root = 1,
    ItOperation,
    SalesOperation,
    BusinessSupport,
    CallCenter,
    WareHouse,
    Curator,
}

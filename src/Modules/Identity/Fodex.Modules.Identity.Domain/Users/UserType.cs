namespace Fodex.Modules.Identity.Domain.Users;

/// <summary>Classifies a <see cref="User"/> by their organizational role.</summary>
public enum UserType
{
    /// <summary>Internal company user; may hold administrative privileges.</summary>
    LocalUser = 1,

    /// <summary>Top-level distributor principal account.</summary>
    Distributor,

    /// <summary>Dealer account under a distributor.</summary>
    Dealer,

    /// <summary>Sub-dealer account under a dealer.</summary>
    SubDealer,

    /// <summary>Staff member operating within a distributor's organization.</summary>
    DistributorUser,

    /// <summary>Warehouse operator within a distributor's organization.</summary>
    DistributorWareHouseUser,
}

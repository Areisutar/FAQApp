namespace src.Models.SeedDatas;

public static class ApplicationRoleSeedData
{
    public const string AdminRoleId = "79a68294-7d70-4ed6-b153-66b7e6dcad50";
    public const string PublicRoleId = "a866201e-95db-46c5-9c48-f61b65ebd43b";

    public static ApplicationRole[] GetData() =>
    [
        new ApplicationRole
        {
            Id = AdminRoleId,
            Name = "Admin",
            NormalizedName = "ADMIN",
            ConcurrencyStamp = "626edaf5-52c7-4d5a-ae52-67288ac88c22",
        },
        new ApplicationRole
        {
            Id = PublicRoleId,
            Name = "Public",
            NormalizedName = "PUBLIC",
            ConcurrencyStamp = "8f98ae83-c50c-4fa4-a2e7-1f3b194522ab",
        },
    ];
}

namespace src.Models.SeedDatas;

public static class ApplicationUserRoleSeedData
{
    public static ApplicationUserRole[] GetData() =>
    [
        new ApplicationUserRole
        {
            UserId = ApplicationUserSeedData.AdminUserId,
            RoleId = ApplicationRoleSeedData.AdminRoleId,
        },
        new ApplicationUserRole
        {
            UserId = ApplicationUserSeedData.PublicUserId,
            RoleId = ApplicationRoleSeedData.PublicRoleId,
        },
    ];
}

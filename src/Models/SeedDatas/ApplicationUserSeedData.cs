namespace src.Models.SeedDatas;

// 目視確認用アカウント。パスワードはそれぞれ Admin123! / Public123! です。
public static class ApplicationUserSeedData
{
    public const string AdminUserId = "ad9bb655-720e-477e-b7a5-cb8dc3a7dc2d";
    public const string PublicUserId = "6e690231-d710-45a0-bc37-ea51a6e108a9";

    // Identity V3 形式のハッシュを事前生成しています。
    // HasData 内で毎回ハッシュや GUID を生成すると、モデルに差分が出るため固定値を使います。
    public static ApplicationUser[] GetData() =>
    [
        new ApplicationUser
        {
            Id = AdminUserId,
            Name = "全体管理者",
            UserName = "admin@example.com",
            NormalizedUserName = "ADMIN@EXAMPLE.COM",
            Email = "admin@example.com",
            NormalizedEmail = "ADMIN@EXAMPLE.COM",
            EmailConfirmed = true,
            PasswordHash = "AQAAAAIAAYagAAAAELMa5Q7gWV1bKUHS38wrenU5clMlGZ5ggzBNhtcSdyhcEyPmoQzEWnceLGscaAG2ew==",
            SecurityStamp = "969821ba-29b6-4767-9d64-552ece616b16",
            ConcurrencyStamp = "5aa4c359-7ca1-4575-b573-cf5c2e35d37c",
            LockoutEnabled = true,
        },
        new ApplicationUser
        {
            Id = PublicUserId,
            Name = string.Empty,
            UserName = "public@example.com",
            NormalizedUserName = "PUBLIC@EXAMPLE.COM",
            Email = "public@example.com",
            NormalizedEmail = "PUBLIC@EXAMPLE.COM",
            EmailConfirmed = true,
            PasswordHash = "AQAAAAIAAYagAAAAEHC70Ils7gTpKze7jnZKO6K109VyYE4qLZMb18aClPbSW4N1Vlu7bkml93DcT2sx0g==",
            SecurityStamp = "e47200f6-2a89-4821-a949-05e6798513b3",
            ConcurrencyStamp = "0cbe2fc1-b1dc-4e3d-9d32-14fb5edfdcf6",
            LockoutEnabled = true,
        },
    ];
}

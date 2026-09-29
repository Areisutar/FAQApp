using Microsoft.AspNetCore.DataProtection.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using src.Models;
using src.Models.SeedDatas;

namespace src.Data;

// Identity と Data Protection のテーブルは DefaultConnection の DB で管理します。
public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string,
    IdentityUserClaim<string>, ApplicationUserRole, IdentityUserLogin<string>,
    IdentityRoleClaim<string>, IdentityUserToken<string>>, IDataProtectionKeyContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
    public DbSet<TestModel> TestModel { get; set; }
    public DbSet<FormModel> FormModel { get; set; }
    public DbSet<DataProtectionKey> DataProtectionKeys { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<ApplicationRole>().HasData(ApplicationRoleSeedData.GetData());
        builder.Entity<ApplicationUser>().HasData(ApplicationUserSeedData.GetData());
        builder.Entity<ApplicationUserRole>().HasData(ApplicationUserRoleSeedData.GetData());
    }
}
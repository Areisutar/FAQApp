using FAQApp.Supabase.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace FAQApp.Supabase.Data;

public sealed class SupabaseDbContext(DbContextOptions<SupabaseDbContext> options)
    : DbContext(options)
{
    public DbSet<TestModel> TestModel { get; set; }

    // Supabase が管理する auth / storage などの内部テーブルは対象にしません。
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.HasDefaultSchema("public");
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(SupabaseDbContext).Assembly);
    }
}

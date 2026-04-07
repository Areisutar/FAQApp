using Microsoft.EntityFrameworkCore;
using src.Models;

namespace src.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
    public DbSet<TestModel> TestModel { get; set; }
    // public DbSet<YourEntity> YourEntities { get; set; }
}
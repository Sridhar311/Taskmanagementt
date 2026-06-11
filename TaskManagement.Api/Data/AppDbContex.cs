using Microsoft.EntityFrameworkCore;
using TaskManagement.Api.Entities;

namespace TaskManagement.Api.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();
    public DbSet<TaskItem> Tasks => Set<TaskItem>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<TaskItem>()
            .Property(x => x.Status)
            .HasConversion<int>();

        modelBuilder.Entity<TaskItem>()
            .Property(x => x.Priority)
            .HasConversion<int>();
    }
}
using Microsoft.EntityFrameworkCore;
using MyTasks.Core.Entities;

namespace MyTasks.Infrastructure;

public class TasksDbContext : DbContext
{
    public TasksDbContext(DbContextOptions<TasksDbContext> options) : base(options) { }
    
    public DbSet<TaskItem> TaskItems { get; set; } = null!;
    
    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<TaskItem>(eb =>
        {
            eb.HasKey(t => t.Id);
            eb.Property(t => t.Title).IsRequired().HasMaxLength(200);
            eb.Property(t => t.CreatedAt).IsRequired();
        });
    }
}
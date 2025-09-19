using Microsoft.EntityFrameworkCore;
using MyTasks.Core.Entities;
using MyTasks.Core.Interfaces;
namespace MyTasks.Infrastructure.Repositories;

public class EfTaskRepository : ITaskRepository
{
    private readonly TasksDbContext _db;
    
    public EfTaskRepository(TasksDbContext db) { _db = db; }

    public async Task<IEnumerable<TaskItem>> GetAllAsync()
    {
        return await _db.TaskItems.ToListAsync();
    }

    public async Task<TaskItem?> GetByIdAsync(Guid id)
    {
        return await _db.TaskItems.FindAsync(id);
    }

    public async Task AddAsync(TaskItem item)
    {
        await _db.TaskItems.AddAsync(item);
        await _db.SaveChangesAsync();
    }

    public async Task UpdateAsync(TaskItem item)
    {
        _db.TaskItems.Update(item);
        await _db.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var task = await _db.TaskItems.FindAsync(id);
        if (task != null)
        {
            _db.TaskItems.Remove(task);
            await _db.SaveChangesAsync();
        }
    }
}
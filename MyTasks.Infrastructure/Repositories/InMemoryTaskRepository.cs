using MyTasks.Core.Entities;
using MyTasks.Core.Interfaces;
namespace MyTasks.Infrastructure.Repositories;

public class InMemoryTaskRepository : ITaskRepository
{
    private readonly List<TaskItem> _tasks = new();

    public Task<IEnumerable<TaskItem>> GetAllAsync()
    {
        return Task.FromResult<IEnumerable<TaskItem>>(_tasks);
    }

    public Task<TaskItem?> GetByIdAsync(Guid id)
    {
        return Task.FromResult(_tasks.FirstOrDefault(t => t.Id == id));
    }

    public Task AddAsync(TaskItem task)
    {
        _tasks.Add(task);
        return Task.CompletedTask;
    }

    public Task UpdateAsync(TaskItem task)
    {
        var index = _tasks.FindIndex(t => t.Id == task.Id);
        if (index != -1)
        {
            _tasks[index] = task;
        }
        /*if (_tasks.Any(t=>t.Id == id))
        {
            _tasks[id] = task;
        }*/
        return Task.CompletedTask;
    }

    public Task DeleteAsync(Guid id)
    {
        var index = _tasks.FindIndex(t => t.Id == id);
        if (index != -1)
        {
            _tasks.RemoveAt(index);
        }
        return Task.CompletedTask;
    }
}
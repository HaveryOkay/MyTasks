using Microsoft.AspNetCore.Mvc;
using MyTasks.Api.Models;
using MyTasks.Core.Entities;
using MyTasks.Core.Interfaces;

namespace MyTasks.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TaskController : ControllerBase
{
    private readonly ITaskRepository _repository;

    public TaskController(ITaskRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<TaskItem>>> GetAll()
    {
        var tasks = await _repository.GetAllAsync();
        return Ok(tasks);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TaskItem>> GetById(Guid id)
    {
        var task = await _repository.GetByIdAsync(id);
        if (task == null)
        {
            return NotFound();
        }
        return Ok(task);
    }

    [HttpPost]
    public async Task<ActionResult<TaskItem>> Create(CreateTaskDto dto)
    {
        var task = new TaskItem
        {
            Title = dto.Title,
            Description = dto.Description,
            IsCompleted = dto.IsCompleted,
            CreatedAt = DateTime.UtcNow // 系统生成
            // Id 会自动生成，因为 TaskItem 的属性默认就是 Guid.NewGuid()
        };

        await _repository.AddAsync(task);
        return CreatedAtAction(nameof(GetById), new { id = task.Id }, task);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, UpdateTaskDto dto)
    {
        var task = await _repository.GetByIdAsync(id);
        if (task == null)
        {
            return NotFound();
        }

        // 更新可修改的字段
        task.Title = dto.Title;
        task.Description = dto.Description;
        task.IsCompleted = dto.IsCompleted;

        await _repository.UpdateAsync(task);
        return NoContent(); // 204 成功但没有返回体
    }

    /*public async Task<ActionResult> UpdateAsync(Guid id,TaskItem task)
    {
        var task1 = await _repository.GetByIdAsync(id);
        if (task1 == null)
        {
            return NotFound();
        }
        else if (task.Id == id)
        {
            await _repository.UpdateAsync(task);
        }
        return NoContent();
    }*/
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAsync(Guid id)
    {
        var task = await _repository.GetByIdAsync(id);
        if (task == null)
        {
            return NotFound();
        }
        await _repository.DeleteAsync(id);
        return NoContent();
    }
}
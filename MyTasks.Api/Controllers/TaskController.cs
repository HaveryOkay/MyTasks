using Microsoft.AspNetCore.Mvc;
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
    public async Task<ActionResult<TaskItem>> CreateAsync(TaskItem task)
    {
        await _repository.AddAsync(task);
        return CreatedAtAction(nameof(GetById), new { id = task.Id }, task);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAsync(Guid id, TaskItem task)
    {
        var existingTask = await _repository.GetByIdAsync(id);
        if (existingTask == null)
        {
            return NotFound();
        }

        // 确保任务的 Id 和路由参数一致
        task.Id = id;

        await _repository.UpdateAsync(task);
        return NoContent(); // 204 状态码，表示成功但无返回内容
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
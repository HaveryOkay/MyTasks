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

    [HttpPut]
    public async Task<ActionResult<TaskItem>> UpdateAsync(Guid id,TaskItem task)
    {
        var task1 = _repository.GetByIdAsync(id);
        if (task1 == null)
        {
            return NotFound();
        }
        else if (task.Id == id)
        {
            await _repository.UpdateAsync(task);
        }

        return NoContent();
    }
}
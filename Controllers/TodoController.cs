using Microsoft.AspNetCore.Mvc;
using TodoAPI.Models;
using TodoAPI.Repositories;

namespace TodoAPI.Controllers;

[ApiController]
[Route("api/todos")]
public class TodoController(IRepositoryBase<TodoModel> repositoryBase, IDbPersistance dbPersistance) : ControllerBase
{
    private readonly IRepositoryBase<TodoModel> _todoRepository = repositoryBase;
    private readonly IDbPersistance _persistance = dbPersistance;

    [HttpGet("{id}")]
    public async Task<IActionResult> FindById(string id)
    {
        try
        {
            var todo = await _todoRepository.FindByIdAsync(Guid.Parse(id));
            if (todo is null) return NotFound(new { message = $"todos with ID {id} not found" });
            return Ok(todo);
        }
        catch (Exception)
        {
            return new StatusCodeResult(500);
        }
    }

    [HttpGet]
    public async Task<IActionResult> FindAll()
    {
        try
        {
            var todos = await _todoRepository.FindAllAsync();
            return Ok(todos);
        }
        catch (Exception)
        {
            return new StatusCodeResult(500);
        }
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] TodoModel body)
    {
        try
        {
            TodoModel payload = new()
            {
                Task = body.Task,
                Status = body.Status,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            var todo = await _todoRepository.SaveAsync(payload);
            await _persistance.SaveChangesAsync();
            return Created("api/todos", todo);
        }
        catch (Exception)
        {
            return new StatusCodeResult(500);
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(string id, [FromBody] TodoModel body)
    {
        try
        {
            var existingTodo = await _todoRepository.FindByIdAsync(Guid.Parse(id));
            if (existingTodo is null) return NotFound(new { message = $"todos with ID {id} not found" });

            existingTodo.Task = !string.IsNullOrEmpty(body.Task) ? body.Task : existingTodo.Task;
            existingTodo.Status = body.Status.HasValue ? body.Status : existingTodo.Status;
            existingTodo.UpdatedAt = DateTime.Now;

            var todo = _todoRepository.Update(existingTodo);
            await _persistance.SaveChangesAsync();

            return Ok(todo);
        }
        catch (Exception)
        {
            return new StatusCodeResult(500);
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        try
        {
            var existingTodo = await _todoRepository.FindByIdAsync(Guid.Parse(id));
            if (existingTodo is null) return NotFound(new { message = $"todos with ID {id} not found" });

            _todoRepository.Delete(existingTodo);
            await _persistance.SaveChangesAsync();

            return NoContent();
        }
        catch (Exception)
        {
            return new StatusCodeResult(500);
        }
    }
}
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoWithControllers.Data;
using TodoWithControllers.Models;

namespace TodoWithControllers.Controllers;

[ApiController]
[Route("/api/todos")]
public class TodosController : ControllerBase
{
    private readonly TodoDbContext _db;
    public TodosController(TodoDbContext db) => _db = db ?? throw new ArgumentNullException(nameof(db));

    [HttpGet]
    public async Task<ActionResult<List<Todo>>> GetAll()
    {
        return await _db.Todos.ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Todo>> Get(int id)
    {
        var todo = await _db.Todos.FindAsync(id);
        if (todo is null)
        {
            return NotFound();
        }

        return todo;
    }

    [HttpPost]
    public async Task<ActionResult<Todo>> Post(Todo todo)
    {
        await _db.Todos.AddAsync(todo);
        await _db.SaveChangesAsync();

        return CreatedAtAction(nameof(Get), new { id = todo.Id }, todo);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<Todo>> Update(int id, Todo todo)
    {
        if(id != todo.Id)
        {
            return BadRequest();
        }

        if(!await _db.Todos.AnyAsync(t => t.Id == id))
        {
            return NotFound();
        }

        _db.Todos.Update(todo);
        await _db.SaveChangesAsync();

        return Ok();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<Todo>> Delete(int id)
    {
        if(await _db.Todos.FindAsync(id) is null)
        {
            return NotFound();
        }
        
        _db.Remove(id);
        await _db.SaveChangesAsync();

        return Ok();
    }
}
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddDbContext<TodoDbContext>(o => o.UseInMemoryDatabase("Todo"));
builder.Services.AddSwaggerGen(c => 
{
    c.SwaggerDoc("v1", new() {Title = builder.Environment.ApplicationName, Version = "v1"});
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", $"{builder.Environment.ApplicationName} v1"));
}

app.MapGet("/todos", async (TodoDbContext db) =>

{
    return await db.Todos.ToListAsync();
});

app.MapGet("/todos/{id}", async (TodoDbContext db, int id) =>
{
    return await db.Todos.FindAsync(id) is Todo todo ? Results.Ok(todo) : Results.NotFound();
});

app.MapPost("/todos", async (TodoDbContext db, Todo todo) =>
{
    await db.Todos.AddAsync(todo);
    await db.SaveChangesAsync();

    return Results.Created($"/todos/{todo.Id}", todo);
});

app.MapPut("/todos/{id}", async (TodoDbContext db, Todo todo, int id) =>
{
    if(todo.Id != id)
    {
        return Results.BadRequest();
    }

    if(!await db.Todos.AnyAsync(t => t.Id == id))
    {
        return Results.NotFound();
    }

    db.Todos.Update(todo);
    await db.SaveChangesAsync();

    return Results.Ok();
});

app.MapDelete("/todos/{id}", async (TodoDbContext db, int id) => {
    var todo = await db.Todos.FindAsync(id);
    if(todo is null)
    {
        return Results.NotFound();
    }

    db.Todos.Remove(todo);
    await db.SaveChangesAsync();

    return Results.Ok();
});

app.Run();

public class TodoDbContext : DbContext
{
    public TodoDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<Todo> Todos => Set<Todo>();
}
public class Todo
{
    public int Id { get; set; }
    [Required]
    public string? Title { get; set; }
    public bool IsComplete { get; set; }
}

using Microsoft.EntityFrameworkCore;
using TodoWithControllers.Models;

namespace TodoWithControllers.Data;

public class TodoDbContext : DbContext
{
    public TodoDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<Todo> Todos => Set<Todo>();
}
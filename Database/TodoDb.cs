using Microsoft.EntityFrameworkCore;
using MinimalApiTodoApi.Models;


namespace MinimalApiTodoApi.Database;

public class TodoDb : DbContext
{
    public TodoDb(DbContextOptions<TodoDb> options)
        : base(options) { }

    public DbSet<Todo> Todos => Set<Todo>();
}
using Microsoft.EntityFrameworkCore;
using MinimalApiTodoApi.Models;


namespace MinimalApiTodoApi.Database;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options) { }

    public DbSet<Todo> Todos { get; set; } = default!;
    // public DbSet<User> Users {get;set;} = default!;
}
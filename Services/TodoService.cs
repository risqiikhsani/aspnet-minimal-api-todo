using Microsoft.EntityFrameworkCore;
using MinimalApiTodoApi.Models;
using MinimalApiTodoApi.Database;


namespace MinimalApiTodoApi.Services;

public class TodoService : ITodoService
{
    private readonly AppDbContext _db;

    public TodoService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<IEnumerable<TodoItemDTO>> GetAllTodosAsync()
    {
        return await _db.Todos.Select(x => new TodoItemDTO(x)).ToArrayAsync();
    }

    public async Task<IEnumerable<TodoItemDTO>> GetCompleteTodosAsync()
    {
        return await _db.Todos.Where(t => t.IsComplete).Select(x => new TodoItemDTO(x)).ToListAsync();
    }

    public async Task<TodoItemDTO?> GetTodoByIdAsync(int id)
    {
        var todo = await _db.Todos.FindAsync(id);
        return todo == null ? null : new TodoItemDTO(todo);
    }

    public async Task<TodoItemDTO> CreateTodoAsync(TodoItemDTO todoItemDto)
    {
        var todoItem = new Todo
        {
            Name = todoItemDto.Name,
            IsComplete = todoItemDto.IsComplete
        };

        _db.Todos.Add(todoItem);
        await _db.SaveChangesAsync();

        return new TodoItemDTO(todoItem);
    }

    public async Task<bool> UpdateTodoAsync(int id, TodoItemDTO todoItemDto)
    {
        var todo = await _db.Todos.FindAsync(id);

        if (todo == null)
        {
            return false;
        }

        todo.Name = todoItemDto.Name;
        todo.IsComplete = todoItemDto.IsComplete;

        await _db.SaveChangesAsync();

        return true;
    }

    public async Task<bool> DeleteTodoAsync(int id)
    {
        var todo = await _db.Todos.FindAsync(id);

        if (todo == null)
        {
            return false;
        }

        _db.Todos.Remove(todo);
        await _db.SaveChangesAsync();

        return true;
    }
}

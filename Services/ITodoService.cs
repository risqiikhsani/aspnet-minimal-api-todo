public interface ITodoService
{
    Task<IEnumerable<TodoItemDTO>> GetAllTodosAsync();
    Task<IEnumerable<TodoItemDTO>> GetCompleteTodosAsync();
    Task<TodoItemDTO?> GetTodoByIdAsync(int id);
    Task<TodoItemDTO> CreateTodoAsync(TodoItemDTO todoItemDTO);
    Task<bool> UpdateTodoAsync(int id, TodoItemDTO todoItemDTO);
    Task<bool> DeleteTodoAsync(int id);
}

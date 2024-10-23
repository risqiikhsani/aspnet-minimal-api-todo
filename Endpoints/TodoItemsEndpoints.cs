using MinimalApiTodoApi.Models;

namespace MinimalApiTodoApi.Endpoints;

public static class TodoItemsEndpoints
{
    public static void RegisterTodoItemsEndpoints(this WebApplication app)
    {
        RouteGroupBuilder todoItems = app.MapGroup("/todo-items");
        todoItems.MapGet("/",
            async (ITodoService todoService) => { return TypedResults.Ok(await todoService.GetAllTodosAsync()); });
        todoItems.MapGet("/complete",
            async (ITodoService todoService) => { return TypedResults.Ok(await todoService.GetCompleteTodosAsync()); }
        );
        todoItems.MapGet("/{id}",
            async (int id, ITodoService todoService) =>
            {
                return TypedResults.Ok(await todoService.GetTodoByIdAsync(id));
            }
        );
        todoItems.MapPost("/", async (TodoItemDTO todoItemDTO, ITodoService todoService) =>
        {
            var createdTodo = await todoService.CreateTodoAsync(todoItemDTO);
            return TypedResults.Created($"/todo-items/{createdTodo.Id}", createdTodo);
        }).RequireAuthorization();
        todoItems.MapPut("/{id}",
            async (int id, TodoItemDTO todoItemDTO, ITodoService todoService) =>
            {
                return TypedResults.Ok(await todoService.UpdateTodoAsync(id, todoItemDTO));
            }
        );
        todoItems.MapDelete("/{id}",
            async (int id, ITodoService todoService) =>
            {
                return TypedResults.Ok(await todoService.DeleteTodoAsync(id));
            }
        );
    }
}
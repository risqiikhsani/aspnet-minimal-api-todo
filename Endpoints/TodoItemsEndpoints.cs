using MinimalApiTodoApi.Models;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Authorization;
namespace MinimalApiTodoApi.Endpoints;

public static class TodoItemsEndpoints
{
    public static void RegisterTodoItemsEndpoints(this WebApplication app)
    {
        RouteGroupBuilder todoItems = app.MapGroup("/todo-items");
        todoItems.MapGet("/",
             async (ITodoService todoService) => { return TypedResults.Ok(await todoService.GetAllTodosAsync()); }).WithName("GetTodos").WithTags("Todos");
        todoItems.MapGet("/complete",
            async (ITodoService todoService) => { return TypedResults.Ok(await todoService.GetCompleteTodosAsync()); }
        ).WithName("GetCompletedTodos").WithTags("Todos");
        todoItems.MapGet("/{id}",
            async (int id, ITodoService todoService) =>
            {
                return TypedResults.Ok(await todoService.GetTodoByIdAsync(id));
            }
        ).WithName("GetTodosById").WithTags("Todos");
        todoItems.MapPost("/", [Authorize] async (TodoItemDTO todoItemDTO, ITodoService todoService) =>
        {
            var createdTodo = await todoService.CreateTodoAsync(todoItemDTO);
            return TypedResults.Created($"/todo-items/{createdTodo.Id}", createdTodo);
        }).WithName("CreateTodo").WithTags("Todos").WithOpenApi();
        todoItems.MapPut("/{id}",
            [Authorize] async (int id, TodoItemDTO todoItemDTO, ITodoService todoService) =>
            {
                return TypedResults.Ok(await todoService.UpdateTodoAsync(id, todoItemDTO));
            }
        ).WithName("UpdateTodo").WithTags("Todos");
        todoItems.MapDelete("/{id}",
            [Authorize] async (int id, ITodoService todoService) =>
            {
                return TypedResults.Ok(await todoService.DeleteTodoAsync(id));
            }
        ).WithName("DeleteTodo").WithTags("Todos");
    }
}
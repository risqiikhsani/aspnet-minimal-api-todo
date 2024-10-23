using NSwag.AspNetCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using MinimalApiTodoApi.Services;
using MinimalApiTodoApi.Database;
using MinimalApiTodoApi.Models;
using MinimalApiTodoApi.Endpoints;


var builder = WebApplication.CreateBuilder(args);

// Configure JSON logging to the console.
// builder.Logging.AddJsonConsole();


// Add the memory cache services.
builder.Services.AddMemoryCache();

// Add a custom scoped service.
builder.Services.AddScoped<ITodoService, TodoService>();


builder.Services.AddCors();
// Requires Microsoft.AspNetCore.Authentication.JwtBearer
builder.Services.AddAuthentication().AddJwtBearer();
builder.Services.AddAuthorization();

// Configure JSON serialization options globally
builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.WriteIndented = true;
    options.SerializerOptions.IncludeFields = true;
});


// builder.Services.AddDbContext<TodoDb>(opt => opt.UseInMemoryDatabase("TodoList"));
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("Database")));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApiDocument(config =>
{
    config.DocumentName = "TodoAPI";
    config.Title = "TodoAPI v1";
    config.Version = "v1";
});

var app = builder.Build();
app.UseCors();
app.UseAuthentication();
app.UseAuthorization();
if (app.Environment.IsDevelopment())
{
    app.UseOpenApi();
    app.UseSwaggerUi(config =>
    {
        config.DocumentTitle = "TodoAPI";
        config.Path = "/swagger";
        config.DocumentPath = "/swagger/{documentName}/swagger.json";
        config.DocExpansion = "list";
    });
}

//////////////////////////////////////////////////////////// STEP 1

// app.MapGet("/todoitems", async (TodoDb db) =>
//     await db.Todos.ToListAsync());

// app.MapGet("/todoitems/complete", async (TodoDb db) =>
//     await db.Todos.Where(t => t.IsComplete).ToListAsync());

// app.MapGet("/todoitems/{id}", async (int id, TodoDb db) =>
//     await db.Todos.FindAsync(id)
//         is Todo todo
//             ? Results.Ok(todo)
//             : Results.NotFound());

// app.MapPost("/todoitems", async (Todo todo, TodoDb db) =>
// {
//     db.Todos.Add(todo);
//     await db.SaveChangesAsync();

//     return Results.Created($"/todoitems/{todo.Id}", todo);
// });

// app.MapPut("/todoitems/{id}", async (int id, Todo inputTodo, TodoDb db) =>
// {
//     var todo = await db.Todos.FindAsync(id);

//     if (todo is null) return Results.NotFound();

//     todo.Name = inputTodo.Name;
//     todo.IsComplete = inputTodo.IsComplete;

//     await db.SaveChangesAsync();

//     return Results.NoContent();
// });

// app.MapDelete("/todoitems/{id}", async (int id, TodoDb db) =>
// {
//     if (await db.Todos.FindAsync(id) is Todo todo)
//     {
//         db.Todos.Remove(todo);
//         await db.SaveChangesAsync();
//         return Results.NoContent();
//     }

//     return Results.NotFound();
// });

/////////////////////////////////////////// STEP 2

// var todoItems = app.MapGroup("/todoitems");
// // MapGroup method is available to help organize such groups. It reduces repetitive code and allows for customizing entire groups of endpoints with a single call to methods like RequireAuthorization and WithMetadata.

// todoItems.MapGet("/", async (TodoDb db) =>
//     await db.Todos.ToListAsync());

// todoItems.MapGet("/complete", async (TodoDb db) =>
//     await db.Todos.Where(t => t.IsComplete).ToListAsync());

// todoItems.MapGet("/{id}", async (int id, TodoDb db) =>
//     await db.Todos.FindAsync(id)
//         is Todo todo
//             ? Results.Ok(todo)
//             : Results.NotFound());

// todoItems.MapPost("/", async (Todo todo, TodoDb db) =>
// {
//     db.Todos.Add(todo);
//     await db.SaveChangesAsync();

//     return Results.Created($"/todoitems/{todo.Id}", todo);
// });

// todoItems.MapPut("/{id}", async (int id, Todo inputTodo, TodoDb db) =>
// {
//     var todo = await db.Todos.FindAsync(id);

//     if (todo is null) return Results.NotFound();

//     todo.Name = inputTodo.Name;
//     todo.IsComplete = inputTodo.IsComplete;

//     await db.SaveChangesAsync();

//     return Results.NoContent();
// });

// todoItems.MapDelete("/{id}", async (int id, TodoDb db) =>
// {
//     if (await db.Todos.FindAsync(id) is Todo todo)
//     {
//         db.Todos.Remove(todo);
//         await db.SaveChangesAsync();
//         return Results.NoContent();
//     }

//     return Results.NotFound();
// });

/////////////////////////////////////////// STEP 3
// Returning TypedResults rather than Results has several advantages, including testability and automatically returning the response type metadata for OpenAPI to describe the endpoint. For more information, see TypedResults vs Results.

// var todoItems = app.MapGroup("/todoitems");

// todoItems.MapGet("/", GetAllTodos);
// todoItems.MapGet("/complete", GetCompleteTodos);
// todoItems.MapGet("/{id}", GetTodo);
// todoItems.MapPost("/", CreateTodo);
// todoItems.MapPut("/{id}", UpdateTodo);
// todoItems.MapDelete("/{id}", DeleteTodo);


// static async Task<IResult> GetAllTodos(TodoDb db)
// {
//     return TypedResults.Ok(await db.Todos.ToArrayAsync());
// }

// static async Task<IResult> GetCompleteTodos(TodoDb db)
// {
//     return TypedResults.Ok(await db.Todos.Where(t => t.IsComplete).ToListAsync());
// }

// static async Task<IResult> GetTodo(int id, TodoDb db)
// {
//     return await db.Todos.FindAsync(id)
//         is Todo todo
//             ? TypedResults.Ok(todo)
//             : TypedResults.NotFound();
// }

// static async Task<IResult> CreateTodo(Todo todo, TodoDb db)
// {
//     db.Todos.Add(todo);
//     await db.SaveChangesAsync();

//     return TypedResults.Created($"/todoitems/{todo.Id}", todo);
// }

// static async Task<IResult> UpdateTodo(int id, Todo inputTodo, TodoDb db)
// {
//     var todo = await db.Todos.FindAsync(id);

//     if (todo is null) return TypedResults.NotFound();

//     todo.Name = inputTodo.Name;
//     todo.IsComplete = inputTodo.IsComplete;

//     await db.SaveChangesAsync();

//     return TypedResults.NoContent();
// }

// static async Task<IResult> DeleteTodo(int id, TodoDb db)
// {
//     if (await db.Todos.FindAsync(id) is Todo todo)
//     {
//         db.Todos.Remove(todo);
//         await db.SaveChangesAsync();
//         return TypedResults.NoContent();
//     }

//     return TypedResults.NotFound();
// }

////////////////////////////////////////// STEP 4
// using DTO
// RouteGroupBuilder todoItems = app.MapGroup("/todoitems");
// todoItems.MapGet("/", GetAllTodos);
// todoItems.MapGet("/complete", GetCompleteTodos);
// todoItems.MapGet("/{id}", GetTodo);
// todoItems.MapPost("/", CreateTodo);
// todoItems.MapPut("/{id}", UpdateTodo);
// todoItems.MapDelete("/{id}", DeleteTodo);

// static async Task<IResult> GetAllTodos(TodoDb db)
// {
//     return TypedResults.Ok(await db.Todos.Select(x => new TodoItemDTO(x)).ToArrayAsync());
// }

// static async Task<IResult> GetCompleteTodos(TodoDb db) {
//     return TypedResults.Ok(await db.Todos.Where(t => t.IsComplete).Select(x => new TodoItemDTO(x)).ToListAsync());
// }

// static async Task<IResult> GetTodo(int id, TodoDb db)
// {
//     return await db.Todos.FindAsync(id)
//         is Todo todo
//             ? TypedResults.Ok(new TodoItemDTO(todo))
//             : TypedResults.NotFound();
// }

// static async Task<IResult> CreateTodo(TodoItemDTO todoItemDTO, TodoDb db)
// {
//     var todoItem = new Todo
//     {
//         IsComplete = todoItemDTO.IsComplete,
//         Name = todoItemDTO.Name
//     };

//     db.Todos.Add(todoItem);
//     await db.SaveChangesAsync();

//     todoItemDTO = new TodoItemDTO(todoItem);

//     return TypedResults.Created($"/todoitems/{todoItem.Id}", todoItemDTO);
// }

// static async Task<IResult> UpdateTodo(int id, TodoItemDTO todoItemDTO, TodoDb db)
// {
//     var todo = await db.Todos.FindAsync(id);

//     if (todo is null) return TypedResults.NotFound();

//     todo.Name = todoItemDTO.Name;
//     todo.IsComplete = todoItemDTO.IsComplete;

//     await db.SaveChangesAsync();

//     return TypedResults.NoContent();
// }

// static async Task<IResult> DeleteTodo(int id, TodoDb db)
// {
//     if (await db.Todos.FindAsync(id) is Todo todo)
//     {
//         db.Todos.Remove(todo);
//         await db.SaveChangesAsync();
//         return TypedResults.NoContent();
//     }

//     return TypedResults.NotFound();
// }


app.RegisterTodoItemsEndpoints();
app.Run();
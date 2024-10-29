using MinimalApiTodoApi.Models;
using MinimalApiTodoApi.Services;

namespace MinimalApiTodoApi.Endpoints;

public static class AuthEndpoints
{
    public static void RegisterAuthEndpoints(this WebApplication app)
    {
        RouteGroupBuilder authGroup = app.MapGroup("/auth");
        authGroup.MapPost("/register", async (AuthService authService, string email, string username, string password) =>
        {
            var result = await authService.RegisterAsync(email, username, password);
            return result.Success ? Results.Ok(result.Message) : Results.Conflict(result.Message);
        }).WithTags("Auth");

        authGroup.MapPost("/login", async (AuthService authService, string username, string password) =>
        {
            var result = await authService.LoginAsync(username, password);
            if (result.Success)
                return Results.Ok(new { Token = result.Token, Message = result.Message });

            return Results.BadRequest(result.Message);
        }).WithTags("Auth");
    }
}
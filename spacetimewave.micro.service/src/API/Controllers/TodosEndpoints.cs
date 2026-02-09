using Microsoft.AspNetCore.Mvc;
using Application.Services;
using Domain.Entities;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace API.Controllers
{
    public static class TodosEndpoints
    {
        public sealed class TodosLogger;
        
        public static void MapTodosEndpoints(this IEndpointRouteBuilder routes)
        {
            var group = routes.MapGroup("endpoints/todos");
            group.MapGet("", GetTodos).WithName("GetAllTodos");
            group.MapGet("protected/{id}", GetProtectedTodos).WithName("GetProtectedTodoById").RequireAuthorization();
            group.MapGet("{id}", GetTodoById).WithName("GetTodoById");

            // Using full route:
            // routes.MapGet("endpoints/todos/", GetTodos).WithName("GetAllTodos");

            // Using inline lambda:
            // routes.MapGet("endpoints/todos", async (ITodosService todosService, ILogger logger) =>
            // {
            //     logger.LogInformation("Retrieving all todos.");
            //     var todos = todosService.GetTodos().Data;
            //     return Results.Ok(todos);
            // });
        }

        public static async Task<IResult> GetTodos(ILogger<TodosLogger> logger, ITodosService todosService)
        {
            logger.LogInformation("Retrieving all todos.");
            var todos = todosService.GetTodos().Data;
            return Results.Ok(todos);
        }

        public static async Task<IResult> GetTodoById(int id, ITodosService todosService)
        {
            var todo = todosService.GetTodoById(id).Data;
            return todo is not null ? Results.Ok(todo) : Results.NotFound();
        }

        public static async Task<IResult> GetProtectedTodos(int id, ILogger<TodosLogger> logger, ClaimsPrincipal user, ITodosService todosService)
        {
            // The JWT middleware does claim mapping by default:
            // JwtSecurityTokenHandler.DefaultInboundClaimTypeMap uses the following mapping:
            // JWT Claim	Mapped ClaimType            Mapped ClaimURI
            // "sub"	    ClaimTypes.NameIdentifier   http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier
            // "email"	    ClaimTypes.Email            http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress
            // "name"	    ClaimTypes.Name             http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name
            string? userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            // TODO: Check user permissions

            var todo = todosService.GetTodoById(id).Data;
            return todo is not null ? Results.Ok(todo) : Results.NotFound();
        }
    }
}
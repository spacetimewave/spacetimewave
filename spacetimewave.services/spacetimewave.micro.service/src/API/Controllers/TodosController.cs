using Microsoft.AspNetCore.Mvc;
using Application.Services;
using Domain.Entities;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TodosController : ControllerBase
    {
        private readonly ITodosService _todosService;
        private readonly ILogger<TodosController> _logger;

        public TodosController(ITodosService todosService, ILogger<TodosController> logger)
        {
            _logger = logger;
            _todosService = todosService;
        }

        [HttpGet]
        public ActionResult<Todo[]> GetTodos()
        {
            _logger.LogInformation("Retrieving all todos.");
            return Ok(_todosService.GetTodos().Data);
        }

        [HttpGet("{id}")]
        public ActionResult<Todo> GetTodoById(int id)
        {
            var todo = _todosService.GetTodoById(id).Data;
            return todo is not null ? Ok(todo) : NotFound();
        }

        /// <summary>
        /// Requires authentication.
        /// "Authorization": Bearer {token}
        /// </summary>
        [HttpGet("protected/{id}")]
        [Authorize]
        public ActionResult<Todo> GetProtectedTodos(int id)
        {
            string? userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var todo = _todosService.GetTodoById(id).Data;
            return todo is not null ? Ok(todo) : NotFound();
        }
    }
}
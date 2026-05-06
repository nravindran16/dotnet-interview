using Microsoft.AspNetCore.Mvc;
using TodoApi.Models;
using TodoApi.Services;

namespace TodoApi.Controllers
{
    [ApiController]
    [Route("api/todos")]
    public class TodoController : ControllerBase
    {
        private readonly TodoApi.Services.ITodoService _todoService;

        public TodoController(TodoApi.Services.ITodoService todoService)
        {
            _todoService = todoService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateTodo([FromBody] Todo todo)
        {
            try
            {
                var result = await _todoService.CreateTodoAsync(todo);
                return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var todo = await _todoService.GetTodoByIdAsync(id);
            if (todo == null) return NotFound();
            return Ok(todo);
        }

        [HttpGet]
        public async Task<IActionResult> GetTodo()
        {
            try
            {
                var todos = await _todoService.GetAllTodosAsync();
                return Ok(todos);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTodo(int id, [FromBody] UpdateTodoRequest request)
        {
            try
            {
                var existingTodo = await _todoService.GetTodoByIdAsync(id);
                if (existingTodo == null)
                {
                    return NotFound();
                }

                var todo = new Todo
                {
                    Title = request.Title,
                    Description = request.Description,
                    IsCompleted = request.IsCompleted
                };

                var result = await _todoService.UpdateTodoAsync(id, todo);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTodo(int id)
        {
            try
            {
                var result = await _todoService.DeleteTodoAsync(id);
                if (result)
                {
                    return Ok(new { message = "Todo deleted successfully" });
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }

    public class UpdateTodoRequest
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsCompleted { get; set; }
    }
}

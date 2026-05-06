using System.Collections.Generic;
using System.Threading.Tasks;
using TodoApi.Models;

namespace TodoApi.Services
{
    public interface ITodoService
    {
        Task<Todo> CreateTodoAsync(Todo todo);
        Task<List<Todo>> GetAllTodosAsync();
        Task<Todo?> GetTodoByIdAsync(int id);
        Task<Todo> UpdateTodoAsync(int id, Todo todo);
        Task<bool> DeleteTodoAsync(int id);
    }
}

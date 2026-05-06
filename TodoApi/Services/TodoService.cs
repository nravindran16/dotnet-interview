using System.Collections.Generic;
using System.Threading.Tasks;
using TodoApi.Models;
using TodoApi.Repositories;

namespace TodoApi.Services
{
    public class TodoService : ITodoService
    {
        private readonly ITodoRepository _repository;

        public TodoService(ITodoRepository repository)
        {
            _repository = repository;
        }

        public async Task<Todo> CreateTodoAsync(Todo todo)
        {
            var id = await _repository.InsertAsync(todo);
            todo.Id = id;
            todo.CreatedAt = DateTime.UtcNow;
            return todo;
        }

        public Task<List<Todo>> GetAllTodosAsync() => _repository.GetAllAsync();

        public Task<Todo?> GetTodoByIdAsync(int id) => _repository.GetByIdAsync(id);

        public async Task<Todo> UpdateTodoAsync(int id, Todo todo)
        {
            await _repository.UpdateAsync(id, todo);
            todo.Id = id;
            return todo;
        }

        public async Task<bool> DeleteTodoAsync(int id) => (await _repository.DeleteAsync(id)) > 0;
    }
}

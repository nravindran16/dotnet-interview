using System.Collections.Generic;
using System.Threading.Tasks;
using TodoApi.Models;

namespace TodoApi.Repositories
{
    public interface ITodoRepository
    {
        Task<int> InsertAsync(Todo todo);
        Task<List<Todo>> GetAllAsync();
        Task<Todo?> GetByIdAsync(int id);
        Task<int> UpdateAsync(int id, Todo todo);
        Task<int> DeleteAsync(int id);
    }
}

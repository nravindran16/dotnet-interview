using System.Collections.Generic;
using TodoApi.Models;

namespace TodoApi.Repositories
{
    public interface ITodoRepository
    {
        int Insert(Todo todo);
        List<Todo> GetAll();
        Todo? GetById(int id);
        int Update(int id, Todo todo);
        int Delete(int id);
    }
}

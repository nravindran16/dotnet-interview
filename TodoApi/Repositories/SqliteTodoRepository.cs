using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;
using TodoApi.Models;

namespace TodoApi.Repositories
{
    public class SqliteTodoRepository : ITodoRepository
    {
        private readonly string _connectionString;

        public SqliteTodoRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("TodoDatabase") ?? "Data Source=todos.db";
        }

        public async Task<int> InsertAsync(Todo todo)
        {
            await using var connection = new SqliteConnection(_connectionString);
            await connection.OpenAsync();

            await using var command = connection.CreateCommand();
            command.CommandText = @"INSERT INTO Todos (Title, Description, IsCompleted, CreatedAt) VALUES ($title, $description, $isCompleted, $createdAt); SELECT last_insert_rowid();";
            command.Parameters.AddWithValue("$title", todo.Title ?? string.Empty);
            command.Parameters.AddWithValue("$description", todo.Description ?? string.Empty);
            command.Parameters.AddWithValue("$isCompleted", todo.IsCompleted ? 1 : 0);
            command.Parameters.AddWithValue("$createdAt", DateTime.UtcNow.ToString("o"));

            var result = await command.ExecuteScalarAsync();
            return Convert.ToInt32(result);
        }

        public async Task<List<Todo>> GetAllAsync()
        {
            var todos = new List<Todo>();
            await using var connection = new SqliteConnection(_connectionString);
            await connection.OpenAsync();

            await using var command = connection.CreateCommand();
            command.CommandText = "SELECT Id, Title, Description, IsCompleted, CreatedAt FROM Todos";

            await using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                todos.Add(new Todo
                {
                    Id = reader.GetInt32(0),
                    Title = reader.GetString(1),
                    Description = reader.IsDBNull(2) ? string.Empty : reader.GetString(2),
                    IsCompleted = reader.GetInt32(3) == 1,
                    CreatedAt = DateTime.Parse(reader.GetString(4))
                });
            }

            return todos;
        }

        public async Task<Todo?> GetByIdAsync(int id)
        {
            await using var connection = new SqliteConnection(_connectionString);
            await connection.OpenAsync();

            await using var command = connection.CreateCommand();
            command.CommandText = "SELECT Id, Title, Description, IsCompleted, CreatedAt FROM Todos WHERE Id = $id";
            command.Parameters.AddWithValue("$id", id);

            await using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return new Todo
                {
                    Id = reader.GetInt32(0),
                    Title = reader.GetString(1),
                    Description = reader.IsDBNull(2) ? string.Empty : reader.GetString(2),
                    IsCompleted = reader.GetInt32(3) == 1,
                    CreatedAt = DateTime.Parse(reader.GetString(4))
                };
            }

            return null;
        }

        public async Task<int> UpdateAsync(int id, Todo todo)
        {
            await using var connection = new SqliteConnection(_connectionString);
            await connection.OpenAsync();

            await using var command = connection.CreateCommand();
            command.CommandText = @"UPDATE Todos SET Title = $title, Description = $description, IsCompleted = $isCompleted WHERE Id = $id";
            command.Parameters.AddWithValue("$title", todo.Title ?? string.Empty);
            command.Parameters.AddWithValue("$description", todo.Description ?? string.Empty);
            command.Parameters.AddWithValue("$isCompleted", todo.IsCompleted ? 1 : 0);
            command.Parameters.AddWithValue("$id", id);

            return await command.ExecuteNonQueryAsync();
        }

        public async Task<int> DeleteAsync(int id)
        {
            await using var connection = new SqliteConnection(_connectionString);
            await connection.OpenAsync();

            await using var command = connection.CreateCommand();
            command.CommandText = "DELETE FROM Todos WHERE Id = $id";
            command.Parameters.AddWithValue("$id", id);

            return await command.ExecuteNonQueryAsync();
        }
    }
}

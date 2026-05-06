using Xunit;
using TodoApi.Services;
using TodoApi.Models;
using TodoApi.Controllers;
using Moq;
using TodoApi.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TodoApi.Tests;

public class UnitTest1
{
    [Fact]
    public void Test1()
    {
        Assert.True(true);
    }

    [Fact]
    public async Task TestCreateTodo()
    {
        var mockRepo = new Mock<ITodoRepository>();
        mockRepo.Setup(r => r.InsertAsync(It.IsAny<Todo>())).ReturnsAsync(1);
        var service = new TodoService(mockRepo.Object);

        var todo = new Todo
        {
            Title = "Test",
            Description = "Test Description",
            IsCompleted = false
        };

        var result = await service.CreateTodoAsync(todo);

        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
    }

    [Fact]
    public async Task TestGetTodo()
    {
        var mockRepo = new Mock<ITodoRepository>();
        mockRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(new List<Todo> { new Todo { Id = 1, Title = "T" } });
        var service = new TodoService(mockRepo.Object);

        var todos = await service.GetAllTodosAsync();

        Assert.Single(todos);
    }

    [Fact]
    public async Task UpdateTest()
    {
        var mockRepo = new Mock<ITodoRepository>();
        mockRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(new Todo { Id = 1 });
        mockRepo.Setup(r => r.UpdateAsync(1, It.IsAny<Todo>())).ReturnsAsync(1);
        var service = new TodoService(mockRepo.Object);

        var todo = new Todo
        {
            Title = "Updated",
            Description = "Updated Description",
            IsCompleted = true
        };

        var result = await service.UpdateTodoAsync(1, todo);
        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
    }

    [Fact]
    public async Task DeleteWorks()
    {
        var mockRepo = new Mock<ITodoRepository>();
        mockRepo.Setup(r => r.DeleteAsync(999)).ReturnsAsync(0);
        var service = new TodoService(mockRepo.Object);

        var result = await service.DeleteTodoAsync(999);

        Assert.False(result);
    }

    [Fact]
    public async Task ControllerTest()
    {
        var mockService = new Mock<ITodoService>();
        mockService.Setup(s => s.CreateTodoAsync(It.IsAny<Todo>())).ReturnsAsync(new Todo { Id = 1, Title = "Test" });
        var controller = new TodoController(mockService.Object);
        var todo = new Todo { Title = "Test", Description = "Desc" };

        var result = await controller.CreateTodo(todo);

        Assert.IsType<CreatedAtActionResult>(result);
    }

    [Fact]
    public async Task TestEverything()
    {
        var mockRepo = new Mock<ITodoRepository>();
        mockRepo.SetupSequence(r => r.InsertAsync(It.IsAny<Todo>())).ReturnsAsync(1).ReturnsAsync(2);
        mockRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(new List<Todo> { new Todo { Id = 1 }, new Todo { Id = 2 } });
        mockRepo.Setup(r => r.UpdateAsync(1, It.IsAny<Todo>())).ReturnsAsync(1);
        mockRepo.Setup(r => r.DeleteAsync(2)).ReturnsAsync(1);

        var service = new TodoService(mockRepo.Object);

        var todo1 = await service.CreateTodoAsync(new Todo { Title = "1", Description = "D1" });
        var todo2 = await service.CreateTodoAsync(new Todo { Title = "2", Description = "D2" });

        var all = await service.GetAllTodosAsync();

        await service.UpdateTodoAsync(todo1.Id, new Todo { Title = "Updated", Description = "D1" });

        await service.DeleteTodoAsync(todo2.Id);

        Assert.True(all.Count >= 2);
    }
}

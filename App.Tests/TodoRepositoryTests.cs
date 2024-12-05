using App.Todos.Repositories;
using AutoFixture.AutoMoq;
using AutoFixture;
using Data;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Tests
{
    public class TodoRepositoryTests
    {
        private readonly IFixture _fixture;
        private readonly TodoDbContext _dbContext;
        private readonly TodoRepository _repository;

        public TodoRepositoryTests()
        {
            _fixture = new Fixture().Customize(new AutoMoqCustomization());

            var options = new DbContextOptionsBuilder<TodoDbContext>()
                .UseInMemoryDatabase(databaseName: "TodoDbTest")
                .Options;

            _dbContext = new TodoDbContext(options);
            _repository = new TodoRepository(_dbContext);
        }

        [Fact]
        public async Task AddTodoAsync_ShouldReturnTodoId_WhenTodoIsAdded()
        {
            // Arrange
            var todo = _fixture.Create<TodoItem>();

            // Act
            var result = await _repository.AddTodoAsync(todo);

            // Assert
            Assert.True(result > 0);
            var addedTodo = await _dbContext.Todos.FindAsync(result);
            Assert.NotNull(addedTodo);
        }

        [Fact]
        public async Task UpdateTodoAsync_ShouldReturnTrue_WhenTodoIsUpdated()
        {
            // Arrange
            var todo = _fixture.Create<TodoItem>();
            _dbContext.Todos.Add(todo);
            await _dbContext.SaveChangesAsync();

            todo.Title = "Updated Title";

            // Act
            var result = await _repository.UpdateTodoAsync(todo);

            // Assert
            Assert.True(result);
            var updatedTodo = await _dbContext.Todos.FindAsync(todo.Id);
            Assert.Equal("Updated Title", updatedTodo?.Title);
        }

        [Fact]
        public async Task DeleteTodoAsync_ShouldReturnTrue_WhenTodoIsDeleted()
        {
            // Arrange
            var todo = _fixture.Create<TodoItem>();
            _dbContext.Todos.Add(todo);
            await _dbContext.SaveChangesAsync();

            // Act
            var result = await _repository.DeleteTodoAsync(todo.Id);

            // Assert
            Assert.True(result);
            var deletedTodo = await _dbContext.Todos.FindAsync(todo.Id);
            Assert.Null(deletedTodo);
        }

        [Fact]
        public async Task GetTodoByIdAsync_ShouldReturnTodo_WhenTodoExists()
        {
            // Arrange
            var todo = _fixture.Create<TodoItem>();
            _dbContext.Todos.Add(todo);
            await _dbContext.SaveChangesAsync();

            // Act
            var result = await _repository.GetTodoByIdAsync(todo.Id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(todo.Id, result?.Id);
        }

        [Fact]
        public async Task GetAllTodosAsync_ShouldReturnListOfTodos_WhenTodosExist()
        {
            // Arrange
            var todos = _fixture.CreateMany<TodoItem>(5).ToList();
            _dbContext.Todos.AddRange(todos);
            await _dbContext.SaveChangesAsync();

            // Act
            var result = (await _repository.GetAllTodosAsync()).ToList();

            // Assert
            Assert.NotEmpty(result);
            Assert.Equal(5, result.Count);
        }

    }
}

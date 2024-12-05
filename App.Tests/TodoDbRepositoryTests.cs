using App.Todos.Repositories;
using AutoFixture.AutoMoq;
using AutoFixture;
using Data;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Tests
{
    public class TodoDbRepositoryTests
    {
        private readonly IFixture _fixture;
        private readonly Mock<DbSet<TodoItem>> _mockDbSet;
        private readonly Mock<TodoDbContext> _mockContext;
        private readonly TodoRepository _repository;

        public TodoDbRepositoryTests()
        {
            _fixture = new Fixture().Customize(new AutoMoqCustomization());
            _mockDbSet = new Mock<DbSet<TodoItem>>();
            _mockContext = new Mock<TodoDbContext>(new DbContextOptions<TodoDbContext>());
            _repository = new TodoRepository(_mockContext.Object);

            _mockContext.Setup(x => x.Todos).Returns(_mockDbSet.Object);
        }

        [Fact]
        public async Task AddTodoAsync_ShouldReturnTodoId_WhenTodoIsAdded()
        {
            // Arrange
            var todo = _fixture.Create<TodoItem>();
            _mockDbSet.Setup(x => x.AddAsync(todo, default)).ReturnsAsync(new Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<TodoItem>(null));

            // Act
            await _repository.AddTodoAsync(todo);

            // Assert
            _mockContext.Verify(m => m.SaveChangesAsync(default), Times.Once);
        }


        [Fact]
        public async Task UpdateTodoAsync_ShouldReturnTrue_WhenTodoIsUpdated()
        {
            // Arrange
            var todo = _fixture.Create<TodoItem>();
            _mockDbSet.Setup(x => x.FindAsync(todo.Id)).ReturnsAsync(todo);
            _mockContext.Setup(m => m.Todos.Update(It.IsAny<TodoItem>()));

            // Act
            var result = await _repository.UpdateTodoAsync(todo);

            // Assert
            Assert.True(result);
            _mockContext.Verify(m => m.SaveChangesAsync(default), Times.Once);
        }

        [Fact]
        public async Task DeleteTodoAsync_ShouldReturnTrue_WhenTodoIsDeleted()
        {
            // Arrange
            var todoId = _fixture.Create<int>();
            var todo = new TodoItem { Id = todoId };
            _mockDbSet.Setup(x => x.FindAsync(todo.Id)).ReturnsAsync(todo);
            _mockContext.Setup(m => m.Todos.Remove(It.IsAny<TodoItem>()));

            // Act
            var result = await _repository.DeleteTodoAsync(todoId);

            // Assert
            Assert.True(result);
            _mockContext.Verify(m => m.SaveChangesAsync(default), Times.Once);
        }


        [Fact]
        public async Task GetTodoByIdAsync_ShouldReturnTodo_WhenTodoExists()
        {
            // Arrange
            var todo = _fixture.Create<TodoItem>();
            _mockDbSet.Setup(x => x.FindAsync(todo.Id)).ReturnsAsync(todo);

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
            var todos = _fixture.CreateMany<TodoItem>(3).ToList();
            _mockDbSet.Setup(x => x.ToListAsync(default)).ReturnsAsync(todos);

            // Act
            var result = await _repository.GetAllTodosAsync();

            // Assert
            Assert.NotEmpty(result);
            Assert.Equal(3, result.Count());
        }
    }
}

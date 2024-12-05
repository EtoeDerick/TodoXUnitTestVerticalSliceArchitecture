using App.Todos.Commands;
using App.Todos.Handlers;
using App.Todos.Repositories;
using AutoFixture;
using AutoFixture.AutoMoq;
using Entities.Models;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Tests
{
    public class CreateTodoHandlerTests
    {
        private readonly IFixture _fixture;
        private readonly Mock<ITodoRepository> _todoRepositoryMock;
        private readonly CreateTodoHandler _handler;

        public CreateTodoHandlerTests()
        {
            _fixture = new Fixture().Customize(new AutoMoqCustomization());
            _todoRepositoryMock = _fixture.Freeze<Mock<ITodoRepository>>();
            _handler = new CreateTodoHandler(_todoRepositoryMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnTodoId_WhenTodoIsSuccessfullyCreated()
        {
            var command = new CreateTodoCommand("New Todo");
            var expectedTodoId = 1;
            _todoRepositoryMock.Setup(repo => repo.AddTodoAsync(It.IsAny<TodoItem>())).ReturnsAsync(expectedTodoId);

            //act
            var result = await _handler.Handle(command, CancellationToken.None);

            //assert
            Assert.Equal(expectedTodoId, result);
            _todoRepositoryMock.Verify(repo => repo.AddTodoAsync(It.IsAny<TodoItem>()), Times.Once());
        }

        [Fact]
        public async Task Handle_ShouldThrowArgumentException_WhenTitleIsEmpty()
        {
            var command = new CreateTodoCommand("");

            await Assert.ThrowsAsync<ArgumentException>(() => _handler.Handle(command, CancellationToken.None));
        }
    }
}

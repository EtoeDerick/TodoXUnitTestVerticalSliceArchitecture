using Api.Controllers;
using App.Todos.Commands;
using App.Todos.Queries;
using Entities.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Tests
{
    public class TodosControllerTests
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly TodosController _controller;

        public TodosControllerTests()
        {
            _mediatorMock = new Mock<IMediator>();
            _controller = new TodosController( _mediatorMock.Object );
        }

        [Fact]
        public async Task Get_ReturnsTodoItem_WhenTodoExists()
        {
            var todoId = 1;
            var expectedTodo = new TodoItem { Id = todoId, Title = "Sample Todo", IsCompleted = false };

            _mediatorMock.Setup(m => m.Send(It.IsAny<GetTodoQuery>(), default)).ReturnsAsync(expectedTodo);

            //Act
            var result = await _controller.Get( todoId ) as OkObjectResult;

            //Assert
            Assert.NotNull( result );
            Assert.Equal(200, result.StatusCode );
            Assert.Equal(expectedTodo, result.Value );
        }

        [Fact]
        public async Task Update_ReturnsNoContent_WhenTodoIsUpdated()
        {
            // Arrange
            var command = new UpdateTodoCommand(1, "Updated Title");
            _mediatorMock.Setup(m => m.Send(It.IsAny<UpdateTodoCommand>(), default))
                         .ReturnsAsync(true);

            // Act
            var result = await _controller.Update(1, command) as NoContentResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(204, result.StatusCode);
        }

        [Fact]
        public async Task Delete_ReturnsNoContent_WhenTodoIsDeleted()
        {
            // Arrange
            var todoId = 1;
            _mediatorMock.Setup(m => m.Send(It.IsAny<DeleteTodoCommand>(), default))
                         .ReturnsAsync(true);

            // Act
            var result = await _controller.Delete(todoId) as NoContentResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(204, result.StatusCode);
        }

    }
}

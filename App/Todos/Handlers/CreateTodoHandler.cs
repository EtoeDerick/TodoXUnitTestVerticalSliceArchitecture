using App.Todos.Commands;
using App.Todos.Repositories;
using Entities.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Todos.Handlers
{
    public class CreateTodoHandler : IRequestHandler<CreateTodoCommand, int>
    {
        private readonly ITodoRepository _todoRepository;

        public CreateTodoHandler(ITodoRepository todoRepository)
        {
            _todoRepository = todoRepository;
        }

        public async Task<int> Handle(CreateTodoCommand request, CancellationToken cancellationToken)
        {
            if(string.IsNullOrWhiteSpace(request.title)) 
                throw new ArgumentException("Title can not be empty.", nameof(request.title));

            var newTodo = new TodoItem { Title = request.title, IsCompleted = false };
            return await _todoRepository.AddTodoAsync(newTodo);
        }
    }
}

using Entities.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Todos.Queries
{
    public record GetTodoQuery(int id) : IRequest<TodoItem>;
}

using Entities.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Todos.Commands
{
    public record CreateTodoCommand(string title) : IRequest<int>;
}

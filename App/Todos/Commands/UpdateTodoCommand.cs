using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Todos.Commands
{
    public record UpdateTodoCommand(int id, string title) : IRequest<bool>;
}

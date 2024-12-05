using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Todos.Repositories
{
    public interface ITodoRepository
    {
        Task<int> AddTodoAsync(TodoItem todo);
        Task<bool> UpdateTodoAsync(TodoItem todo);
        Task<bool> DeleteTodoAsync(int id );
        Task<TodoItem> GetTodoByIdAsync(int id);
        Task<IEnumerable<TodoItem>> GetAllTodosAsync();
    }
}

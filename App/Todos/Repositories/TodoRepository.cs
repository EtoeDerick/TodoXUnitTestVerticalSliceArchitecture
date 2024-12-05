using Data;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Todos.Repositories
{
    public class TodoRepository : ITodoRepository
    {
        private readonly TodoDbContext _context;

        public TodoRepository(TodoDbContext context)
        {
            _context = context;
        }

        public async Task<int> AddTodoAsync(TodoItem todo)
        {
            await _context.Todos.AddAsync(todo);
            await _context.SaveChangesAsync();
            return todo.Id;
        }

        public async Task<bool> DeleteTodoAsync(int id)
        {
            var todo = await _context.Todos.FindAsync(id);
            if (todo == null) return false;

            _context.Todos.Remove(todo);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<TodoItem>> GetAllTodosAsync()
        {
            return await _context.Todos.ToListAsync();
        }

        public async Task<TodoItem> GetTodoByIdAsync(int id)
        {
            var todo = await _context.Todos.FindAsync(id);
            if(todo == null) return new TodoItem();
            return todo;
        }

        public async Task<bool> UpdateTodoAsync(TodoItem todo)
        {
            var existingTodo = await _context.Todos.FindAsync(todo.Id);
            if(existingTodo == null)  return false;

            existingTodo.Title = todo.Title;
            existingTodo.IsCompleted = todo.IsCompleted;

            _context.Todos.Update(existingTodo);
            await _context.SaveChangesAsync();
            return true;    
        }
    }
}

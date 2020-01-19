using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;
using TodoApiNet.Contexts;
using TodoApiNet.Models;

namespace TodoApiNet.Repositories
{
    public class TodoRepository : ITodoRepository
    {
        private readonly TodoApiContext _context;

        public TodoRepository(TodoApiContext context) => _context = context;

        /// <summary>
        /// GET
        /// </summary>

        #region snippet_GetAll

        public async Task<IEnumerable<Todo>> GetAllAsync() => await _context.Todos.ToListAsync();

        #endregion

        #region snippet_GetById

        public async Task<Todo> GetByIdAsync(long id) => await _context.Todos.AsNoTracking().FirstOrDefaultAsync(t => t.Id == id);

        #endregion

        /// <summary>
        /// POST
        /// </summary>

        #region snippet_Create

        public async Task Create(Todo todo)
        {
            await _context.Todos.AddAsync(todo);
            await _context.SaveChangesAsync();
        }

        #endregion

        /// <summary>
        /// PATCH
        /// </summary>

        #region snippet_Update

        public async Task UpdateAsync(Todo newTodo, JsonPatchDocument<Todo> currentTodo)
        {
            currentTodo.ApplyTo(newTodo);
            _context.Entry(newTodo).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        #endregion

        /// <summary>
        /// DELETE
        /// </summary>

        #region snippet_Delete

        public async Task DeleteAsync(long id)
        {
            _context.Todos.Remove(new Todo { Id = id });
            await _context.SaveChangesAsync();
        }

        #endregion
    }
}
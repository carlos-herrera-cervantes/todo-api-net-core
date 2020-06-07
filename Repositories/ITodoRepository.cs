using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.JsonPatch;
using TodoApiNet.Models;

namespace TodoApiNet.Repositories
{
    public interface ITodoRepository
    {
         Task<IEnumerable<Todo>> GetAllAsync(Request querys);

         Task<Todo> GetByIdAsync(string id);

         Task Create(Todo todo);

         Task UpdateAsync(string id, Todo newTodo, JsonPatchDocument<Todo> currentTodo);

         Task DeleteAsync(string id);
    }
}
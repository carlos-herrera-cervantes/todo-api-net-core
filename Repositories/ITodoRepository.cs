using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.JsonPatch;
using TodoApiNet.Models;

namespace TodoApiNet.Repositories
{
    public interface ITodoRepository
    {
         Task<IEnumerable<Todo>> GetAllAsync();

         Task<Todo> GetByIdAsync(long id);

         Task Create(Todo todo);

         Task UpdateAsync(Todo newTodo, JsonPatchDocument<Todo> currentTodo);

         Task DeleteAsync(long id);
    }
}
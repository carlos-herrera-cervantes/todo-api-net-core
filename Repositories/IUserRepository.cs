using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.JsonPatch;
using TodoApiNet.Models;

namespace TodoApiNet.Repositories
{
    public interface IUserRepository
    {
         Task CreateAsync(User user);

         Task<IEnumerable<User>> GetAllAsync();

         Task<User> GetByIdAsync(long id);

         Task UpdateAsync(User newUser, JsonPatchDocument<User> currentUser);

         Task DeleteAsync(long id);
    }
}
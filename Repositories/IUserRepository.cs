using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.JsonPatch;
using TodoApiNet.Models;

namespace TodoApiNet.Repositories
{
    public interface IUserRepository
    {
         Task CreateAsync(User user);

         Task<IEnumerable<User>> GetAllAsync(Request queryParameters);

         Task<User> GetByIdAsync(string id);

         Task<User> GetOneAsync(Request queryParameters);

         Task UpdateAsync(string id, User newUser, JsonPatchDocument<User> currentUser);

         Task DeleteAsync(string id);
    }
}
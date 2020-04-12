using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.JsonPatch;
using MongoDB.Driver;
using TodoApiNet.Models;

namespace TodoApiNet.Repositories
{
    public interface IUserRepository
    {
         Task CreateAsync(User user);

         Task<IEnumerable<User>> GetAllAsync(FilterDefinition<User> filter, string sort);

         Task<User> GetByIdAsync(string id);

         Task<User> GetOneAsync(FilterDefinition<User> filter);

         Task UpdateAsync(string id, User newUser, JsonPatchDocument<User> currentUser);

         Task DeleteAsync(string id);
    }
}
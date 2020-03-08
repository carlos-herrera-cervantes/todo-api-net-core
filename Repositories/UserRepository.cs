using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using TodoApiNet.Contexts;
using TodoApiNet.Models;

namespace TodoApiNet.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IMongoCollection<User> _context;

        public UserRepository(IMongoDBSettings context)
        {
            var client = new MongoClient(context.ConnectionString);
            var database = client.GetDatabase(context.Database);
            _context = database.GetCollection<User>("Users");
        }

        /// <summary>
        /// GET
        /// </summary>
        
        #region snippet_GetAll

        public async Task<IEnumerable<User>> GetAllAsync() => await _context.Find(user => true).ToListAsync();

        #endregion

        #region snippet_GetById

        public async Task<User> GetByIdAsync(string id) => await _context.Find<User>(user => user.Id == id).FirstOrDefaultAsync();

        #endregion

        #region snippet_GetByEmail

        public async Task<User> GetByEmailAsync(string email) => await _context.Find<User>(user => user.Email == email).FirstOrDefaultAsync();

        #endregion

        /// <summary>
        /// POST
        /// </summary>

        #region snippet_Create

        public async Task CreateAsync(User user) => await _context.InsertOneAsync(user);

        #endregion

        /// <summary>
        /// PUT
        /// </summary>

        #region snippet_Update

        public async Task UpdateAsync(string id, User newUser, JsonPatchDocument<User> currentUser)
        {
            currentUser.ApplyTo(newUser);
            await _context.ReplaceOneAsync(user => user.Id == id, newUser);
        }

        #endregion

        /// <summary>
        /// DELETE
        /// </summary>

        #region snippet_Delete

        public async Task DeleteAsync(string id) => await _context.DeleteOneAsync(user => user.Id == id);

        #endregion
    }
}
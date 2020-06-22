using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using TodoApiNet.Contexts;
using TodoApiNet.Extensions;
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

        public async Task<IEnumerable<User>> GetAllAsync(Request queryParameters) => 
            await MongoDBFilter<User>.GetDocuments(_context, queryParameters, new User().Relations);

        #endregion

        #region snippet_GetById

        public async Task<User> GetByIdAsync(string id) => await _context.Find<User>(user => user.Id == id).FirstOrDefaultAsync();

        #endregion

        #region snippet_GetOne

        public async Task<User> GetOneAsync(Request queryParameters) => 
            await MongoDBFilter<User>.GetDocument(_context, queryParameters, new User().Relations);

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

        /// <summary>
        /// COUNT
        /// </summary>

        #region snippet_Count

        public async Task<int> CountAsync(Request queryParameters) => await MongoDBFilter<User>.GetNumberOfDocuments(_context, queryParameters);

        #endregion
    }
}
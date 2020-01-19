using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;
using TodoApiNet.Contexts;
using TodoApiNet.Models;

namespace TodoApiNet.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly TodoApiContext _context;

        public UserRepository(TodoApiContext context) => _context = context;

        /// <summary>
        /// GET
        /// </summary>
        
        #region snippet_GetAll

        public async Task<IEnumerable<User>> GetAllAsync() => await _context.Users.ToListAsync();

        #endregion

        #region snippet_GetById

        public async Task<User> GetByIdAsync(long id) => await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
     
        #endregion

        /// <summary>
        /// POST
        /// </summary>
        
        #region snippet_Create

        public async Task CreateAsync(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }

        #endregion

        /// <summary>
        /// PUT
        /// </summary>

        #region snippet_Update

        public async Task UpdateAsync(User newUser, JsonPatchDocument<User> currentUser)
        {
            currentUser.ApplyTo(newUser);
            _context.Entry(currentUser).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        #endregion

        /// <summary>
        /// DELETE
        /// </summary>

        #region snippet_Delete

        public async Task DeleteAsync(long id)
        {
            _context.Users.Remove(new User { Id = id });
            await _context.SaveChangesAsync();
        }

        #endregion
    }
}
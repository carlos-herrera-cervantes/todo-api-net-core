using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using TodoApiNet.Middlewares;
using TodoApiNet.Models;
using TodoApiNet.Repositories;

namespace TodoApiNet.Controllers
{
    [Authorize]
    [Route("api/v1/users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;

        public UserController(IUserRepository userRepository) => _userRepository = userRepository;

        /// <summary>
        /// GET
        /// </summary>

        #region snippet_GetAll

        [HttpGet]
        public async Task<IEnumerable<User>> GetAllAsync() => await _userRepository.GetAllAsync();

        #endregion

        #region snippet_GetById

        [HttpGet("{id}")]
        [UserExists]
        public async Task<IActionResult> GetByIdAsync(string id) => Ok(await _userRepository.GetByIdAsync(id));

        #endregion

        /// <summary>
        /// POST
        /// </summary>

        #region snippet_Create

        [AllowAnonymous]
        [HttpPost]
        [EmailExists]
        public async Task<IActionResult> CreateAsync(User user)
        {
            await _userRepository.CreateAsync(user);
            return Ok(user);
        }

        #endregion

        /// <summary>
        /// PATCH
        /// </summary>

        #region snippet_Update

        [HttpPatch("{id}")]
        [UserExists]
        public async Task<IActionResult> UpdateAsync(string id, [FromBody] JsonPatchDocument<User> currentUser)
        {
            var newUser = await _userRepository.GetByIdAsync(id);
            await _userRepository.UpdateAsync(id, newUser, currentUser);
            return NoContent();
        }

        #endregion

        /// <summary>
        /// DELETE
        /// </summary>

        #region snippet_Delete

        [HttpDelete("{id}")]
        [UserExists]
        public async Task<IActionResult> DeleteAsync(string id)
        {
            await _userRepository.DeleteAsync(id);
            return NoContent();
        }

        #endregion
    }
}
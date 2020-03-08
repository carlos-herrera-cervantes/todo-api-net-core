using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
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
        public async Task<IActionResult> GetByIdAsync(string id)
        {
            var user = await _userRepository.GetByIdAsync(id);

            if (user is null) { return NotFound(); }

            return Ok(user);
        }

        #endregion

        /// <summary>
        /// POST
        /// </summary>

        #region snippet_Create

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> CreateAsync(User user)
        {
            if (ModelState.IsValid) 
            {
                await _userRepository.CreateAsync(user);
                return Ok(user);
            }
            
            return BadRequest();
        }

        #endregion

        /// <summary>
        /// PATCH
        /// </summary>

        #region snippet_Update

        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateAsync(string id, [FromBody] JsonPatchDocument<User> currentUser)
        {
            var newUser = await _userRepository.GetByIdAsync(id);
            
            if (newUser is null) { return NotFound(); }

            await _userRepository.UpdateAsync(id, newUser, currentUser);
            return NoContent();
        }

        #endregion

        /// <summary>
        /// DELETE
        /// </summary>

        #region snippet_Delete

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(string id)
        {
            var user = await _userRepository.GetByIdAsync(id);

            if (user is null) { return NotFound(); }

            await _userRepository.DeleteAsync(id);
            return NoContent();
        }

        #endregion
    }
}
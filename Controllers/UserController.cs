using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using TodoApiNet.Extensions;
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
        private readonly ITodoRepository _todoRepository;

        public UserController(IUserRepository userRepository, ITodoRepository todoRepository) => (_userRepository, _todoRepository) = (userRepository, todoRepository);

        /// <summary>
        /// GET
        /// </summary>

        #region snippet_GetAll

        [HttpGet]
        [Role(roles: new string[] { "Admin", "Client" })]
        [PaginateValidator]
        public async Task<IActionResult> GetAllAsync([FromQuery] Request querys)
        {
            var (_, _, paginate) = querys;
            Paginate paginateResponse = null;
            
            if (paginate)
            {
                var totalDocuments = await _userRepository.CountAsync(querys);
                paginateResponse = QueryObject.CreateObjectResponsePaginate(querys, totalDocuments);
            }
            
            return Ok(new Response<IEnumerable<User>>
            {
                Status = true,
                Data = await _userRepository.GetAllAsync(querys),
                Paginate = paginateResponse
            });
        }

        #endregion

        #region snippet_GetById

        [HttpGet("{id}")]
        [UserExists]
        public async Task<IActionResult> GetByIdAsync(string id) => Ok(new Response<User>() { Status = true, Data = await _userRepository.GetByIdAsync(id) });

        #endregion

        #region snippet_GetTodosByUser

        [HttpGet("{id}/todos")]
        [UserExists]
        [PaginateValidator]
        public async Task<IActionResult> GetTodosByUserId(string id, [FromQuery] Request querys)
        {
            var (_, _, paginate) = querys;
            Paginate paginateResponse = null;
            querys.Filters = new string[] { $"UserId={id}" };
            
            if (paginate)
            {
                var totalDocuments = await _todoRepository.CountAsync(querys);
                paginateResponse = QueryObject.CreateObjectResponsePaginate(querys, totalDocuments);
            }

            return Ok(new Response<IEnumerable<Todo>>
            {
                Status = true,
                Data = await _todoRepository.GetAllAsync(querys),
                Paginate = paginateResponse
            });
        }

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
            return Ok(new Response<User>() { Status = true, Data = user });
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
            return Ok(new Response<User>() { Status = true, Data = newUser });
        }

        #endregion

        /// <summary>
        /// DELETE
        /// </summary>

        #region snippet_Delete

        [HttpDelete("{id}")]
        [Role(roles: new string[] { "Admin" })]
        [UserExists]
        public async Task<IActionResult> DeleteAsync(string id)
        {
            await _userRepository.DeleteAsync(id);
            return NoContent();
        }

        #endregion
    }
}
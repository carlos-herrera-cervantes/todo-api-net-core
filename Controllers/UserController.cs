using System;
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
        [PaginateValidator]
        public async Task<IActionResult> GetAllAsync([FromQuery] Request querys)
        {
            var objectQuery = CreateObjectForFilterAndSort<User>("", querys.Sort);
            var objectPaginate = QueryObject<User>.CreateObjectPaginate(querys);
            var users = await _userRepository.GetAllAsync(objectQuery.Filter, objectQuery.FilterSort, objectPaginate);

            return Ok(new Response<IEnumerable<User>>() { Status = true, Data = users });
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
            var objectQuery = CreateObjectForFilterAndSort<Todo>($"UserId-{id}", querys.Sort);
            var objectPaginate = QueryObject<Todo>.CreateObjectPaginate(querys);
            var todos = await _todoRepository.GetAllAsync(objectQuery.Filter, objectQuery.FilterSort, objectPaginate);
            
            return Ok(new Response<IEnumerable<Todo>>() { Status = true, Data = todos });
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
        [UserExists]
        public async Task<IActionResult> DeleteAsync(string id)
        {
            await _userRepository.DeleteAsync(id);
            return NoContent();
        }

        #endregion

        /// <summary>
        /// HELPERS
        /// </summary>

        #region snippet_CreateObjectForFilterAndSort

        private dynamic CreateObjectForFilterAndSort<T>(string query, string sort) where T : class        
        {
            var filterSort = String.IsNullOrEmpty(sort) ? "{}" : QueryObject<T>.CreateObjectQuerySort(sort);
            var filter = QueryObject<T>.CreateObjectQuery(query);

            return new { Filter = filter, FilterSort = filterSort };
        }

        #endregion
    }
}
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
    [Route("api/v1/todos")]
    [ApiController]
    public class TodoController : ControllerBase
    {
        private readonly ITodoRepository _todoRepository;
        private readonly IUserRepository _userRepository;

        public TodoController(ITodoRepository todoRepository, IUserRepository userRepository) => (_todoRepository, _userRepository) = (todoRepository, userRepository);

        /// <summary>
        /// GET
        /// </summary>

        #region snippet_GetAll

        [HttpGet]
        [PaginateValidator]
        public async Task<IActionResult> GetAllAsync([FromQuery] Request querys)
        {
            var filterSort = String.IsNullOrEmpty(querys.Sort) ? "{}" : QueryObject<Todo>.CreateObjectQuerySort(querys.Sort);
            var filter = QueryObject<Todo>.CreateObjectQuery("");
            var objectPaginate = QueryObject<Todo>.CreateObjectPaginate(querys);
            var todos = await _todoRepository.GetAllAsync(filter, filterSort, objectPaginate);

            return Ok(new Response<IEnumerable<Todo>>() { Status = true, Data = todos });
        }

        #endregion

        #region snippet_GetById

        [HttpGet("{id}")]
        [TodoExists]
        public async Task<IActionResult> GetByIdAsync(string id) => Ok(new Response<Todo>() { Status = true, Data = await _todoRepository.GetByIdAsync(id) });

        #endregion

        /// <summary>
        /// POST
        /// </summary>

        #region snippet_Create

        [HttpPost("{id}")]
        [UserExists]
        public async Task<IActionResult> CreateAsync(string id, [FromBody] Todo todo)
        {
            var newUser = await _userRepository.GetByIdAsync(id);

            todo.UserId = id;

            await _todoRepository.Create(todo);
            await UpdateTodosUser(newUser, todo.Id, addTodo: true);

            return Ok(new Response<Todo>() { Status = true, Data = todo });
        }

        #endregion

        /// <summary>
        /// PATCH
        /// </summary>

        #region snippet_Update

        [HttpPatch("{id}")]
        [TodoExists]
        public async Task<IActionResult> UpdateAsync(string id, [FromBody] JsonPatchDocument<Todo> currentTodo)
        {
            var newTodo = await _todoRepository.GetByIdAsync(id);
            await _todoRepository.UpdateAsync(id, newTodo, currentTodo);

            return Ok(new Response<Todo>() { Status = true, Data = newTodo });
        }

        #endregion

        /// <summary>
        /// DELETE
        /// </summary>

        #region snippet_Delete

        [HttpDelete("{id}")]
        [TodoExists]
        public async Task<IActionResult> DeleteAsync(string id)
        {
            var todo = await _todoRepository.GetByIdAsync(id);
            var user = await _userRepository.GetByIdAsync(todo.UserId);

            await UpdateTodosUser(user, todo.Id, addTodo: false);
            await _todoRepository.DeleteAsync(id);

            return NoContent();
        }

        #endregion

        /// <summary>
        /// HELPERS
        /// </summary>

        #region snippet_UpdateUser

        private async Task UpdateTodosUser(User newUser, string todoId, bool addTodo)
        {
            if (newUser is null) { return; }
            if (addTodo) { newUser.Todos.Add(todoId); } else { newUser.Todos.Remove(todoId); }

            var jsonPatchDocument = new JsonPatchDocument<User>();
            jsonPatchDocument.Replace(user => user.Todos, newUser.Todos);

            await _userRepository.UpdateAsync(newUser.Id, newUser, jsonPatchDocument);
        }

        #endregion
    }
}
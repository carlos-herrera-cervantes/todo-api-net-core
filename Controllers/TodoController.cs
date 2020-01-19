using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using TodoApiNet.Models;
using TodoApiNet.Repositories;

namespace TodoApiNet.Controllers
{
    [Route("api/v1/todos")]
    [ApiController]
    public class TodoController : ControllerBase
    {
        private readonly ITodoRepository _todoRepository;

        public TodoController(ITodoRepository todoRepository) => _todoRepository = todoRepository;

        /// <summary>
        /// GET
        /// </summary>

        #region snippet_GetAll

        [HttpGet]
        public async Task<IEnumerable<Todo>> GetAllAsync() => await _todoRepository.GetAllAsync();

        #endregion

        #region snippet_GetById

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(long id)
        {
            var todo = await _todoRepository.GetByIdAsync(id);

            if (todo is null) { return NotFound(); }

            return Ok(todo);
        }

        #endregion

        /// <summary>
        /// POST
        /// </summary>

        #region snippet_Create

        [HttpPost]
        public async Task<IActionResult> CreateAsync(Todo todo)
        {
            if (ModelState.IsValid) 
            {
                await _todoRepository.Create(todo);
                return Ok(todo);
            }

            return BadRequest();
        }

        #endregion

        /// <summary>
        /// PATCH
        /// </summary>

        #region snippet_Update

        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateAsync(long id, [FromBody] JsonPatchDocument<Todo> currentTodo)
        {
            var newTodo = await _todoRepository.GetByIdAsync(id);

            if (newTodo is null) { return NotFound(); }

            await _todoRepository.UpdateAsync(newTodo, currentTodo);
            return NoContent();
        }

        #endregion

        /// <summary>
        /// DELETE
        /// </summary>

        #region snippet_Delete

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(long id)
        {
            var todo = await _todoRepository.GetByIdAsync(id);

            if (todo is null) { return NotFound(); }

            await _todoRepository.DeleteAsync(id);
            return NoContent();
        }

        #endregion
    }
}
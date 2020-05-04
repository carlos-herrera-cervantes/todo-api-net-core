using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Localization;
using TodoApiNet.Models;
using TodoApiNet.Repositories;

namespace TodoApiNet.Middlewares
{
    public class TodoExistsAttribute : TypeFilterAttribute
    {
        public TodoExistsAttribute() : base(typeof(TodoExistsFilter)) { }

        private class TodoExistsFilter : IAsyncActionFilter
        {
            private readonly ITodoRepository _todoRepository;
            private readonly IStringLocalizer<SharedResources> _localizer;

            public TodoExistsFilter(ITodoRepository todoRepository, IStringLocalizer<SharedResources> localizer) => (_todoRepository, _localizer) = (todoRepository, localizer);

            #region snippet_BeforeExecuted

            public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
            {
                try
                {
                    var id = context.ActionArguments["id"] as string;
                    var todo = await _todoRepository.GetByIdAsync(id);
                    var response = new Response<IActionResult>() { Status = false, Message = _localizer["TodoNotFound"].Value };

                    if (todo is null) { context.Result = new NotFoundObjectResult(response); return; }

                    await next();
                }
                catch (FormatException)
                {
                    var response = new Response<IActionResult>() { Status = false, Message = _localizer["ObjectIdIsValid"].Value };
                    context.Result = new BadRequestObjectResult(response);
                    return;
                }
            }

            #endregion
        }
    }
}
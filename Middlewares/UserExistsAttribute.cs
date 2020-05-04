using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Localization;
using TodoApiNet.Models;
using TodoApiNet.Repositories;

namespace TodoApiNet.Middlewares
{
    public class UserExistsAttribute : TypeFilterAttribute
    {
        public UserExistsAttribute() : base(typeof(UserExistsFilter)) { }

        private class UserExistsFilter : IAsyncActionFilter
        {
            private readonly IUserRepository _userRepository;
            private readonly IStringLocalizer<SharedResources> _localizer;

            public UserExistsFilter(IUserRepository userRepository, IStringLocalizer<SharedResources> localizer) => (_userRepository, _localizer) = (userRepository, localizer);

            #region snippet_BeforeExecuted

            public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
            {
                try
                {
                    var id = context.ActionArguments["id"] as string;
                    var user = await _userRepository.GetByIdAsync(id);
                    var  response = new Response<IActionResult>() { Status = false, Message = _localizer["UserNotFound"].Value };

                    if (user is null) { context.Result = new NotFoundObjectResult(response); return; }

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
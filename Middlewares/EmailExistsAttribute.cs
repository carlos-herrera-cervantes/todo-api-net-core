using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Localization;
using TodoApiNet.Extensions;
using TodoApiNet.Models;
using TodoApiNet.Repositories;

namespace TodoApiNet.Middlewares
{
    public class EmailExistsAttribute : TypeFilterAttribute
    {
        public EmailExistsAttribute() : base(typeof(EmailExistsFilter)) { }

        private class EmailExistsFilter : IAsyncActionFilter
        {
            private readonly IUserRepository _userRepository;
            private readonly IStringLocalizer<SharedResources> _localizer;

            public EmailExistsFilter(IUserRepository userRepository, IStringLocalizer<SharedResources> localizer) => (_userRepository, _localizer) = (userRepository, localizer);

            #region snippet_BeforeExecuted

            public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
            {
                var user = context.ActionArguments["user"] as User;
                var filter = QueryObject<User>.CreateObjectQuery($"Email-{user.Email}");
                var findedUser = await _userRepository.GetOneAsync(filter);
                var response = new Response<IActionResult>() { Status = false, Message = _localizer["EmailAlreadyExists"].Value };

                if (findedUser != null)
                {
                    context.Result = new BadRequestObjectResult(response);
                    return;
                }

                await next();
            }

            #endregion
        }
    }
}
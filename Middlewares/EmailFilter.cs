using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Localization;
using TodoApiNet.Models;
using TodoApiNet.Repositories;

namespace TodoApiNet.Middlewares
{
    public class EmailFilter : IAsyncActionFilter
    {
        private readonly IUserRepository _userRepository;
        private readonly IStringLocalizer<SharedResources> _localizer;

        public EmailFilter(IUserRepository userRepository, IStringLocalizer<SharedResources> localizer)
        {
            _userRepository = userRepository;
            _localizer = localizer;
        }

        #region snippet_BeforeExecuted

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var user = context.ActionArguments["user"] as User;
            var findedUser = await _userRepository.GetByEmailAsync(user.Email);

            if (findedUser != null) 
            { 
                var objectResult = new { Message = _localizer["EmailAlreadyExists"].Value };
                context.Result = new BadRequestObjectResult(objectResult);
                return; 
            }

            await next();
        }

        #endregion
    }
}
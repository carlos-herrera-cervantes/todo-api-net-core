using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using TodoApiNet.Models;
using TodoApiNet.Repositories;

namespace TodoApiNet.Middlewares
{
    public class EmailFilter : IAsyncActionFilter
    {
        private readonly IUserRepository _userRepository;

        public EmailFilter(IUserRepository userRepository) => _userRepository = userRepository;

        #region snippet_BeforeExecuted

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var user = context.ActionArguments["user"] as User;
            var findedUser = await _userRepository.GetByEmailAsync(user.Email);

            if (findedUser != null) 
            { 
                var objectResult = new { Message = "Ya existe un usuario registrado con ese email." };
                context.Result = new BadRequestObjectResult(objectResult);
                return; 
            }

            await next();
        }

        #endregion
    }
}
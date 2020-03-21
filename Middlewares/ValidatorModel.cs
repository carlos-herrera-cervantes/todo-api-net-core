using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using TodoApiNet.Models;

namespace TodoApiNet.Middlewares
{
    public class ValidatorModel : IAsyncActionFilter
    {
        #region snippet_BeforeExecuted

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var user = context.ActionArguments["user"] as User;

            if (!context.ModelState.IsValid) 
            {
                context.Result = new BadRequestObjectResult("");
                return;
            }

            await next();
        }

        #endregion
    }
}
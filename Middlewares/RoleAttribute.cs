using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Localization;
using TodoApiNet.Models;
using TodoApiNet.Repositories;

namespace TodoApiNet.Middlewares
{
    public class RoleAttribute : TypeFilterAttribute
    {

        public static string[] Roles;

        public RoleAttribute(string[] roles) : base(typeof(RoleFilter)) 
        {
            Roles = roles;
        }

        private class RoleFilter : IAsyncActionFilter
        {
            private readonly IStringLocalizer<SharedResources> _localizer;
            private readonly IAccessTokenRepository _accessTokenRepository;
            
            public RoleFilter(IStringLocalizer<SharedResources> localizer, IAccessTokenRepository accessTokenRepository) =>
                (_localizer, _accessTokenRepository) = (localizer, accessTokenRepository);

            #region snippet_BeforeExecute
            
            public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
            {
                var bearerToken = context.HttpContext.Request.Headers["Authorization"].ToString();
                var token = bearerToken.Split(" ")[1];
                var tokenSession = await _accessTokenRepository.GetOneAsync(token);
                var role = RoleAttribute.Roles.Where(role => role == tokenSession.Role).FirstOrDefault();

                if (role is null)
                {
                    var response = new Response<IActionResult>() { Status = false, Message = _localizer["InvalidPermissions"].Value };
                    context.Result = new BadRequestObjectResult(response);
                    return;
                }
                
                await next();
            }

            #endregion
        }
    }
}
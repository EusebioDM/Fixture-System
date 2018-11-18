using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using EirinDuran.IServices.Infrastructure_Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;

namespace EirinDuran.WebApi.Middlewares
{
    public class LogginMiddleware : IAsyncAuthorizationFilter
    {   
        public Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            ILogger logger = GetLogger(context);
            string userName = GetUserName(context);
            string actionName = GetActionName(context);
            
            logger.Log(userName, actionName);

            return Task.CompletedTask;
        }

        private static ILogger GetLogger(AuthorizationFilterContext context)
        {
            return context.HttpContext.RequestServices.GetService(typeof(ILogger)) as ILogger;
        }

        private string GetUserName(AuthorizationFilterContext context)
        {
            ClaimsIdentity identity = context.HttpContext.User.Identity as ClaimsIdentity;
            List<Claim> claims = identity.Claims.ToList();
            return claims.Where(c => c.Type == "UserName").Select(c => c.Value).SingleOrDefault();
        }

        private string GetActionName(AuthorizationFilterContext context)
        {
            ControllerActionDescriptor controllerActionDescriptor = context.ActionDescriptor as ControllerActionDescriptor;
            string controllerName = controllerActionDescriptor?.ControllerName;
            return controllerActionDescriptor?.ActionName;
        }
    }
}
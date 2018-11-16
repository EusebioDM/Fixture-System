using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace EirinDuran.WebApi.Middlewares
{
    public class LogginMiddleware
    {
        private readonly RequestDelegate next;

        public LogginMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var identity = context.User.Identity as ClaimsIdentity;
            List<Claim> claims = identity.Claims.ToList();
            throw new Exception();
            string userName = claims.Where(c => c.Type == "UserName").Select(c => c.Value).SingleOrDefault();
            string password = claims.Where(c => c.Type == "Password").Select(c => c.Value).SingleOrDefault();
            
            // Call the next delegate/middleware in the pipeline
            await next(context);
        }
    }
}
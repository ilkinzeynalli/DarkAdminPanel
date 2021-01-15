using DarkAdminPanel.WebUI.Extensions;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DarkAdminPanel.WebUI.Middlewares
{
    public class HttpRequestHeaderMiddleware
    {
        private readonly RequestDelegate _next;

        public HttpRequestHeaderMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            var JWToken = httpContext.Session.GetJson<string>("JWToken");

            if (!string.IsNullOrEmpty(JWToken))
            {
                httpContext.Request.Headers.Add("Authorization", "Bearer " + JWToken);
            }

            await _next(httpContext); // calling next middleware
        }
    }
}

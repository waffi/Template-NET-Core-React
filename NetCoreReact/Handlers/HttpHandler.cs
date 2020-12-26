using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using NetCoreReact.Models.Response;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace NetCoreReact.Handlers
{
    public class HttpHandler
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<HttpHandler> _logger;

        public HttpHandler(RequestDelegate next, ILogger<HttpHandler> logger)
        {
            _logger = logger;
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {

            if (httpContext.Request.Path.StartsWithSegments("/api"))
            {
                try
                {
                    await _next.Invoke(httpContext);
                }
                catch (Exception ex)
                {
                    await HandleExceptionAsync(httpContext, ex);
                }

                if (!httpContext.Response.HasStarted)
                {
                    httpContext.Response.ContentType = "application/json";

                    var response = new Response((HttpStatusCode)httpContext.Response.StatusCode);

                    await httpContext.Response.WriteAsync(JsonConvert.SerializeObject(response));
                }
            }
            else
            {
                await _next(httpContext);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            return context.Response.WriteAsync(new Response(HttpStatusCode.InternalServerError, exception).ToString());
        }
    }

}

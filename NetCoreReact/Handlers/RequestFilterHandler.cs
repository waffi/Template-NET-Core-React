using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
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
    public class RequestFilterHandler : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                context.Result = new BadRequestObjectResult(context.ModelState);
            }
        }

        internal static IActionResult CustomErrorResponse(ActionContext context)
        {
            var data = context.ModelState
             .Where(modelError => modelError.Value.Errors.Count > 0)
             .Select(modelError => new
             {
                 Field = modelError.Key,
                 Description = modelError.Value.Errors.FirstOrDefault().ErrorMessage
             }).ToList();

            return new BadRequestObjectResult(new Response(HttpStatusCode.BadRequest, data));
        }
    }

}

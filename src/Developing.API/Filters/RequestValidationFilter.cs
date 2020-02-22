using System.Linq;
using Developing.API.Models;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Developing.API.Filters
{
    public sealed class RequestValidationFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (!filterContext.ModelState.IsValid)
            {
                var errors = filterContext.ModelState.Values
                    .SelectMany(v => v.Errors)
                    .ToList();

                filterContext.Result = new BadRequestError(errors);
            }
        }
    }
}

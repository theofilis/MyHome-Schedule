using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;

namespace MyHome.Dashboard.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class PaginationAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            // do something before the action executes
        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            var result = context.Result as ObjectResult;

            if (result != null)
            {
                dynamic items = Convert.ChangeType(result.Value, result.DeclaredType);
                context.HttpContext.Response.Headers.Add("x-total-count", items.TotalCount.ToString());
                context.HttpContext.Response.Headers.Add("x-page-size", items.PageSize.ToString());
                context.HttpContext.Response.Headers.Add("x-current-page", items.CurrentPage.ToString());
                context.HttpContext.Response.Headers.Add("x-total-pages", items.TotalPages.ToString());
                context.HttpContext.Response.Headers.Add("x-has-next", items.HasNext.ToString());
                context.HttpContext.Response.Headers.Add("x-has-previous", items.HasPrevious.ToString());
            }
        }
    }
}
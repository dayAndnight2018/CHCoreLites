using ExtendsLite;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Collections.Generic;

namespace WebLite.Filters
{
    public class ModelValidationFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.ModelState.IsValid)
            {
                return;
            }
            
            List<string> errors = new List<string>();
            foreach (var item in context.ModelState.Keys)
            {
                var value = context.ModelState[item];
                if (value.Errors.HasValue())
                {
                    errors.Add(string.Format("{0}:{1}", item,value.Errors[0].ErrorMessage));
                }

            }
            context.Result = new RestResult(StatusCode.请求错误, "The request is invalid", errors);
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using WebLite.Exceptions;

namespace WebLite.Filters
{
    public class ExceptionHandlerFilterAttribute : ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            var exception = context.Exception;
            RestResult result = new RestResult();

            if (exception is BadRequestException)
            {
                result.StatusCode = StatusCode.请求错误;
                result.ReasonPhrase = "Invalid request!";
                result.Data = null;
            }
            else if (exception is NotFoundException)
            {
                result.StatusCode = StatusCode.路由错误;
                result.ReasonPhrase = "Check the url please!";
                result.Data = null;
            }
            else if (exception is UnAuthenticationException)
            {
                result.StatusCode = StatusCode.身份异常;
                result.ReasonPhrase = "Try again with token!";
                result.Data = null;
            }
            else if(exception is UnAuthorizationException)
            {
                result.StatusCode = StatusCode.鉴权异常;
                result.ReasonPhrase = "You have no plenty priviledges!";
                result.Data = null;
            }
            else
            {
                result.StatusCode = StatusCode.服务器异常;
                result.ReasonPhrase = "The server comes across an error!";
                result.Data = null;
            }

            context.Result = new ObjectResult(result);
        }
    }
}

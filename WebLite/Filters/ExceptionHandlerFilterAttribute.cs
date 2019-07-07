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
            RestResult result;

            if (exception is BadRequestException)
            {
                result = new RestResult(WebLite.StatusCode.请求错误, reasonPhrase:"Bad request!");
            }
            else if (exception is NotFoundException)
            {
                result = new RestResult(WebLite.StatusCode.路由错误, reasonPhrase:"Check the url please!");
            }
            else if (exception is UnAuthenticationException)
            {
                result = new RestResult(WebLite.StatusCode.身份异常, reasonPhrase: "UnAuthentication request!");
            }
            else if(exception is UnAuthorizationException)
            {
                result = new RestResult(WebLite.StatusCode.鉴权异常, reasonPhrase: "UnAuthorized request!");
            }
            else
            {
                result = new RestResult(WebLite.StatusCode.服务器异常, reasonPhrase: "The server comes across an error!");
            }

            context.Result =result;
        }
    }
}

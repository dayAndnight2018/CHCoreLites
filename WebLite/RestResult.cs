using ExtendsLite;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace WebLite
{
    public enum StatusCode
    {
        成功 = 200,
        暂时重定向 = 301,
        永久重定向 = 302,
        请求错误 = 400,
        身份异常 = 401,
        鉴权异常 = 403,
        路由错误 = 404,
        服务器异常 = 500,
        服务器维护 = 503
    }

    public class RestResult : JsonResult
    {
        public RestResult(StatusCode statusCode, string reasonPhrase = null, dynamic data = null) : base(
            new {
            StatusCode = statusCode.IntValue(),
            ReasonPhrase = reasonPhrase ?? statusCode.StringValue(),
            Data = data
        })
        {
            StatusCode = statusCode.IntValue();
        }

        public RestResult(TransResult result): base(
            new
            {
                StatusCode = result.StatusCode.IntValue(),
                ReasonPhrase = result.ReasonPhrase ?? result.StatusCode.StringValue(),
                Data = result.Data
            } )
        {
            StatusCode = result.StatusCode.IntValue();
        }

        public override Task ExecuteResultAsync(ActionContext context)
        {
            return base.ExecuteResultAsync(context);
        }
    }
}

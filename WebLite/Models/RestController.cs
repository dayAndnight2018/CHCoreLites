
using Microsoft.AspNetCore.Mvc;

namespace WebLite.Models
{
    public abstract class RestController : Controller
    {
        /// <summary>
        /// 200
        /// </summary>
        protected IActionResult Success(string reasonPhrase =null, dynamic data = null)
        {
            return new RestResult(WebLite.StatusCode.成功, reasonPhrase ?? "Request success!", data);
        }

        /// <summary>
        /// 400
        /// </summary>
        protected IActionResult BadRequest(string reasonPhrase = null, dynamic data = null)
        {
            return new RestResult(WebLite.StatusCode.请求错误, reasonPhrase ?? "Bad request!", data);
        }

        /// <summary>
        /// 404
        /// </summary>
        protected IActionResult  NotFound(string reasonPhrase = null, dynamic data = null)
        {
            return new RestResult(WebLite.StatusCode.路由错误, reasonPhrase ?? "Not found!", data);
        }

        /// <summary>
        /// 301
        /// </summary>
        protected IActionResult TemperRedirect(string reasonPhrase = null, dynamic data = null)
        {
            return new RestResult(WebLite.StatusCode.暂时重定向, reasonPhrase ?? "Temper redirect!", data);
        }

        /// <summary>
        /// 302
        /// </summary>
        protected IActionResult PermanentRedirect(string reasonPhrase = null, dynamic data = null)
        {
            return new RestResult(WebLite.StatusCode.永久重定向, reasonPhrase ?? "Permanent redirect!", data);
        }

        /// <summary>
        /// 401
        /// </summary>
        protected IActionResult UnAuthentication(string reasonPhrase = null, dynamic data = null)
        {
            return new RestResult(WebLite.StatusCode.身份异常, reasonPhrase ?? "UnAuthentication request!", data);
        }

        /// <summary>
        /// 403
        /// </summary>
        protected IActionResult UnAuthorized(string reasonPhrase = null, dynamic data = null)
        {
            return new RestResult(WebLite.StatusCode.鉴权异常, reasonPhrase ?? "UnAuthorized request!", data);
        }

        /// <summary>
        /// 500
        /// </summary>
        protected IActionResult ServerException(string reasonPhrase = null, dynamic data = null)
        {
            return new RestResult(WebLite.StatusCode.服务器异常, reasonPhrase ?? "The server comes across an error!", data);
        }

        /// <summary>
        /// 503
        /// </summary>
        protected IActionResult ServerRepairing(string reasonPhrase = null, dynamic data = null)
        {
            return new RestResult(WebLite.StatusCode.服务器维护, reasonPhrase ?? "The server is under repairing!", data);
        }

        /// <summary>
        /// 包装类型数据
        /// </summary>
        protected IActionResult TransObjectResult(TransResult result)
        {
            return new RestResult(result);
        }
    }
}

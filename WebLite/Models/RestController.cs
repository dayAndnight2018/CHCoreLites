using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Mvc;

namespace WebLite.Models
{
    public abstract class RestController : Controller
    {
        protected IActionResult Success(string reasonPhrase =null, dynamic data = null)
        {
            return new RestResult(WebLite.StatusCode.成功, reasonPhrase ?? "Request success!", data);
        }

        protected IActionResult BadRequest(string reasonPhrase = null, dynamic data = null)
        {
            return new RestResult(WebLite.StatusCode.请求错误, reasonPhrase ?? "Bad request!", data);
        }

        protected IActionResult  NotFound(string reasonPhrase = null, dynamic data = null)
        {
            return new RestResult(WebLite.StatusCode.路由错误, reasonPhrase ?? "Not found!", data);
        }

        protected IActionResult TemperRedirect(string reasonPhrase = null, dynamic data = null)
        {
            return new RestResult(WebLite.StatusCode.暂时重定向, reasonPhrase ?? "Temper redirect!", data);
        }

        protected IActionResult PermanentRedirect(string reasonPhrase = null, dynamic data = null)
        {
            return new RestResult(WebLite.StatusCode.永久重定向, reasonPhrase ?? "Permanent redirect!", data);
        }

        protected IActionResult UnAuthentication(string reasonPhrase = null, dynamic data = null)
        {
            return new RestResult(WebLite.StatusCode.身份异常, reasonPhrase ?? "UnAuthentication request!", data);
        }

        protected IActionResult UnAuthorized(string reasonPhrase = null, dynamic data = null)
        {
            return new RestResult(WebLite.StatusCode.鉴权异常, reasonPhrase ?? "UnAuthorized request!", data);
        }

        protected IActionResult ServerException(string reasonPhrase = null, dynamic data = null)
        {
            return new RestResult(WebLite.StatusCode.服务器异常, reasonPhrase ?? "The server comes across an error!", data);
        }

        protected IActionResult ServerRepairing(string reasonPhrase = null, dynamic data = null)
        {
            return new RestResult(WebLite.StatusCode.服务器维护, reasonPhrase ?? "The server is under repairing!", data);
        }
    }
}

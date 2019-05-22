using System;
using System.Collections.Generic;
using System.Text;

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

    public class RestResult
    {
        public RestResult(StatusCode statusCode, string reasonPhrase, dynamic data = null)
        {
            StatusCode = statusCode;
            ReasonPhrase = reasonPhrase;
            Data = data;
        }

        public RestResult()
        {
        }

        public StatusCode StatusCode { get; set; }

        public string ReasonPhrase { get; set; }

        public dynamic Data { get; set; }

    }
}

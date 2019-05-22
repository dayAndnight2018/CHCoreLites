using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebLite.Exceptions
{
    public class GlobalException : IExceptionFilter
    {
        private readonly IHostingEnvironment env;
        private readonly ILogger<GlobalException> logger;
        public GlobalException(IHostingEnvironment env, ILogger<GlobalException> logger)
        {
            this.env = env;
            this.logger = logger;
        }

        void IExceptionFilter.OnException(ExceptionContext context)
        {

            context.Result = new JsonResult(new
            {
                StatusCode = 500,
                ReasonPhrase = "服务器错误",
            });

            logger.LogWarning("异常信息："+context.Exception.Message);
           
            logger.LogWarning("异常位置：" + context.Exception.StackTrace);

        }
    }
}

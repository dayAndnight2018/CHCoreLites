using AspectCore.DynamicProxy;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CacheLite
{
    public class LogAspect : AbstractInterceptor
    {
        private static readonly string successTemplate = "[{0}]--function: [{1}]--params: [{2}] proceed succeed, results: [{3}]";
        private static readonly string errorTemplate = "[{0}]--function: [{1}]--params: [{2}] proceed failed, exception: [{3}]";
        private readonly ILogger logger;
        public LogAspect(ILogger logger)
        {
            this.logger = logger;
        }

        public override async Task Invoke(AspectContext context, AspectDelegate next)
        {
            var method = context.ImplementationMethod;
            try
            {
                await next(context);
                var log = string.Format(
                    successTemplate,
                    DateTime.Now.ToString("yyyy/MM/dd T HH:mm:ss"),
                    method.GetBaseDefinition(), string.Join(", ", context.Parameters.Select(a => a.ToString())),
                    context.ReturnValue
                    );
                logger.LogInformation(log);
                Console.WriteLine(log);
            }
            catch (Exception ex)
            {
                var log = string.Format(
                   errorTemplate,
                   DateTime.Now.ToString("yyyy/MM/dd T HH:mm:ss"),
                   method.GetBaseDefinition(), string.Join(", ", context.Parameters.Select(a => a.ToString())),
                    ex.Message
                   );
                logger.LogError(log);
                Console.WriteLine(log);
            }
        }
    }
}

using AspectCore.DynamicProxy;
using AspectCore.Injector;
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
        private static readonly string successTemplate = "function: [{0}] proceed succeed, params: [{1}] , results: [{2}]";
        private static readonly string errorTemplate = "function: [{0}] proceed failed, params: [{1}] , exception: [{2}]";
        public LogAspect()
        {

        }

        [FromContainer]
        public ILogger<LogAspect> Logger
        {
            get; set;
        }

        public override async Task Invoke(AspectContext context, AspectDelegate next)
        {
            var method = context.ImplementationMethod;
            try
            {
                await next(context);
                var log = string.Format(
                    successTemplate,
                    method.GetBaseDefinition(), 
                    string.Join(", ", context.Parameters.Select(a => a.ToString())),
                    context.ReturnValue
                    );
                Logger.LogWarning(log);
            }
            catch (Exception ex)
            {
                var log = string.Format(
                   errorTemplate,
                   method.GetBaseDefinition(), 
                   string.Join(", ", context.Parameters.Select(a => a.ToString())),
                    ex.Message
                   );
                Logger.LogWarning(log);
            }
        }
    }
}

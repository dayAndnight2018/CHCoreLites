using AspectCore;
using AspectCore.DynamicProxy;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CacheLite
{
    public class CacheAttribute : AbstractInterceptorAttribute
    {
        public CacheAttribute()
        {

        }

        public override async Task Invoke(AspectContext context, AspectDelegate next)
        {
            //获取自定义缓存键，这个和Memory内存缓存是一样的，不细说
            var cacheKey = context.GetMethodFullName();

            //核心1：注意这里和之前不同，是获取的string值，之前是object
            var cacheValue = await RedisHelper.GetStringAsync(cacheKey);

            //将当前获取到的缓存值，赋值给当前执行方法
            var type = context.ImplementationMethod.ReturnType;

            if (cacheValue != null)
            {
                object response;

                dynamic temp = JsonConvert.DeserializeObject(cacheValue, type);
                response = Task.FromResult(temp);
                context.ReturnValue = response;
                return;
            }

            //去执行当前的方法
            await next(context);

            // 核心5：将获取到指定的response 和特性的缓存时间，进行set操作
            await RedisHelper.SetObjectAsync(cacheKey, context.ReturnValue);

        }
    }
}

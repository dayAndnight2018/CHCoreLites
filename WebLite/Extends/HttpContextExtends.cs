using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebLite.Extends
{
    /// <summary>
    /// 基于上下文的扩展
    /// </summary>
    public static class HttpContextExtends
    {
        /// <summary>
        /// 获取客户端IP
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static string GetUserIp(this HttpContext context)
        {
            var ip = context.Request.Headers["X-Forwarded-For"].FirstOrDefault();
            if (string.IsNullOrWhiteSpace(ip))
            {
                ip = context.Connection.RemoteIpAddress.ToString();
            }
            return ip;
        }

        /// <summary>
        /// 获取String Session
        /// </summary>
        /// <param name="context"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetSessionString(this HttpContext context, string key)
        {
            byte[] bytes;
            var result = context.Session.TryGetValue(key, out bytes);
            if (!result)
            {
                return null;
            }
            return Encoding.UTF8.GetString(bytes);
        }

        /// <summary>
        /// 设置String Session
        /// </summary>
        /// <param name="context"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool SetSessionString(this HttpContext context, string key, string value)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(value);
            context.Session.Set(key, bytes);
            return true;
        }

        /// <summary>
        /// 更新object格式session
        /// </summary>
        /// <param name="context"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool SetSessionObject(this HttpContext context, string key, object value)
        {
            return SetSessionString(context, key, JsonConvert.SerializeObject(value));
        }

        /// <summary>
        /// 获取object数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="context"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static T GetSessionObject<T>(this HttpContext context, string key)
        {
            var valueString = GetSessionString(context, key);

            return valueString == null ? default(T) : JsonConvert.DeserializeObject<T>(valueString);
        }

        /// <summary>
        /// 删除session数据
        /// </summary>
        /// <param name="context"></param>
        /// <param name="key"></param>
        public static void RemoveSessionKey(this HttpContext context, string key)
        {
             context.Session.Remove(key);
        }
    }
}

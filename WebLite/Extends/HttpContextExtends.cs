using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebLite.Extends
{
    public static class HttpContextExtends
    {
        public static string GetUserIp(this HttpContext context)
        {
            var ip = context.Request.Headers["X-Forwarded-For"].FirstOrDefault();
            if (string.IsNullOrWhiteSpace(ip))
            {
                ip = context.Connection.RemoteIpAddress.ToString();
            }

            return ip;
        }

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

        public static bool SetSessionString(this HttpContext context, string key, string value)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(value);
            context.Session.Set(key, bytes);
            return true;
        }

        public static bool SetSessionObject(this HttpContext context, string key, object value)
        {
            return SetSessionString(context, key, JsonConvert.SerializeObject(value));
        }

        public static T GetSessionObject<T>(this HttpContext context, string key)
        {
            var valueString = GetSessionString(context, key);

            return valueString == null ? default(T) : JsonConvert.DeserializeObject<T>(valueString);
        }

        public static void RemoveSessionKey(this HttpContext context, string key)
        {
             context.Session.Remove(key);
        }
    }
}

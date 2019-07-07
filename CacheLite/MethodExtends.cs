using AspectCore.DynamicProxy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace CacheLite
{
    public static class MethodExtends
    {
        public static string GetMethodFullName(this AspectContext context)
        {
            var method = context.ImplementationMethod;
            //获取返回类型全称
            var returnType = method.ReturnType.FullName;
            //获取方法全名
            var methodName = method.GetBaseDefinition();
            //获取参数
            var arguments = context.Parameters;

            string key = $"{returnType}:{methodName}" + string.Join(":", arguments.Select(GetArgumentValue));

            return key;
        }

        private static string GetArgumentValue(object arg)
        {
            //需要完善
            if (arg is int || arg is long || arg is string)
                return arg.ToString();

            if (arg is DateTime)
                return ((DateTime)arg).ToString("yyyyMMddHHmmss");

            return string.Empty;
        }
    }
}

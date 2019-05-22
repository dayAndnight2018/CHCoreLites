using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Text;

namespace ExtendsLite
{
    public static class EnumExtends
    {
        /// <summary>
        /// 获取枚举值的字符串表示
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string StringValue(this Enum value)
        {
            var type = value.GetType();
            var name = Enum.GetName(type, value);
            var attribute = type.GetField(name).GetCustomAttribute<DisplayAttribute>();
            return attribute == null ? name : attribute.Name;
        }

        /// <summary>
        /// 获取int值
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static int IntValue(this Enum value)
        {
            var type = value.GetType();
            var values = Enum.GetValues(type);
            return (int)values.GetValue(Array.BinarySearch(values, value));
        }
    }
}

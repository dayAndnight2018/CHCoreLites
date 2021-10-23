using System;
using System.Text;

namespace EncryptLite
{
    public static class Base64Generator
    {
        /// <summary>
        /// 默认使用utf-8编码
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ToBase64String(this string str)
        {
            return str.ToBase64String(Encoding.UTF8);
        }

        /// <summary>
        /// 默认使用utf-8解码
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string FromBase64String(this string str)
        {
            return str.FromBase64String(Encoding.UTF8);
        }

        /// <summary>
        /// 按照指定的编码格式转换base64
        /// </summary>
        /// <param name="encode"></param>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ToBase64String(this string str, Encoding encode)
        {
            try
            {
                return Convert.ToBase64String(encode.GetBytes(str));
            }
            catch
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// 使用指定编码格式从base64转回来
        /// </summary>
        /// <param name="encode"></param>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string FromBase64String(this string str, Encoding encode)
        {
            try
            {
                return encode.GetString(Convert.FromBase64String(str));
            }
            catch
            {
                return string.Empty;
            }
        }
    }
}
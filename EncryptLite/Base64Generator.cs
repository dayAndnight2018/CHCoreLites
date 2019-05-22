using System;
using System.Collections.Generic;
using System.Text;

namespace EncryptLite
{
    public class Base64Generator
    {
        /// <summary>
        /// 默认使用utf-8编码
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string EncodeBase64(string str)
        {
            return EncodeBase64(Encoding.UTF8, str);
        }

        /// <summary>
        /// 默认使用utf-8解码
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string DecodeBase64(string str)
        {
            return DecodeBase64(Encoding.UTF8, str);
        }

        /// <summary>
        /// 按照指定的编码格式转换base64
        /// </summary>
        /// <param name="encode"></param>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string EncodeBase64(Encoding encode, string str)
        {
            byte[] bytes = encode.GetBytes(str);
            try
            {
                return Convert.ToBase64String(bytes);
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
        public static string DecodeBase64(Encoding encode, string str)
        {
            byte[] bytes = Convert.FromBase64String(str);
            try
            {
                return encode.GetString(bytes);
            }
            catch
            {
                return string.Empty;
            }
        }
    }
}

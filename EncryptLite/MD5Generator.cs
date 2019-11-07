using RecifyLite;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace EncryptLite
{
    /// <summary>
    /// MD5生成器
    /// </summary>
    public class MD5Generator
    {
        /// <summary>
        /// 根据输入生成MD5
        /// </summary>
        /// <param name="input">输入字符串</param>
        /// <returns>MD5处理结果</returns>
        public static string Compute(string input)
        {
            MD5 md = MD5.Create();
            byte[] before = Encoding.UTF8.GetBytes(input);
            byte[] after = md.ComputeHash(before);

            StringBuilder sb = new StringBuilder();
            foreach (var item in after)
            {
                sb.Append(item.ToString("x2"));
            }
            return sb.ToString();
        }

        /// <summary>
        /// 加盐计算MD5值
        /// </summary>
        /// <param name="input"></param>
        /// <param name="salt"></param>
        /// <returns></returns>
        public static string Compute(string input, string salt)
        {
            MD5 md = MD5.Create();
            if (salt == null)
            {
                salt = RecifyCodeGenerator.GenerateRandomString(8);
            } 

            byte[] before = Encoding.UTF8.GetBytes(input + salt);
            byte[] after = md.ComputeHash(before);

            StringBuilder sb = new StringBuilder();
            foreach (var item in after)
            {
                sb.Append(item.ToString("x2"));
            }
            return sb.ToString();
        }
    }
}

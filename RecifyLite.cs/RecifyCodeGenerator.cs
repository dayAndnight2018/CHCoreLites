using System;
using System.Collections.Generic;
using System.Text;

namespace RecifyLite.cs
{
    public class RecifyCodeGenerator
    {
        /// <summary>
        /// 生成指定长的随机数
        /// </summary>
        /// <param name="len">长度</param>
        /// <returns>指定长度的随机数</returns>
        public static String GenerateRandomNumberString(int len)
        {
            StringBuilder sb = new StringBuilder();
            string numbers = "0123456789";
            Random rand = new Random();
            for (int i = 0; i < len; i++)
            {
                sb.Append(numbers[rand.Next(numbers.Length)]);
            }
            return sb.ToString();
        }

        /// <summary>
        /// 产生指定长度的数字字母混合字符串
        /// </summary>
        /// <param name="size">字符串长度</param>
        /// <returns></returns>
        public static string GenerateRandomString(int len)
        {
            StringBuilder sb = new StringBuilder();
            string alphabet = "abcdefghijklmnopqrstuvwyxzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var rand = new Random();
            for (int i = 0; i < len; i++)
            {
                sb.Append(alphabet[rand.Next(alphabet.Length)]);
            }
            return sb.ToString();
        }

        /// <summary>
        /// 生成指定长度的字母字符串
        /// </summary>
        /// <param name="len">长度</param>
        /// <returns>随机字符串</returns>
        public static string GenerateRandomLetterString(int len)
        {
            StringBuilder sb = new StringBuilder();
            string alphabet = "abcdefghijklmnopqrstuvwyxz";
            var rand = new Random();
            for (int i = 0; i < len; i++)
            {
                sb.Append(alphabet[rand.Next(alphabet.Length)]);
            }
            return sb.ToString();
        }
    }
}

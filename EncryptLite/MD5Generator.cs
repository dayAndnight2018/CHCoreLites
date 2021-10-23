using RecifyLite;
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
        /// 生成MD5
        /// </summary>
        public static string Compute(string input)
        {
            var after = MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(input));
            var sb = new StringBuilder();
            foreach (var item in after)
            {
                sb.Append(item.ToString("x2"));
            }
            return sb.ToString();
        }

        /// <summary>
        /// 加盐计算MD5值
        /// </summary>
        public static string Compute(string input, string salt)
        {
            if (string.IsNullOrWhiteSpace(salt))
            {
                salt = RecifyCodeGenerator.GenerateRandomString(8);
            }
            return Compute(input + salt);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace EncryptLite
{
    /// <summary>
    /// 微信常用加密方式(默认采用UTF-8编码)
    /// </summary>
    public class SH1Generator
    {
        public static String Encrypt(string input, Encoding encoding = null)
        {
            try
            {
                if (encoding == null)
                {
                    encoding = Encoding.UTF8;
                }
                SHA1 sh = SHA1.Create();
                var bytes = encoding.GetBytes(input);
                var after = sh.ComputeHash(bytes);
                StringBuilder sb = new StringBuilder();
                foreach (var item in after)
                {
                    sb.Append(item.ToString("x2"));
                }
                return sb.ToString();
            }
            catch
            {
                return String.Empty;
            }
            
        }
    }
}

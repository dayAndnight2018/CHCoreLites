using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;

namespace EncryptLite
{
    public class SHAGenerator
    {
        private const int SALT_SIZE = 128 / 8;          // 128 bits
        private const int SUBKEY_SIZE = 256 / 8;        // 256 bits
        private const int PBKDF2_ITERATION = 1000;      // default for Rfc2898DeriveBytes.

        /// <summary>
        /// 产生密码的哈希算法
        /// </summary>
        /// <param name="password">The password to hash.</param>
        /// <returns>Salted hash result.</returns>
        public static string ComputePassword(string password)
        {
            var rng = RandomNumberGenerator.Create();
            var salt = new byte[SALT_SIZE];
            rng.GetBytes(salt);

            byte[] subkey = Pbkdf2(password, salt);

            var hash = new byte[SALT_SIZE + SUBKEY_SIZE];
            Buffer.BlockCopy(salt, 0, hash, 0, SALT_SIZE);
            Buffer.BlockCopy(subkey, 0, hash, SALT_SIZE, SUBKEY_SIZE);

            return Convert.ToBase64String(hash);
        }

        /// <summary>
        /// 根据密码和哈希值进行验证
        /// </summary>
        /// <param name="password">The password to check.</param>
        /// <param name="hash">A hash of the correct password.</param>
        /// <returns>True if the password is correct. False otherwise.</returns>
        public static bool VerifyPassword(string password, string hash)
        {
            byte[] decoded = Convert.FromBase64String(hash);
            if (decoded.Length != SALT_SIZE + SUBKEY_SIZE)
            {
                return false;
            }

            byte[] salt = new byte[SALT_SIZE];
            Buffer.BlockCopy(decoded, 0, salt, 0, SALT_SIZE);

            byte[] expectedSubkey = new byte[SUBKEY_SIZE];
            Buffer.BlockCopy(decoded, SALT_SIZE, expectedSubkey, 0, SUBKEY_SIZE);

            byte[] actualSubkey = Pbkdf2(password, salt);
            return ByteArraysEqual(expectedSubkey, actualSubkey);
        }


        /// <summary>
        /// 根据盐值产生加密后的值
        /// </summary>
        /// <param name="password">密码</param>
        /// <param name="salt">盐</param>
        /// <returns></returns>
        private static byte[] Pbkdf2(string password, byte[] salt)
        {
            Rfc2898DeriveBytes pbkdf2 = new Rfc2898DeriveBytes(password, salt)
            {
                IterationCount = PBKDF2_ITERATION
            };
            return pbkdf2.GetBytes(SUBKEY_SIZE);
        }

        /// <summary>
        /// 完全比较两个数组
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
        private static bool ByteArraysEqual(byte[] a, byte[] b)
        {
            if (a == null && b == null)
            {
                return true;
            }
            if (a == null || b == null || a.Length != b.Length)
            {
                return false;
            }
            var areSame = true;
            for (var i = 0; i < a.Length; i++)
            {
                areSame &= (a[i] == b[i]);
            }
            return areSame;
        }
    }
}

using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Threading.Tasks;
using WebLite.Configurations;

namespace CacheLite
{
    public class RedisHelper
    {
        private readonly static ConnectionMultiplexer connection = ConnectionMultiplexer.Connect(ConfigurationManager.GetSection("redisConnectionString"));
        private readonly static TimeSpan expires = TimeSpan.FromMinutes(30);

        /// <summary>
        /// 添加缓存
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="expiry"></param>
        /// <returns></returns>
        public static async Task<bool> SetStringAsync(string key, string value, TimeSpan? expiry = null)
        {
            try
            {
                var db = connection.GetDatabase();
                await db.StringSetAsync(key, value, expiry??expires);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// 添加对象缓存
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="expiry"></param>
        /// <returns></returns>
        public static async Task<bool> SetObjectAsync(string key, Object value, TimeSpan? expiry = null)
        {
            var v = JsonConvert.SerializeObject(value);
            try
            {
                var db = connection.GetDatabase();
                await db.StringSetAsync(key, v, expiry?? expires);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// 获取缓存
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static async Task<string> GetStringAsync(string key)
        {
            try
            {
                var db = connection.GetDatabase();
                return await db.StringGetAsync(key);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// 获取对象缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public static async Task<T> GetObjectAsync<T>(string key) where T:class,new()
        {
            try
            {
                var db = connection.GetDatabase();
                var s =  await db.StringGetAsync(key);
                return JsonConvert.DeserializeObject<T>(s);
            }
            catch (Exception ex)
            {
                return default(T);
            }
        }

        /// <summary>
        /// 检查缓存存在性
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static async Task<bool> KeyExistsAsync(string key)
        {
            var db = connection.GetDatabase();
            return await db.KeyExistsAsync( key);
        }

        /// <summary>
        /// 删除缓存
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static async Task<bool> DeleteKeyAsync(string key)
        {
            var db = connection.GetDatabase();
            return await db.KeyDeleteAsync(key);
        }

        /// <summary>
        /// 设置计数器
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="expiry"></param>
        /// <returns></returns>
        public static async Task<long> SetStringIncrAsync(string key, long value = 1L, TimeSpan? expiry = null)
        {
            try
            {
                var db = connection.GetDatabase();
                var nubmer = await db.StringIncrementAsync(key, value);
                if (nubmer == 1)//只有第一次设置有效期（防止覆盖）
                    await db.KeyExpireAsync(key, expiry?? expires);//设置有效期
                return nubmer;
            }
            catch (Exception ex)
            {
                return -1;
            }
        }

        /// <summary>
        /// 获取计数器
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static async Task<long> GetStringIncrAsync(string key)
        {
            var value = await GetStringAsync(key);
            if (string.IsNullOrWhiteSpace(value))
            {
                return 0;
            }
            else
            {
                return long.Parse(value);
            }
        }
    }
}

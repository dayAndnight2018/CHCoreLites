using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using WebLite;
using WebLite.Attributes;

namespace CacheLite.MemoryCache
{
    [Singleton(typeof(ICacheOperations))]
    public class DictionaryMemoryCache : ICacheOperations
    {
        private readonly ConcurrentDictionary<string, object> cache = new ConcurrentDictionary<string, object>();

        /// <summary>
        /// 获取缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Key"></param>
        /// <returns></returns>
        public T Get<T>(string Key)
        {
            lock (cache)
            {
                cache.TryGetValue(Key, out object val);
                if (val != null && val is T)
                {
                    return (T)val;
                }
                return default(T);
            }
        }

        /// <summary>
        /// 根据Key 移除
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool Remove(string key)
        {
            lock (cache)
            {
                return cache.TryRemove(key, out object value);
            }
        }

        /// <summary>
        /// 清除所有
        /// </summary>
        public void Clear()
        {
            lock (cache)
            {
                cache.Clear();
            }
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="Data"></param>
        public bool Add<T>(string key, T data)
        {
            lock (cache)
            {
                return cache.TryAdd(key, data);
            }
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public bool UpdateOrAdd<T>(string key, T data)
        {
            lock (cache)
            {
                if (Contains(key))
                {
                    return Update(key, data);
                }
                else
                    return Add<T>(key, data);
            }
        }

        /// <summary>
        /// 添加或更新
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public bool AddOrUpdate<T>(string key, T data)
        {
            return UpdateOrAdd<T>(key, data);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public bool Update<T>(string key, T data)
        {
            lock (cache)
            {
                return cache.TryUpdate(key, data, Get<T>(key));
            }
        }

        /// <summary>
        /// 存在性
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool Contains(string key)
        {
            lock (cache)
            {
                return cache.ContainsKey(key);
            }
        }
    }
}

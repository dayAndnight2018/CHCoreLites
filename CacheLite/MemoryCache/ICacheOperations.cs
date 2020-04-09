using System;
using System.Collections.Generic;
using System.Text;

namespace CacheLite.MemoryCache
{
    public interface ICacheOperations
    {
        T Get<T>(string Key);

        bool Remove(string key);

        void Clear();

        bool Add<T>(string key, T data);

        bool UpdateOrAdd<T>(string key, T data);

        bool AddOrUpdate<T>(string key, T data);

        bool Update<T>(string key, T data);

        bool Contains(string key);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;

namespace ExtendsLite
{
    public static class CollectionExtends
    {
        /// <summary>
        /// 对指定的数组进行分页取值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <param name="page"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public static IQueryable<T> PageObject<T>(this IQueryable<T> query, int page, int size)
        {
            return query.Skip((page - 1) * size).Take(size);
        }

        /// <summary>
        /// 判断是否含有元素
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection"></param>
        /// <returns></returns>
        public static bool HasValue<T>(this IEnumerable<T> collection)
        {
            if (collection == null || collection.Count() == 0)
            {
                return false;
            }

            return true;
        }
    }
}

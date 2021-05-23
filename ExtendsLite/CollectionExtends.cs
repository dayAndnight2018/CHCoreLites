using System;
using System.Collections.Generic;
using System.Linq;
using ExtendsLite.Exceptions;

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
            if (page < 1 || size <= 0)
            {
                throw new InvalidPageParamException();
            }
            return query.Skip((page - 1) * size).Take(size);
        }

        /// <summary>
        /// 判断是否含有元素
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection"></param>
        /// <returns></returns>
        [Obsolete]
        public static bool HasValue<T>(this IEnumerable<T> collection)
        {
            return collection.Any();
        }

        /// <summary>
        /// 空校验
        /// </summary>
        /// <param name="collection"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static bool IsEmpty<T>(IEnumerable<T> collection)
        {
            return collection == null || !collection.Any();
        }
        
        /// <summary>
        /// 非空校验
        /// </summary>
        /// <param name="collection"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static bool IsNotEmpty<T>(IEnumerable<T> collection)
        {
            return !IsEmpty(collection);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace ToolLite
{
    public class QueueFactory<TEntity>
    {
        private List<TEntity> list;
        private readonly object LOCK = new object();

        public QueueFactory()
        {
            list = new List<TEntity>();
        }

        public QueueFactory(int capacity)
        {
            list = new List<TEntity>(capacity);
        }

        /// <summary>
        /// 添加任务到队列
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool Add(TEntity entity)
        {
            try
            {
                if (!Exists(entity))
                {
                    lock (LOCK)
                    {
                        if (!Exists(entity))
                            list.Add(entity);
                    }
                }                    
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// 判断存在
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool Exists(TEntity entity)
        {
            return list.Contains(entity);
        }

        /// <summary>
        /// 获取队列信息
        /// </summary>
        /// <returns></returns>
        public TEntity Get()
        {
            try
            {
                TEntity entity = default(TEntity);
                if (list.Count > 0)
                {
                    lock (LOCK)
                    {
                        if (list.Count > 0)
                        {
                            entity = list[0];
                            list.RemoveAt(0);
                        }                           
                    }
                }
                return entity;
            }
            catch (Exception ex)
            {
                return default(TEntity);
            }
        }

        /// <summary>
        /// 尝试获取
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool TryGet(ref TEntity entity)
        {
            try
            {
                if (list.Count > 0)
                {
                    lock (LOCK)
                    {
                        if (list.Count > 0)
                        {
                            entity = list[0];
                            list.RemoveAt(0);
                        }
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                entity = default(TEntity);
                return false;
            }
        }
    }
}

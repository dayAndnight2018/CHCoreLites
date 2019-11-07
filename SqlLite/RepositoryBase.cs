using SqlLite;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace SqlLite
{
    /// <summary>
    /// 仓储类的基类
    /// </summary>
    public abstract class RepositoryBase<TEntity, TKey>
    {
        protected string TableName { get => typeof(TEntity).Name; }
        /// <summary>
        /// 初始化一个仓储
        /// </summary>
        /// <param name="db">The database to access.</param>
        public RepositoryBase(Database db)
        {
            Db = db;
        }

        /// <summary>
        /// 一个数据库
        /// </summary>
        protected Database Db { get; }

        /// <summary>
        /// 开启事务
        /// </summary>
        /// <returns>An object representing the new transaction.</returns>
        public IDbTransaction BeginTransaction()
        {
            return BeginTransaction(IsolationLevel.ReadCommitted);
        }

        /// <summary>
        /// 开启指定事务等级的事务
        /// </summary>
        /// <param name="il">One of the <see cref="IsolationLevel"/> values.</param>
        /// <returns> An object representing the new transaction.</returns>
        public IDbTransaction BeginTransaction(IsolationLevel il)
        {
            return Db.BeginTransaction(il);
        }


        #region CRUD Mehthods

        /// <summary>
        /// 条件查询单条记录
        /// </summary>
        /// <param name="clause"></param>
        /// <returns></returns>
        //public Task<TEntity> FindAsync(string keyName, object value)
        //{
        //    var sqlSentence = new SqlSentence(TableName);
        //    var sql = sqlSentence.Select().Where(keyName).ToString();
        //    return Db.QuerySingleOrDefaultAsync<TEntity>(sql, value);
        //}

        /// <summary>
        /// 条件查询所有记录
        /// </summary>
        /// <param name="clause"></param>
        /// <returns></returns>
        //public Task<IEnumerable<TEntity>> QueryAsync(string clause, object value)
        //{
        //    var sqlSentence = new SqlSentence(TableName);
        //    var sql = sqlSentence.Select().WhereClause(clause).ToString();
        //    return Db.QueryAsync<TEntity>(sql, value);
        //}

        /// <summary>
        /// 插入单条记录
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        //public Task InsertAsync(TEntity entity)
        //{
        //    var sqlSentence = new SqlSentence(TableName);
        //    var sql = sqlSentence.InsertFull(entity).ToString();
        //    return Db.ExecuteAsync(sql, entity);
        //}

        /// <summary>
        /// 更新记录
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        //public Task<int> UpdateAsync(TEntity entity, string clause, object value, params string[] except)
        //{
        //    var sqlSentence = new SqlSentence(TableName);
        //    var sql = sqlSentence.Update(entity, except).WhereClause(clause).ToString();
        //    return Db.ExecuteAsync(sql, value);
        //}

        /// <summary>
        /// 条件局部更新记录
        /// </summary>
        /// <param name="update"></param>
        /// <param name="clause"></param>
        /// <returns></returns>
        //public Task<int> UpdateAsync(string clause, object value, params string[] param)
        //{
        //    var sqlSentence = new SqlSentence(TableName);
        //    var sql = sqlSentence.Update(param).WhereClause(clause).ToString();
        //    return Db.ExecuteAsync(sql, value);
        //}

        /// <summary>
        /// 根据主键删除记录
        /// </summary>
        /// <param name="keyName">主键</param>
        /// <param name="value">主键值</param>
        /// <returns></returns>
        //public Task<int> DeleteAsync(string keyName, object value)
        //{
        //    var sqlSentence = new SqlSentence(TableName);
        //    var sql = sqlSentence.Delete().Where(keyName).ToString();
        //    return Db.ExecuteAsync(sql, value);
        //}

        #endregion
    }

    /// <summary>
    /// A base class for a repository.
    /// </summary>
    /// <typeparam name="TEntity">The type of entity.</typeparam>
    public abstract class RepositoryBase<TEntity> : RepositoryBase<TEntity, long>
    {
        public RepositoryBase(Database db) : base(db)
        {
        }
    }
}

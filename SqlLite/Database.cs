
using System.Data;

namespace SqlLite
{
    public class Database
    {
        /// <summary>
        /// 内置日志
        /// </summary>
        //internal static ILogger<Database> Logger { get; set; }

        /// <summary>
        /// 注入连接
        /// </summary>
        /// <param name="connection"></param>
        public Database(IDbConnection connection)
        {
            Connection = connection;
        }

        public IDbConnection Connection { get; private set; }

        public IDbTransaction Transaction { get; private set; }

        /// <summary>
        /// 默认开启读写提交事务
        /// </summary>
        /// <returns></returns>
        public IDbTransaction BeginTransaction()
        {
            return BeginTransaction(IsolationLevel.ReadCommitted);
        }

        /// <summary>
        /// 开启指定事务等级的事务
        /// </summary>
        /// <param name="il"></param>
        /// <returns></returns>
        public IDbTransaction BeginTransaction(IsolationLevel level)
        {
            // Auto open connection.
            if (Connection.State == ConnectionState.Closed)
                Connection.Open();
            Transaction = Connection.BeginTransaction(level);
            return Transaction;
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            if (Transaction != null)
            {
                Transaction.Dispose();
                Transaction = null;
            }
            if (Connection != null)
            {
                Connection.Close();
                Connection.Dispose();
                Connection = null;
            }
        }
    }
}

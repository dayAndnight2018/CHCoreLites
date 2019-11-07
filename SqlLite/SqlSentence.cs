using ExtendsLite;
using SqlLite.Dapper;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SqlLite
{
    public class SqlSentence<TEntity> where TEntity : class
    {
        private string Sentence { get; set; }

        private string Table { get; set; } 

        public SqlSentence(string table)
        {
            this.Table = table;
        }

        public SqlSentence()
        {
            this.Table = typeof(TEntity).Name.ToLower();
        }

        /// <summary>
        /// 选择语句
        /// </summary>
        public SqlSentence<TEntity> Select()
        {
            this.Sentence = $" SELECT * FROM {Table} ";
            return this;
        }

        /// <summary>
        /// 查询字段
        /// </summary>
        /// <param name="param"></param>
        public SqlSentence<TEntity> Select(params string[] param)
        {
            this.Sentence = " SELECT " + string.Join(" , ", param) + $" FROM  {Table} ";
            return this;
        }

        /// <summary>
        /// 查询字段
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public SqlSentence<TEntity> Select(object param)
        {
            var columns = ParseParams(param);
            this.Sentence = " SELECT " + string.Join(" , ", columns) + $" FROM  {Table} ";
            return this;
        }

        /// <summary>
        /// 删除语句
        /// </summary>
        /// <returns></returns>
        public SqlSentence<TEntity> Delete()
        {
            this.Sentence = $" Delete FROM {Table} ";
            return this;
        }

        /// <summary>
        /// 更新语句
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public SqlSentence<TEntity> Update(params string[] param)
        {
            this.Sentence = $" UPDATE {Table} SET " + string.Join(" , ", param.Select(p => p + " = @" + p));
            return this;
        }

        /// <summary>
        /// 更新语句
        /// </summary>
        /// <param name="model">模型</param>
        /// <param name="except">排除项</param>
        /// <returns></returns>
        public SqlSentence<TEntity> Update(object model, params string[] except)
        {
            var updateCols = ParseParams(model).Except(except.ToList());
            this.Sentence = $" UPDATE {Table} SET " + string.Join(" , ", updateCols.Select(p => p + " = @" + p));
            return this;
        }

        /// <summary>
        /// 插入语句
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public SqlSentence<TEntity> InsertParticle(params string[] param)
        {
            this.Sentence = $" INSERT INTO {Table} (" + string.Join(" , ", param) + ") VALUES (  " + string.Join(" , ", param.Select(p => "@" + p)) + " ); ";
            return this;
        }

        /// <summary>
        /// 插入部分模型
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public SqlSentence<TEntity> InsertParticle(object model)
        {
            var colNames = ParseParams(model);
            this.Sentence = $" INSERT INTO {Table}( {string.Join(", ", colNames)} ) VALUES( @{string.Join(", @", colNames)}); ";
            return this;
        }

        /// <summary>
        /// 插入语句
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public SqlSentence<TEntity> InsertFull(params string[] param)
        {
            this.Sentence = $" INSERT INTO {Table} " + " VALUES ( " + string.Join(" , ", param.Select(p => "@" + p)) + " ); ";
            return this;
        }

        /// <summary>
        /// 插入完整对象
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public SqlSentence<TEntity> InsertFull(object model)
        {
            var colNames = ParseParams(model);
            this.Sentence = $" INSERT INTO {Table} VALUES( @{string.Join(", @", colNames)} ); ";
            return this;
        }

        /// <summary>
        /// 完整插入但不包含部分字段
        /// </summary>
        /// <param name="model"></param>
        /// <param name="excepts"></param>
        /// <returns></returns>
        public SqlSentence<TEntity> InsertFull(object model,params String[] excepts)
        {
            var colNames = ParseParams(model).Except(excepts);
            this.Sentence = $" INSERT INTO {Table} VALUES( @{string.Join(", @", colNames)} ); ";
            return this;
        }

        /// <summary>
        /// 添加分页信息
        /// </summary>
        /// <param name="page">页码</param>
        /// <param name="num">每页数量</param>
        /// <returns></returns>
        public SqlSentence<TEntity> Page(int page, int num)
        {
            this.Sentence += $" LIMIT {num} OFFSET {(page - 1) * num} ";
            return this;
        }

        /// <summary>
        /// 条件语句
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public SqlSentence<TEntity> Where(params string[] param)
        {
            this.Sentence += " WHERE " + And(param);
            return this;
        }

        /// <summary>
        /// 条件语句
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public SqlSentence<TEntity> WhereOr(params string[] param)
        {
            this.Sentence += " WHERE " + Or(param);
            return this;
        }


        /// <summary>
        /// OR
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public string Or(params string[] param)
        {
            return string.Join(" OR ", param.Select(p => p + " = @" + p));
        }

        /// <summary>
        /// 条件
        /// </summary>
        /// <param name="clause"></param>
        /// <returns></returns>
        public SqlSentence<TEntity> WhereClause(string clause)
        {
            this.Sentence += " WHERE " + clause;
            return this;
        }

        /// <summary>
        /// like语句
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public SqlSentence<TEntity> Like(string param)
        {
            this.Sentence += $" WHERE {param} LIKE %@{param}%";
            return this;
        }

        public SqlSentence<TEntity> OrderBy()
        {
            this.Sentence += $" ORDER BY ";
            return this;
        }

        public SqlSentence<TEntity> ASC(params string[] param)
        {
            this.Sentence += string.Join(" , ", param.Select(p => p + " ASC "));
            return this;
        }

        public SqlSentence<TEntity> DESC(params string[] param)
        {
            this.Sentence += string.Join(" , ", param.Select(p => p + " DESC "));
            return this;
        }

        /// <summary>
        /// 查询唯一
        /// </summary>
        /// <returns></returns>
        public SqlSentence<TEntity> SelectDistinct()
        {
            this.Sentence = $" SELECT DISTINCT * FROM {Table} ";
            return this;
        }

        /// <summary>
        /// 查询唯一
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public SqlSentence<TEntity> SelectDistinct(params string[] param)
        {
            this.Sentence = " SELECT  DISTINCT " + string.Join(" , ", param) + $" FROM  {Table} ";
            return this;
        }

        /// <summary>
        /// 唯一查询
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public SqlSentence<TEntity> SelectDistinct(object param)
        {
            var columns = ParseParams(param);
            this.Sentence = " SELECT  DISTINCT " + string.Join(" , ", columns) + $" FROM  {Table} ";
            return this;
        }


        /// <summary>
        /// InnerJoin
        /// </summary>
        /// <param name="tableB"></param>
        /// <param name="aParams"></param>
        /// <param name="bParams"></param>
        /// <returns></returns>
        public SqlSentence<TEntity> InnerJoin(string tableB, string[] aParams, string[] bParams)
        {
            this.Sentence = " SELECT  " + string.Join(" , ", aParams.Select(a => "A." + a).Union(bParams.Select(b => "B." + b))) + $" FROM  {Table}  A  INNER  JOIN {tableB}  B ";
            return this;
        }

        /// <summary>
        /// InnerJoin
        /// </summary>
        /// <param name="tableB"></param>
        /// <param name="aParams"></param>
        /// <param name="bParams"></param>
        /// <returns></returns>
        public SqlSentence<TEntity> InnerJoin(string tableB, object aParams, object bParams)
        {
            var aColNames = ParseParams(aParams);
            var bColNames = ParseParams(bParams);
            this.Sentence = " SELECT  " + string.Join(" , ", aColNames.Select(a => "A." + a).Union(bColNames.Select(b => "B." + b))) + $" FROM  {Table}  A  INNER  JOIN {tableB}  B ";
            return this;
        }

        /// <summary>
        /// InnerJoin
        /// </summary>
        /// <param name="tableB"></param>
        /// <returns></returns>
        public SqlSentence<TEntity> InnerJoin(string tableB)
        {
            this.Sentence = $" SELECT  A.* , B.*  FROM  {Table} A  INNER  JOIN {tableB}  B ";
            return this;
        }

        /// <summary>
        /// Join clauses
        /// </summary>
        /// <param name="aReference"></param>
        /// <param name="bReference"></param>
        /// <returns></returns>
        public SqlSentence<TEntity> JoinWhere(string aReference, string bReference)
        {
            this.Sentence += $" ON  A.{aReference} = B.{bReference} ";
            return this;
        }


        /// <summary>
        /// LEFT join
        /// </summary>
        /// <param name="tableB"></param>
        /// <param name="aParams"></param>
        /// <param name="bParams"></param>
        /// <returns></returns>
        public SqlSentence<TEntity> LeftJoin(string tableB, string[] aParams, string[] bParams)
        {
            this.Sentence = " SELECT  " + string.Join(" , ", aParams.Select(a => "A." + a).Union(bParams.Select(b => "B." + b))) + $" FROM  {Table}  A  LEFT  JOIN {tableB}  B ";
            return this;
        }

        /// <summary>
        /// LEft join
        /// </summary>
        /// <param name="tableB"></param>
        /// <param name="aParams"></param>
        /// <param name="bParams"></param>
        /// <returns></returns>
        public SqlSentence<TEntity> LeftJoin(string tableB, object aParams, object bParams)
        {
            var aColNames = ParseParams(aParams);
            var bColNames = ParseParams(bParams);
            this.Sentence = " SELECT  " + string.Join(" , ", aColNames.Select(a => "A." + a).Union(bColNames.Select(b => "B." + b))) + $" FROM  {Table}  A  LEFT  JOIN {tableB}  B ";
            return this;
        }

        /// <summary>
        /// Left join
        /// </summary>
        /// <param name="tableB"></param>
        /// <returns></returns>
        public SqlSentence<TEntity> LeftJoin(string tableB)
        {
            this.Sentence = $" SELECT  A.* , B.*  FROM  {Table} A  LEFT  JOIN {tableB}  B ";
            return this;
        }

        /// <summary>
        /// right join
        /// </summary>
        /// <param name="tableB"></param>
        /// <param name="aParams"></param>
        /// <param name="bParams"></param>
        /// <returns></returns>
        public SqlSentence<TEntity> RightJoin(string tableB, string[] aParams, string[] bParams)
        {
            this.Sentence = " SELECT  " + string.Join(" , ", aParams.Select(a => "A." + a).Union(bParams.Select(b => "B." + b))) + $" FROM  {Table}  A  RIGHT  JOIN {tableB}  B ";
            return this;
        }

        /// <summary>
        /// right join
        /// </summary>
        /// <param name="tableB"></param>
        /// <param name="aParams"></param>
        /// <param name="bParams"></param>
        /// <returns></returns>
        public SqlSentence<TEntity> RightJoin(string tableB, object aParams, object bParams)
        {
            var aColNames = ParseParams(aParams);
            var bColNames = ParseParams(bParams);
            this.Sentence = " SELECT  " + string.Join(" , ", aColNames.Select(a => "A." + a).Union(bColNames.Select(b => "B." + b))) + $" FROM  {Table}  A  RIGHT  JOIN {tableB}  B ";
            return this;
        }

        /// <summary>
        /// right join
        /// </summary>
        /// <param name="tableB"></param>
        /// <returns></returns>
        public SqlSentence<TEntity> RightJoin(string tableB)
        {
            this.Sentence = $" SELECT  A.* , B.*  FROM  {Table} A  RIGHT  JOIN {tableB}  B ";
            return this;
        }


        /// <summary>
        /// 计数
        /// </summary>
        /// <returns></returns>
        public SqlSentence<TEntity> Count()
        {
            this.Sentence = $" SELECT COUNT(*) FROM {Table} ";
            return this;
        }

        /// <summary>
        /// 拼接与运算
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        private string And(string[] param)
        {
            return string.Join(" AND ", param.Select(p => p + " = @" + p));
        }

        /// <summary>
        /// 解析参数
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public static IEnumerable<string> ParseParams(object param)
        {
            if (param == null)
            {
                return new HashSet<string>();
            }

            if (param is string str)
            {
                if (str.Contains(','))
                    return str
                        .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                        .Select(x => x.Trim());
                return new[] { str };
            }

            if (param is IEnumerable<string> array)
            {
                return array;
            }

            if (param is DynamicParameters dynamicParameters)
            {
                return dynamicParameters.ParameterNames;
            }

            var type = param is Type ? (param as Type) : param.GetType();

            return type
                .GetProperties()
                .Where(x => x.PropertyType.IsSimpleType())
                .Select(x => x.Name);
        }


        public override string ToString()
        {
            return Sentence;
        }

    }
}

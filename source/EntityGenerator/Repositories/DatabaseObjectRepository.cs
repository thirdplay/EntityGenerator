using Dapper;
using EntityGenerator.Entities;
using EntityGenerator.Properties;
using System.Collections.Generic;
using System.Data.Common;

namespace EntityGenerator.Repositories
{
    /// <summary>
    /// データベースオブジェクトへのアクセスを提供するリポジトリ。
    /// </summary>
    public class DatabaseObjectRepository : RepositoryBase
    {
        /// <summary>
        /// コンストラクタ。
        /// </summary>
        /// <param name="connection">DB接続</param>
        public DatabaseObjectRepository(DbConnection connection) : base(connection)
        {
        }

        /// <summary>
        /// テーブル名を取得します。
        /// </summary>
        /// <returns>テーブル名の列挙</returns>
        public IEnumerable<string> FindTableNames()
        {
            return this.Connection.Query<string>(Resources.Sql_SelectTableNames);
        }

        /// <summary>
        /// 指定 <see cref="owner"/> のカラム定義を全て取得します。
        /// </summary>
        /// <param name="tableName">テーブル名</param>
        /// <returns>カラム定義一覧</returns>
        public IEnumerable<ColumnDefinition> FindColumnDefinitions(string tableName)
        {
            return this.Connection.Query<ColumnDefinition>(
                Resources.Sql_SelectColumnDefinition, new { TableName = tableName });
        }
    }
}

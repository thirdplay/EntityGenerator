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
        /// データオブジェクトを全て取得します。
        /// </summary>
        /// <returns>データオブジェクトの列挙</returns>
        public IEnumerable<DatabaseObject> FindDataObjects()
        {
            return this.Connection.Query<DatabaseObject>(Resources.Sql_SelectDatabaseObject);
        }

        /// <summary>
        /// 指定 <see cref="owner"/> のテーブル定義を全て取得します。
        /// </summary>
        /// <param name="owner">オーナー</param>
        /// <param name="tableName">テーブル名</param>
        /// <returns>テーブル定義</returns>
        public IEnumerable<TableDefinition> FindTableDefinitions(string owner, string tableName)
        {
            return this.Connection.Query<TableDefinition>(
                Resources.Sql_SelectTableDefinition, new { Owner = owner, TableName = tableName });
        }
    }
}

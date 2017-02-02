using Dapper;
using EntityGenerator.Entities;
using EntityGenerator.Properties;
using System.Collections.Generic;
using System.Data.Common;

namespace EntityGenerator.Repositories
{
    /// <summary>
    /// データベース定義のアクセスを提供するリポジトリ。
    /// </summary>
    public class DbDefinitionRepository : RepositoryBase
    {
        /// <summary>
        /// コンストラクタ。
        /// </summary>
        /// <param name="connection">DB接続</param>
        public DbDefinitionRepository(DbConnection connection) : base(connection)
        {
        }

        /// <summary>
        /// 指定 <see cref="owner"/> のテーブル名を全て取得します。
        /// </summary>
        /// <param name="owner">オーナー</param>
        /// <returns>テーブル名の列挙</returns>
        public IEnumerable<string> FindTableNames(string owner)
        {
            return this.Connection.Query<string>(
                Resources.Sql_SelectTableName, new { Owner = owner });
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

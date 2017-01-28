using Dapper;
using EntityGenerator.Entities;
using EntityGenerator.Models;
using EntityGenerator.Properties;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityGenerator.Repositories
{
    /// <summary>
    /// テーブル定義のリポジトリ。
    /// </summary>
    public class TableDefinitionRepository : RepositoryBase
    {
        /// <summary>
        /// コンストラクタ。
        /// </summary>
        /// <param name="connection">DB接続</param>
        public TableDefinitionRepository(DbConnection connection) : base(connection)
        {
        }

        /// <summary>
        /// 指定 <see cref="owner"/> のテーブル定義を全て取得します。
        /// </summary>
        /// <param name="owner">オーナー</param>
        /// <param name="tableName">テーブル名</param>
        /// <returns>テーブル定義</returns>
        public IEnumerable<TableDefinition> FindAll(string owner, string tableName)
        {
            return this.Connection.Query<TableDefinition>(
                Resources.Sql_SelectTableDefinition, new { Owner = owner, TableName = tableName }).ToList();
        }
    }
}

using EntityGenerator.Entities;
using EntityGenerator.Repositories;
using Oracle.ManagedDataAccess.Client;
using System.Collections.Generic;

namespace EntityGenerator.Models
{
    /// <summary>
    /// テーブルを検索する機能を提供します。
    /// </summary>
    public class TableSearcher
    {
        /// <summary>
        /// テーブルを検索します。
        /// </summary>
        /// <param name="connectionString">接続文字列</param>
        /// <returns>ユーザテーブルの列挙</returns>
        public IEnumerable<UserTable> Search(OracleConnectionStringBuilder builder)
        {
            using (var conn = OracleConnectionFactory.CreateConnection(builder.ToString()))
            {
                var repository = new DatabaseObjectRepository(conn);
                return repository.FindUserTables();
            }
        }
    }
}

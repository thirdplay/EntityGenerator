using Dapper;
using EntityGenerator.Models;
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
        /// <returns></returns>
        public IEnumerable<TableDefinition> FindAll(string owner)
        {
            return this.Connection.Query<TableDefinition>(
                @"select TC.TABLE_NAME, TC.OWNER, TC.COLUMN_NAME, TC.DATA_TYPE, TC.DATA_DEFAULT, TC.NULLABLE, TC.COLUMN_ID, TC.DATA_LENGTH, TC.DATA_SCALE, TC.DATA_PRECISION, C.CONSTRAINT_TYPE, CC.POSITION "
                + @"from ALL_TAB_COLUMNS TC left join (ALL_CONS_COLUMNS CC INNER JOIN ALL_CONSTRAINTS C ON(CC.CONSTRAINT_NAME = C.CONSTRAINT_NAME and CC.TABLE_NAME = C.TABLE_NAME and CC.OWNER = C.OWNER and C.CONSTRAINT_TYPE = 'P')) on (TC.TABLE_NAME = CC.TABLE_NAME and TC.COLUMN_NAME = CC.COLUMN_NAME) "
                + @"where TC.OWNER = :Owner order by TC.TABLE_NAME, TC.COLUMN_ID"
                , new { Owner = owner }
            ).ToList();
        }
    }
}

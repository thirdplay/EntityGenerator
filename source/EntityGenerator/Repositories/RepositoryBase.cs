using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityGenerator.Repositories
{
    /// <summary>
    /// リポジトリの基底クラス。
    /// </summary>
    public abstract class RepositoryBase
    {
        /// <summary>
        /// DB接続
        /// </summary>
        protected DbConnection Connection { get; set; }

        /// <summary>
        /// コンストラクタ。
        /// </summary>
        /// <param name="connection">DB接続</param>
        public RepositoryBase(DbConnection connection)
        {
            this.Connection = connection;
        }
    }
}

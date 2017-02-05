using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityGenerator.Entities
{
    /// <summary>
    /// ユーザテーブルを表すクラス。
    /// </summary>
    public class UserTable
    {
        /// <summary>
        /// オーナー
        /// </summary>
        public string Owner { get; set; }

        /// <summary>
        /// テーブル名
        /// </summary>
        public string TableName { get; set; }
    }
}

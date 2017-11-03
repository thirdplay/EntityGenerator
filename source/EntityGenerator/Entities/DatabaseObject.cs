using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityGenerator.Entities
{
    /// <summary>
    /// データベースオブジェクトを表すクラス。
    /// </summary>
    public class DatabaseObject
    {
        /// <summary>
        /// 所有者
        /// </summary>
        public string Owner { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
    }
}

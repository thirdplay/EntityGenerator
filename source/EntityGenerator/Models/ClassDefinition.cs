using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityGenerator.Models
{
    /// <summary>
    /// クラス定義を表すクラス。
    /// </summary>
    public class ClassDefinition
    {
        /// <summary>
        /// クラス名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// クラスの説明
        /// </summary>
        public string Description { get; set; }
    }
}

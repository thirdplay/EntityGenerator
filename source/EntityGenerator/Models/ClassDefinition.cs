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
        /// ネームスペース。
        /// </summary>
        public string Namespace { get; set; };

        /// <summary>
        /// クラス名称。
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// クラスの説明。
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// プロパティのリスト。
        /// </summary>
        public List<PropertyDefinition> Properties { get; set; }
    }
}

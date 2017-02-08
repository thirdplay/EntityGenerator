using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityGenerator.Models
{
    /// <summary>
    /// プロパティ定義を表すクラス。
    /// </summary>
    public class PropertyDefinition
    {
        /// <summary>
        /// プロパティ名称。
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// プロパティの説明。
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// プロパティの型。
        /// </summary>
        public string TypeName { get; set; }
    }
}

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

        /// <summary>
        /// Oracleのデータ型をC#のデータ型に置換する置換表。
        /// </summary>
        private static readonly Dictionary<string, string> TypeNames = new Dictionary<string, string>()
        {
            {"CHAR", "string"},
            {"NCHAR", "string"},
            {"VARCHAR2", "string"},
            {"NVARCHAR2", "string"},
            {"NUMBER", "decimal"},
            {"INTEGER", "decimal"},
            {"UNSIGNED INTEGER", "decimal"},
            {"FLOAT", "decimal"},
            {"DATE", "DateTime"},
            {"TIMESTAMP", "DateTime"},
            {"TIMESTAMP WITH TIME ZONE", "DateTime"},
            {"TIMESTAMP WITH LOCAL TIME ZONE", "DateTime"},
            {"BLOB", "Byte[]"},
            {"CLOB", "string"},
            {"ROWID", "string"},
        };

        /// <summary>
        /// プロパティの型を取得します。
        /// </summary>
        /// <param name="dataType">Oracleデータ型</param>
        /// <returns>プロパティの型</returns>
        public string GetTypeName(string dataType)
        {
            if (TypeNames.ContainsKey(dataType))
            {
                return TypeNames[dataType];
            }
            return "UnknownType";
        }
    }
}

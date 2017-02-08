using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityGenerator.Models
{
    /// <summary>
    /// Oracleデータ型からC#データ型への変換を行うコンバータを表します。
    /// </summary>
    public static class OracleTypeToCsTypeConverter
    {
        /// <summary>
        /// Oracleデータ型をC#データ型に置換する置換表。
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
        /// Oracleデータ型をC#データ型に変換します。
        /// </summary>
        /// <param name="oracleDataType">Oracleデータ型</param>
        /// <returns>C#データ型</returns>
        public static string Convert(string oracleDataType)
        {
            if (TypeNames.ContainsKey(oracleDataType))
            {
                return TypeNames[oracleDataType];
            }
            return "UnknownType";
        }
    }
}

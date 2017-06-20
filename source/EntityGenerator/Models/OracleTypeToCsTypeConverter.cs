using EntityGenerator.Entities;
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
        public static string Convert(TableDefinition tableDefinition)
        {
            // NUMBER型の整数の場合、桁数ごとに個別に変換する
            if (tableDefinition.DataType == "NUMBER" && tableDefinition.DataScale == 0)
            {
                // 整数10桁以下の場合はint
                if (tableDefinition.DataPrecision <= 10)
                {
                    return "int";
                }
                // 整数16桁以下の場合はlong
                else if (tableDefinition.DataPrecision <= 18)
                {
                    return "long";
                }
            }
            if (TypeNames.ContainsKey(tableDefinition.DataType))
            {
                return TypeNames[tableDefinition.DataType];
            }
            return "UnknownType";
        }
    }
}

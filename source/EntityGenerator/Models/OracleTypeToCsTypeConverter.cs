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
        /// <param name="columnDefinition">カラム定義</param>
        /// <returns>C#データ型</returns>
        public static string Convert(ColumnDefinition columnDefinition)
        {
            // NUMBER型の整数の場合、桁数ごとに個別に変換する
            if (columnDefinition.DataType == "NUMBER" && columnDefinition.DataScale == 0)
            {
                // 整数9桁以下の場合はint
                if (columnDefinition.DataPrecision <= 9)
                {
                    return "int";
                }
                // 整数18桁以下の場合はlong
                else if (columnDefinition.DataPrecision <= 18)
                {
                    return "long";
                }
            }
            if (TypeNames.ContainsKey(columnDefinition.DataType))
            {
                return TypeNames[columnDefinition.DataType];
            }
            return "UnknownType";
        }
    }
}

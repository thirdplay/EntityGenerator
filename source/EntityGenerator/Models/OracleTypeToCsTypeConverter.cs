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
        /// NULL許容型の型名。
        /// </summary>
        private static readonly string[] NullableTypeNames = new[] {
            "string",
            "Byte[]",
        };

        /// <summary>
        /// Oracleデータ型をC#データ型に変換します。
        /// </summary>
        /// <param name="columnDefinition">カラム定義</param>
        /// <returns>C#データ型</returns>
        public static string Convert(ColumnDefinition columnDefinition)
        {
            string result = "UnknownType";

            if (TypeNames.ContainsKey(columnDefinition.DataType))
            {
                result = TypeNames[columnDefinition.DataType];
            }

            // NUMBER型の整数の場合、桁数ごとに個別に変換する
            if (columnDefinition.DataType == "NUMBER" && columnDefinition.DataScale == 0)
            {
                // 整数9桁以下の場合はint
                if (columnDefinition.DataPrecision <= 9)
                {
                    result = "int";
                }
                // 整数18桁以下の場合はlong
                else if (columnDefinition.DataPrecision <= 18)
                {
                    result = "long";
                }
            }

            // カラムがNULLを許容する場合
            if (columnDefinition.Nullable == "Y")
            {
                // C#データ型が非NULL許容型の場合
                if (!NullableTypeNames.Contains(result))
                {
                    // NULL許容型に変更する
                    result += "?";
                }
            }

            return result;
        }
    }
}

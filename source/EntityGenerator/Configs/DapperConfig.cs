using Dapper;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace EntityGenerator.Configs
{
    /// <summary>
    /// Dapperの設定クラス。
    /// </summary>
    public class DapperConfig
    {
        /// <summary>
        /// 型マッピングを設定します。
        /// </summary>
        public static void RegisterMappings(string @namespace)
        {
            var types = from type in Assembly.GetExecutingAssembly().GetTypes()
                        where type.IsClass && type.Namespace == @namespace
                        select type;
            foreach(var target in types)
            {
                SqlMapper.SetTypeMap(target, new CustomPropertyTypeMap(target, (type, columnName) =>
                {
                    // TODO
                    // 1. columnNameをPascalCaseに変換する
                    //   TABLE_NAME => TableName
                    //   table_name => TableName
                    //
                    return type.GetProperties()
                        .FirstOrDefault(x =>
                            x.GetCustomAttributes(false)
                                .OfType<ColumnAttribute>()
                                .Any(attr => attr.Name == columnName)
                    );
                }));
            }
        }
        ///// <summary>
        ///// スネークケースをパスカルケースに変換します
        ///// 例) quoted_printable_encode → QuotedPrintableEncode
        ///// </summary>
        //public static string SnakeToPascal(this string self)
        //{
        //    if (string.IsNullOrEmpty(self))
        //    {
        //        return self;
        //    }

        //    return self
        //        .Split(new[] { '_' }, StringSplitOptions.RemoveEmptyEntries)
        //        .Select(s => char.ToUpperInvariant(s[0]) + s.Substring(1, s.Length - 1))
        //        .Aggregate(string.Empty, (s1, s2) => s1 + s2);
        //}
    }
}

using Dapper;
using EntityGenerator.Models;
using System;
using System.Linq;
using System.Reflection;

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
                    type.GetProperty(columnName.SnakeToPascal())));
            }
        }
    }
}

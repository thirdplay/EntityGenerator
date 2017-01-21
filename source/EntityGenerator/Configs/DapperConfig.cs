using Dapper;
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
                {
                    return type.GetProperty(
                        columnName.ToLower()
                        .Split(new[] { '_' }, StringSplitOptions.RemoveEmptyEntries)
                        .Select(s => char.ToUpperInvariant(s[0]) + s.Substring(1, s.Length - 1))
                        .Aggregate(string.Empty, (s1, s2) => s1 + s2)
                    );
                }));
            }
        }
    }
}

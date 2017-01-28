using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityGenerator.Models
{
    /// <summary>
    /// 文字列クラスの拡張機能を提供します。
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// スネークケースをパスカルケースに変換します。
        /// </summary>
        /// <param name="value">変換する文字列</param>
        /// <returns>変換後の文字列</returns>
        public static string SnakeToPascal(this string value)
        {
            return value.ToLower()
                .Split(new[] { '_' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(s => char.ToUpperInvariant(s[0]) + s.Substring(1, s.Length - 1))
                .Aggregate(string.Empty, (s1, s2) => s1 + s2);
        }
    }
}

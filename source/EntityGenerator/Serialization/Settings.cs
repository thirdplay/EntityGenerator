using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityGenerator.Serialization
{
    /// <summary>
    /// アプリケーションの設定を提供します。
    /// </summary>
    public static class Settings
    {
        /// <summary>
        /// 接続設定
        /// </summary>
        public static ConnectionSettings Connection { get; } = new ConnectionSettings(LocalSettingsProvider.Instance);

        /// <summary>
        /// 生成設定
        /// </summary>
        public static GenerationSettings Generation { get;} = new GenerationSettings(LocalSettingsProvider.Instance);
    }
}

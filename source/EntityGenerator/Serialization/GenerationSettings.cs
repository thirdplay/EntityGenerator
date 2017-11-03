using WpfUtility.Serialization;

namespace EntityGenerator.Serialization
{
    /// <summary>
    /// 生成設定のアクセスを提供します。
    /// </summary>
    public class GenerationSettings: SettingsHost
    {
        /// <summary>
        /// シリアル化機能提供者
        /// </summary>
        private readonly ISerializationProvider provider;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="provider">シリアル化機能提供者</param>
        public GenerationSettings(ISerializationProvider provider)
        {
            this.provider = provider;
        }

        /// <summary>
        /// 名前空間
        /// </summary>
        public SerializableProperty<string> Namespace => this.Cache(key => new SerializableProperty<string>(key, this.provider));

        /// <summary>
        /// 出力先
        /// </summary>
        public SerializableProperty<string> OutputDestnation => this.Cache(key => new SerializableProperty<string>(key, this.provider));
    }
}

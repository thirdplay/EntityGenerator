using WpfUtility.Serialization;

namespace EntityGenerator.Serialization
{
    /// <summary>
    /// 接続設定のアクセスを提供します。
    /// </summary>
    public class ConnectionSettings: SettingsHost
    {
        /// <summary>
        /// シリアル化機能提供者
        /// </summary>
        private readonly ISerializationProvider provider;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="provider">シリアル化機能提供者</param>
        public ConnectionSettings(ISerializationProvider provider)
        {
            this.provider = provider;
        }

        /// <summary>
        /// データソース
        /// </summary>
        public SerializableProperty<string> DataSource => this.Cache(key => new SerializableProperty<string>(key, this.provider));

        /// <summary>
        /// ユーザID
        /// </summary>
        public SerializableProperty<string> UserId => this.Cache(key => new SerializableProperty<string>(key, this.provider));

        /// <summary>
        /// パスワード
        /// </summary>
        public SerializableProperty<string> Password => this.Cache(key => new SerializableProperty<string>(key, this.provider));
    }
}

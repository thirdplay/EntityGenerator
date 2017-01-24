using EntityGenerator.Repositories;
using EntityGenerator.Templetes;
using Oracle.ManagedDataAccess.Client;
using System;

namespace EntityGenerator.Models
{
    /// <summary>
    /// エンティティを生成する機能を提供します。
    /// </summary>
    public class EntityGenerator
    {
        /// <summary>
        /// エンティティを生成します。
        /// </summary>
        /// <param name="connectionString">接続文字列</param>
        public void Generate(string userId, string password, string dataSource)
        {
            var builder = new OracleConnectionStringBuilder()
            {
                UserID = userId,
                Password = password,
                DataSource = dataSource
            };
            using (var conn = OracleConnectionFactory.CreateConnection(builder.ToString()))
            {
                // テーブル定義情報の取得
                var repository = new TableDefinitionRepository(conn);
                var list = repository.FindAll(userId);

                // クラス定義情報の生成
                //ClassDefinition
                //PropertyDefinition
            }

            // 実行時テンプレートをインスタンス化する
            // 実行時テンプレートはカスタムツールは"TextTemplatingFilePreprocessor"として
            // 設定されており、テンプレートの保存時に実行時テンプレートクラスに変換される.
            var tmpl = new EntityTemplate();

            // partial classで宣言したメンバ変数に対して値を入れる.
            //tmpl.Title = "Hello, T4 RuntimeTemplate!";
            //tmpl.Items = new List<string>() { "aaa", "bbb", "ccc" };

            // テンプレートを評価する.
            string generatedText = tmpl.TransformText();

            // エンティティモデルの生成
            // 結果の出力
            System.Diagnostics.Debug.WriteLine(generatedText);
            Console.WriteLine(generatedText);

            //    conn.Query<UserInfo>(@"SELECT * FROM USER_INFO WHERE USER_ID = :UserId", new { UserId = "test" })
            //        .ToList()
            //        .ForEach(x => Console.WriteLine($"UserId:{x.UserId}"));
        }
    }
}

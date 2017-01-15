using EntityGenerator.Templetes;
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
        public void Generate()
        {
            // 実行時テンプレートをインスタンス化する
            // 実行時テンプレートはカスタムツールは"TextTemplatingFilePreprocessor"として
            // 設定されており、テンプレートの保存時に実行時テンプレートクラスに変換される.
            var tmpl = new EntityTemplate();

            // partial classで宣言したメンバ変数に対して値を入れる.
            //tmpl.Title = "Hello, T4 RuntimeTemplate!";
            //tmpl.Items = new List<string>() { "aaa", "bbb", "ccc" };

            // テンプレートを評価する.
            string generatedText = tmpl.TransformText();

            // 結果の出力
            System.Diagnostics.Debug.WriteLine(generatedText);
            Console.WriteLine(generatedText);
        }
    }
}

using EntityGenerator.Repositories;
using EntityGenerator.Templetes;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Linq;

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
        public void Generate(OracleConnectionStringBuilder builder)
        {
            using (var conn = OracleConnectionFactory.CreateConnection(builder.ToString()))
            {
                // テーブル定義情報の取得
                var repository = new TableDefinitionRepository(conn);
                var tables = repository.FindAll(builder.UserID);

                // クラス定義情報の生成
                //var classes = GetClassDefinition(tables);
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
        }

        //private IEnumerable<ClassDefinition> GetClassDefinition(IEnumerable<TableDefinition> tables)
        //{
        //    var tableNames = tables.Select(x => x.TableName).Distinct();

        //    var list = new List<ClassDefinition>();
        //    foreach (var tableName in tableNames)
        //    {
        //        var definition = new ClassDefinition()
        //        {
        //            Name = tableName,
        //        };
        //        list.Add(definition);
        //    }
        //    return null;
        //}
    }
}

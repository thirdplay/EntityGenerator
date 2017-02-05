using EntityGenerator.Entities;
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
            var tableNames = new string[] { "USER_INFO" };
            using (var conn = OracleConnectionFactory.CreateConnection(builder.ToString()))
            {
                var repository = new DatabaseObjectRepository(conn);

                foreach (var tableName in tableNames)
                {
                    // テーブル定義情報の取得
                    var tableDefinitinos = repository.FindTableDefinitions(builder.UserID, tableName);

                    // クラス定義情報の生成
                    var classDefinition = GetClassDefinition(tableName, tableDefinitinos);

                    // 実行時テンプレートをインスタンス化する
                    // 実行時テンプレートはカスタムツールは"TextTemplatingFilePreprocessor"として
                    // 設定されており、テンプレートの保存時に実行時テンプレートクラスに変換される.
                    var tmpl = new EntityTemplate()
                    {
                        ClassDefinition = classDefinition
                    };

                    // テンプレートを評価する.
                    string generatedText = tmpl.TransformText();

                    // エンティティモデルの生成
                    // 結果の出力
                    System.Diagnostics.Debug.WriteLine(generatedText);
                    Console.WriteLine(generatedText);
                }
            }

        }

        /// <summary>
        /// テーブル定義に従い、クラス定義を取得します。
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="tableDefinitions"></param>
        /// <returns>クラス定義</returns>
        private ClassDefinition GetClassDefinition(string tableName, IEnumerable<TableDefinition> tableDefinitions)
        {
            // クラス定義の生成
            var firstDefinition = tableDefinitions.First();
            var classDefinition = new ClassDefinition()
            {
                Name = firstDefinition.TableName.SnakeToPascal(),
                Description = firstDefinition.TableComments
            };

            // プロパティ定義の生成
            classDefinition.Properties = new List<PropertyDefinition>();
            foreach (var tableDefinition in tableDefinitions)
            {
                var property = new PropertyDefinition()
                {
                    Name = tableDefinition.ColumnName.SnakeToPascal(),
                    Description = tableDefinition.ColumnComments
                };
                classDefinition.Properties.Add(property);
            }

            return classDefinition;
        }
    }
}

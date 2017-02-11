using EntityGenerator.Entities;
using EntityGenerator.Properties;
using EntityGenerator.Repositories;
using EntityGenerator.Templetes;
using EntityGenerator.ViewModels;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace EntityGenerator.Models
{
    /// <summary>
    /// エンティティを生成する機能を提供します。
    /// </summary>
    public class EntityGenerator
    {
        /// <summary>
        /// データベースオブジェクトを検索します。
        /// </summary>
        /// <param name="connectionString">接続文字列</param>
        /// <returns>データベースオブジェクトのコレクション</returns>
        public ObservableCollection<DataObjectViewModel> SearchDataObjects(OracleConnectionStringBuilder builder)
        {
            try
            {
                using (var conn = OracleConnectionFactory.CreateConnection(builder.ToString()))
                {
                    // データオブジェクトの検索
                    var repository = new DatabaseObjectRepository(conn);
                    var dataObjects = repository.FindDataObjects();

                    // ツリービューソースに変換する
                    var results = new ObservableCollection<DataObjectViewModel>();
                    var owners = dataObjects.Select(x => x.Owner).Distinct();
                    foreach (string owner in owners)
                    {
                        var ownerNode = new DataObjectViewModel(owner, true);
                        var childrens = dataObjects.Where(x => x.Owner == owner);
                        foreach (var children in childrens)
                        {
                            ownerNode.Add(new DataObjectViewModel(children.Name, true));
                        }
                        results.Add(ownerNode);
                    }

                    return results;
                }
            }
            catch (Exception ex)
            {
                Application.ShowException(ex);
                return new ObservableCollection<DataObjectViewModel>();
            }
        }

        /// <summary>
        /// エンティティを生成します。
        /// </summary>
        /// <param name="connectionString">接続文字列</param>
        public void Generate(OracleConnectionStringBuilder builder)
        {
            try
            {
                var tableNames = new string[] { "USER_INFO" };
                using (var conn = OracleConnectionFactory.CreateConnection(builder.ToString()))
                {
                    var repository = new DatabaseObjectRepository(conn);

                    foreach (var tableName in tableNames)
                    {
                        // クラス定義情報の生成
                        var tableDefinitinos = repository.FindTableDefinitions(builder.UserID, tableName);
                        var classDefinition = GetClassDefinition(tableName, tableDefinitinos);

                        // 実行時テンプレートをインスタンス化する
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
            catch (Exception ex)
            {
                Application.ShowException(ex);
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
                    Description = tableDefinition.ColumnComments,
                    TypeName = OracleTypeToCsTypeConverter.Convert(tableDefinition.DataType)
                };
                classDefinition.Properties.Add(property);
            }

            return classDefinition;
        }
    }
}

﻿using EntityGenerator.Entities;
using EntityGenerator.Extensions;
using EntityGenerator.Repositories;
using EntityGenerator.Templetes;
using EntityGenerator.Views.Controls;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

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
        public Task<ObservableCollection<CheckTreeSource>> SearchDataObjects(OracleConnectionStringBuilder builder)
        {
            return Task.Run(() =>
            {
                try
                {
                    using (var conn = new OracleConnection(builder.ToString()))
                    {
                        var results = new ObservableCollection<CheckTreeSource>();

                        // テーブル名とビュー名の検索
                        var repository = new DatabaseObjectRepository(conn);
                        var tableNames = repository.FindTableNames();
                        var viewNames = repository.FindViewNames();

                        // ツリービューソースに変換
                        results.Add(CreateNode("テーブル", tableNames));
                        results.Add(CreateNode("ビュー", viewNames));

                        return results;
                    }
                }
                catch (Exception ex)
                {
                    Application.ShowException(ex);
                    return new ObservableCollection<CheckTreeSource>();
                }
            });
        }

        /// <summary>
        /// エンティティを生成します。
        /// </summary>
        /// <param name="outputDir">出力先ディレクトリ</param>
        /// <param name="builder">接続文字列</param>
        /// <param name="namespace">名前空間</param>
        /// <param name="checkedItems">選択中の項目</param>
        /// <returns>タスク</returns>
        public Task Generate(string outputDir, OracleConnectionStringBuilder builder, string @namespace, List<CheckTreeSource> checkedItems)
        {
            return Task.Run(() =>
            {
                try
                {
                    var tableNames = checkedItems.Where(x => x.Parent != null).Select(x => x.Header);
                    using (var conn = new OracleConnection(builder.ToString()))
                    {
                        var repository = new DatabaseObjectRepository(conn);

                        foreach (var tableName in tableNames)
                        {
                            // クラス定義情報の生成
                            var tableDefinitinos = repository.FindTableDefinitions(builder.UserID, tableName);
                            var classDefinition = GetClassDefinition(@namespace, tableName, tableDefinitinos);

                            // テンプレートを評価する
                            var tmpl = new EntityTemplate()
                            {
                                ClassDefinition = classDefinition
                            };
                            var generatedText = tmpl.TransformText();

                            // エンティティモデルの出力
                            Debug.WriteLine(generatedText);
                            File.WriteAllText(Path.Combine(outputDir, tableName.SnakeToPascal() + ".cs"), generatedText);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Application.ShowException(ex);
                }
            });
        }

        /// <summary>
        /// ツリービューノードを作成します。
        /// </summary>
        /// <param name="header">ラベル</param>
        /// <param name="children">子要素</param>
        /// <returns>ノード</returns>
        private CheckTreeSource CreateNode(string header, IEnumerable<string> children)
        {
            var node = new CheckTreeSource(header, true);
            foreach (string child in children)
            {
                node.Add(new CheckTreeSource(child, true));
            }
            return node;
        }

        /// <summary>
        /// テーブル定義に従い、クラス定義を取得します。
        /// </summary>
        /// <param name="@namespace">名前空間</param>
        /// <param name="tableName">テーブル名</param>
        /// <param name="tableDefinitions">テーブル定義リスト</param>
        /// <returns>クラス定義</returns>
        private ClassDefinition GetClassDefinition(string @namespace, string tableName, IEnumerable<TableDefinition> tableDefinitions)
        {
            // クラス定義の生成
            var firstDefinition = tableDefinitions.First();
            var classDefinition = new ClassDefinition()
            {
                Namespace = @namespace,
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
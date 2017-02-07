using EntityGenerator.Entities;
using EntityGenerator.Repositories;
using EntityGenerator.Views.Controls;
using Oracle.ManagedDataAccess.Client;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace EntityGenerator.Models
{
    /// <summary>
    /// テーブルを検索する機能を提供します。
    /// </summary>
    public class TableSearcher
    {
        /// <summary>
        /// テーブルを検索します。
        /// </summary>
        /// <param name="connectionString">接続文字列</param>
        /// <returns>ユーザテーブルの列挙</returns>
        public ObservableCollection<CheckTreeSource> Search(OracleConnectionStringBuilder builder)
        {
            using (var conn = OracleConnectionFactory.CreateConnection(builder.ToString()))
            {
                //var repository = new DatabaseObjectRepository(conn);
                //return repository.FindUserTables();
                var tables = new ObservableCollection<CheckTreeSource>();
                var table = new CheckTreeSource() { Text = "テーブル" };
                var demo = new CheckTreeSource() { Text = "DEMO" };
                table.Add(demo);
                demo.Add(new CheckTreeSource() { Text = "USER_INFO" });
                demo.Add(new CheckTreeSource() { Text = "T_SAMPLE01" });
                tables = new ObservableCollection<CheckTreeSource>();
                tables.Add(table);
                return tables;
            }
        }
    }
}

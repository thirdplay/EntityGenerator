using Dapper;
using EntityGenerator.DbProfilers;
using EntityGenerator.Models;
using Livet;
using Oracle.ManagedDataAccess.Client;
using StackExchange.Profiling;
using StackExchange.Profiling.Data;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace EntityGenerator.ViewModels
{
    /// <summary>
    /// メインウィンドウのViewModel。
    /// </summary>
    public class MainWindowViewModel : ViewModel
    {
        /// <summary>
        /// 初期化。
        /// </summary>
        public void Initialize()
        {
        }

        /// <summary>
        /// エンティティを生成します。
        /// </summary>
        public void Generate()
        {
            // TODO:
            // 環境変数(ORACLE_HOME)が設定されていないため、tnsname.oraを参照しません。

            // TODO:
            // 1. テーブル定義情報の取得
            // 2. クラス定義情報の生成
            // 3. エンティティモデルの生成
            var connBuilder = new OracleConnectionStringBuilder()
            {
                UserID = "DEMO",
                Password = "DEMO",
                DataSource = "XE"
            };
            using (var conn = OracleConnectionFactory.CreateConnection(connBuilder.ToString()))
            {
                var list = conn.Query<TableDefinition>(@"select TC.TABLE_NAME as TableName, TC.OWNER, TC.COLUMN_NAME, TC.DATA_TYPE, TC.DATA_DEFAULT, TC.NULLABLE, TC.COLUMN_ID, TC.DATA_LENGTH, TC.DATA_SCALE, TC.DATA_PRECISION, C.CONSTRAINT_TYPE, CC.POSITION from ALL_TAB_COLUMNS TC left join (ALL_CONS_COLUMNS CC INNER JOIN ALL_CONSTRAINTS C ON(CC.CONSTRAINT_NAME = C.CONSTRAINT_NAME and CC.TABLE_NAME = C.TABLE_NAME and CC.OWNER = C.OWNER and C.CONSTRAINT_TYPE = 'P')) on (TC.TABLE_NAME = CC.TABLE_NAME and TC.COLUMN_NAME = CC.COLUMN_NAME) where TC.OWNER = 'DEMO' order by TC.TABLE_NAME, TC.COLUMN_ID").ToList();
                var record = list.First();
                //.ForEach(x => Console.WriteLine($"TABLE_NAME:{x.TABLE_NAME}"));

                conn.Query<UserInfo>(@"SELECT * FROM USER_INFO WHERE USER_ID = :UserId", new { UserId = "test" })
                    .ToList()
                    .ForEach(x => Console.WriteLine($"UserId:{x.UserId}"));
                //
            }

            var generator = new Models.EntityGenerator();
            generator.Generate();
        }
    }

    [Table("USER_INFO")]
    public class UserInfo
    {
        [Column("USER_ID")]
        public string UserId { get; set; }
        [Column("PASSWORD")]
        public string Password { get; set; }
        [Column("FIRST_NAME")]
        public string FirstName { get; set; }
        [Column("LAST_NAME")]
        public string LastName { get; set; }
        [Column("SEX")]
        public string Sex { get; set; }
        [Column("PHONE_NUMBER")]
        public string PhoneNumber { get; set; }
        [Column("MAIL_ADDRESS")]
        public string MailAddress { get; set; }
    }
}

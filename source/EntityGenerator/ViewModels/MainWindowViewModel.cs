using Dapper;
using Livet;
using MiniProfiler.Integrations;
using Oracle.ManagedDataAccess.Client;
using StackExchange.Profiling.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Data.Common;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

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
            //環境変数(ORACLE_HOME)が設定されていないため、tnsname.oraを参照しません。
            var connBuilder = new OracleConnectionStringBuilder()
            {
                UserID = "DEMO",
                Password = "DEMO",
                DataSource = "XE"
            };
            //using (var conn = new OracleConnection(connBuilder.ToString()))
            using (var conn = CreateOracleConnection(connBuilder.ToString(), CustomDbProfiler.Current))
            {
                conn.Query("SELECT * FROM user_tables")
                    .ToList()
                    .ForEach(x => Console.WriteLine($"TABLE_NAME:{x.TABLE_NAME}"));

                conn.Query<UserInfo>("select * from USER_INFO")
                    .ToList()
                    .ForEach(x => Console.WriteLine($"UserId:{x}"));

                foreach (var command in CustomDbProfiler.Current.ProfilerContext.ExecutedCommands)
                {
                    Console.WriteLine(command.ExtractToText());
                }
            }
        }

        private DbConnection CreateOracleConnection(string connectionString, IDbProfiler dbProfiler)
        {
            var connection = new ProfiledDbConnection(new OracleConnection(connectionString), dbProfiler);
            //connection.Open();
            return connection;
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

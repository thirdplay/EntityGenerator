﻿using Dapper;
using EntityGenerator.DbProfilers;
using EntityGenerator.Models;
using Livet;
using Livet.Messaging.IO;
using Oracle.ManagedDataAccess.Client;
using StackExchange.Profiling;
using StackExchange.Profiling.Data;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace EntityGenerator.ViewModels
{
    /// <summary>
    /// メインウィンドウのViewModel。
    /// </summary>
    public class MainWindowViewModel : ViewModel
    {
        /// <summary>
        /// エンティティ生成クラス。
        /// </summary>
        private readonly Models.EntityGenerator generator;

        /// <summary>
        /// 接続文字列。
        /// </summary>
        private OracleConnectionStringBuilder builder;

        #region UserId 変更通知プロパティ
        private string _UserId;
        /// <summary>
        /// ユーザIDを取得または設定します。
        /// </summary>
        public string UserId
        {
            get { return _UserId; }
            set
            { 
                if (_UserId != value)
                {
                    _UserId = value;
                    RaisePropertyChanged();
                }
            }
        }
        #endregion

        #region Password 変更通知プロパティ
        private string _Password;
        /// <summary>
        /// パスワードを取得または設定します。
        /// </summary>
        public string Password
        {
            get { return _Password; }
            set
            { 
                if (_Password != value)
                {
                    _Password = value;
                    RaisePropertyChanged();
                }
            }
        }
        #endregion

        #region DataSource 変更通知プロパティ
        private string _DataSource;
        /// <summary>
        /// データソースを取得または設定します。
        /// </summary>
        public string DataSource
        {
            get { return _DataSource; }
            set
            { 
                if (_DataSource != value)
                {
                    _DataSource = value;
                    RaisePropertyChanged();
                }
            }
        }
        #endregion

        /// <summary>
        /// コンストラクタ。
        /// </summary>
        public MainWindowViewModel()
        {
            this.generator = new Models.EntityGenerator();
            this.DataSource = "XE";
            this.UserId = "DEMO";
            this.Password = "DEMO";
        }

        /// <summary>
        /// 初期化。
        /// </summary>
        public void Initialize()
        {
        }

        /// <summary>
        /// テーブルを検索します。
        /// </summary>
        public void Search()
        {
            this.builder = new OracleConnectionStringBuilder()
            {
                UserID = this.UserId,
                Password = this.Password,
                DataSource = this.DataSource
            };

            //generator.SearchTable(this.builder);
        }

        /// <summary>
        /// エンティティを生成します。
        /// </summary>
        public void Generate()
        {
            // 暫定対応。あとで入力項目を増やして対応するかも
            var message = new FolderSelectionMessage("FolderDialog.Open")
            {
                Title = "出力フォルダを指定",
                DialogPreference = Helper.IsWindows8OrGreater
                    ? FolderSelectionDialogPreference.CommonItemDialog
                    : FolderSelectionDialogPreference.FolderBrowser,
            };
            this.Messenger.Raise(message);
            if (!Directory.Exists(message.Response))
            {
                return;
            }

            var builder = new OracleConnectionStringBuilder()
            {
                UserID = this.UserId,
                Password = this.Password,
                DataSource = this.DataSource
            };
            generator.Generate(builder);
        }
    }
}

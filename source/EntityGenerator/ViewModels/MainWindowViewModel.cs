using Dapper;
using EntityGenerator.DbProfilers;
using EntityGenerator.Models;
using EntityGenerator.Views.Controls;
using Livet;
using Livet.Messaging.IO;
using Oracle.ManagedDataAccess.Client;
using StackExchange.Profiling;
using StackExchange.Profiling.Data;
using System;
using System.Collections.ObjectModel;
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
        /// テーブル検索クラス。
        /// </summary>
        private readonly TableSearcher searcher;

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

        #region Tables 変更通知プロパティ
        private ObservableCollection<CheckTreeSource> _Tables;
        /// <summary>
        /// テーブル一覧を取得または設定します。
        /// </summary>
        public ObservableCollection<CheckTreeSource> Tables
        {
            get { return _Tables; }
            set
            { 
                if (_Tables != value)
                {
                    _Tables = value;
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
            searcher = new TableSearcher();
            this.generator = new Models.EntityGenerator();
            this.DataSource = "XE";
            this.UserId = "DEMO";
            this.Password = "DEMO";
            //this.Tables = new ObservableCollection<CheckTreeSource>();
            //var item1 = new CheckTreeSource() { Text = "Item1", IsExpanded = true, IsChecked = false };
            //var item11 = new CheckTreeSource() { Text = "Item1-1", IsExpanded = true, IsChecked = false };
            //var item12 = new CheckTreeSource() { Text = "Item1-2", IsExpanded = true, IsChecked = false };
            //var item2 = new CheckTreeSource() { Text = "Item2", IsExpanded = false, IsChecked = false };
            //var item21 = new CheckTreeSource() { Text = "Item2-1", IsExpanded = true, IsChecked = false };
            //this.Tables.Add(item1);
            //this.Tables.Add(item2);
            //item1.Add(item11);
            //item1.Add(item12);
            //item2.Add(item21);
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

            var tableNames = searcher.Search(this.builder);
            //テーブル
            //  + ユーザ
            //      + テーブル名1
            //      + テーブル名2
            //      + テーブル名3
            //      + テーブル名4
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

using EntityGenerator.Models;
using EntityGenerator.Properties;
using EntityGenerator.Views.Controls;
using Livet;
using Livet.Messaging;
using Livet.Messaging.IO;
using Oracle.ManagedDataAccess.Client;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using WpfUtility.Mvvm;

namespace EntityGenerator.ViewModels
{
    /// <summary>
    /// メインウィンドウのViewModel。
    /// </summary>
    public class MainWindowViewModel : ValidatableViewModel
    {
        #region Fields

        /// <summary>
        /// エンティティ生成クラス。
        /// </summary>
        private readonly Models.EntityGenerator generator;

        /// <summary>
        /// 接続文字列。
        /// </summary>
        private OracleConnectionStringBuilder builder;

        #endregion

        #region DataSource 変更通知プロパティ
        private string _DataSource;
        /// <summary>
        /// データソースを取得または設定します。
        /// </summary>
        [Display(Name = "接続先", GroupName = "Connection")]
        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "Validation_Required")]
        public string DataSource
        {
            get { return this._DataSource; }
            set
            {
                if (this._DataSource != value)
                {
                    this._DataSource = value;
                    RaisePropertyChanged();
                }
            }
        }
        #endregion

        #region UserId 変更通知プロパティ
        private string _UserId;
        /// <summary>
        /// ユーザIDを取得または設定します。
        /// </summary>
        [Display(Name = "ユーザID", GroupName = "Connection")]
        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "Validation_Required")]
        public string UserId
        {
            get { return this._UserId; }
            set
            {
                if (this._UserId != value)
                {
                    this._UserId = value;
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
        [Display(Name = "パスワード", GroupName = "Connection")]
        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "Validation_Required")]
        public string Password
        {
            get { return this._Password; }
            set
            {
                if (this._Password != value)
                {
                    this._Password = value;
                    RaisePropertyChanged();
                }
            }
        }
        #endregion

        #region Namespace 変更通知プロパティ
        private string _Namespace;
        /// <summary>
        /// 名前空間を取得または設定します。
        /// </summary>
        public string Namespace
        {
            get { return this._Namespace; }
            set
            {
                if (this._Namespace != value)
                {
                    this._Namespace = value;
                    RaisePropertyChanged();
                }
            }
        }
        #endregion

        #region OutputDestnation 変更通知プロパティ
        private string _OutputDestnation;
        /// <summary>
        /// 出力先を取得または設定します。
        /// </summary>
        public string OutputDestnation
        {
            get { return this._OutputDestnation; }
            set
            {
                if (this._OutputDestnation != value)
                {
                    this._OutputDestnation = value;
                    RaisePropertyChanged();
                }
            }
        }
        #endregion

        #region DatabaseObjects 変更通知プロパティ
        private ObservableCollection<CheckTreeSource> _DatabaseObjects;
        /// <summary>
        /// データベースオブジェクト一覧を取得または設定します。
        /// </summary>
        public ObservableCollection<CheckTreeSource> DatabaseObjects
        {
            get { return this._DatabaseObjects; }
            set
            {
                if (this._DatabaseObjects != value)
                {
                    this._DatabaseObjects = value;
                    RaisePropertyChanged();
                }
            }
        }
        #endregion

        #region CheckedItems 変更通知プロパティ
        private List<CheckTreeSource> _CheckedItems;
        /// <summary>
        /// チェック中のコレクションを取得または設定します。
        /// </summary>
        public List<CheckTreeSource> CheckedItems
        {
            get { return this._CheckedItems; }
            set
            {
                if (this._CheckedItems != value)
                {
                    this._CheckedItems = value;
                    RaisePropertyChanged();
                }
            }
        }
        #endregion

        #region CanGenerate 変更通知プロパティ
        private bool _CanGenerate;
        /// <summary>
        /// 生成できるかどうかを示す値を取得します。
        /// </summary>
        public bool CanGenerate
        {
            get { return this._CanGenerate; }
            set
            {
                if (this._CanGenerate != value)
                {
                    this._CanGenerate = value;
                    RaisePropertyChanged();
                }
            }
        }
        #endregion

        #region IsBusy 変更通知プロパティ
        private bool _IsBusy;
        public bool IsBusy
        {
            get { return this._IsBusy; }
            set
            {
                if (this._IsBusy != value)
                {
                    this._IsBusy = value;
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
#if DEBUG
            this.DataSource = "XE";
            this.UserId = "DEMO";
            this.Password = "DEMO";
            this.Namespace = "Prototype.Entities";
#endif
        }

        /// <summary>
        /// 初期化。
        /// </summary>
        protected override void InitializeCore()
        {
            this.Subscribe(nameof(CheckedItems), () =>
            {
                this.CanGenerate = this.CheckedItems?.Count > 0;
            }).AddTo(this);
        }

        /// <summary>
        /// データベースオブジェクトを検索します。
        /// </summary>
        public async void Search()
        {
            // 接続情報の入力検証
            if (!this.ValidateAll("Connection"))
            {
                this.Messenger.Raise(new InteractionMessage(this.GetErrorProperties().First() + ".Focus"));
                return;
            }

            // 接続情報の生成
            this.builder = new OracleConnectionStringBuilder()
            {
                UserID = this.UserId,
                Password = this.Password,
                DataSource = this.DataSource
            };

            // データオブジェクトの検索
            this.IsBusy = true;
            this.DatabaseObjects = await this.generator.SearchDataObjects(this.builder).ConfigureAwait(false);
            this.IsBusy = false;
        }

        /// <summary>
        /// エンティティを生成します。
        /// </summary>
        public async void Generate()
        {
            //if (string.IsNullOrEmpty(this.OutputDestnation))
            //{
            //    var message = new FolderSelectionMessage("FolderDialog.Open")
            //    {
            //        Title = "出力フォルダを指定",
            //        DialogPreference = Helper.IsWindows8OrGreater
            //            ? FolderSelectionDialogPreference.CommonItemDialog
            //            : FolderSelectionDialogPreference.FolderBrowser,
            //    };
            //    this.Messenger.Raise(message);
            //    if (!Directory.Exists(message.Response))
            //    {
            //        return;
            //    }
            //    this.OutputDestnation = message.Response;
            //}

            this.IsBusy = true;
            await this.generator.Generate(this.OutputDestnation, this.builder, this.Namespace, this.CheckedItems);
            this.IsBusy = false;
        }
    }
}

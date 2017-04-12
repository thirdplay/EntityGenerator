using EntityGenerator.Models;
using EntityGenerator.Properties;
using EntityGenerator.Serialization;
using EntityGenerator.Views.Controls;
using Livet.Messaging;
using Livet.Messaging.IO;
using Oracle.ManagedDataAccess.Client;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Reactive.Disposables;
using WpfUtility.Mvvm;
using Settings = EntityGenerator.Serialization.Settings;

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
        [Display(Name = "名前空間", GroupName = "Generation")]
        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "Validation_Required")]
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
        [Display(Name = "出力先", GroupName = "Generation")]
        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "Validation_Required")]
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

        #region IsExpandedGeneration 変更通知プロパティ
        private bool _IsExpandedGeneration;
        /// <summary>
        /// 生成情報の展開状態を取得または設定します。
        /// </summary>
        public bool IsExpandedGeneration
        {
            get { return this._IsExpandedGeneration; }
            set
            {
                if (this._IsExpandedGeneration != value)
                {
                    this._IsExpandedGeneration = value;
                    RaisePropertyChanged();
                }
            }
        }
        #endregion

        #region IsBusy 変更通知プロパティ
        private bool _IsBusy;
        /// <summary>
        /// ビジー状態を取得または設定します。
        /// </summary>
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
            Disposable.Create(() =>
            {
                // アプリケーション設定の保存
                Settings.Connection.DataSource.Value = this.DataSource;
                Settings.Connection.UserId.Value = this.UserId;
                Settings.Connection.Password.Value = this.Password;
                Settings.Generation.Namespace.Value = this.Namespace;
                Settings.Generation.OutputDestnation.Value = this.OutputDestnation;
                LocalSettingsProvider.Instance.Save();
            }).AddTo(this);

            // アプリケーション設定の読み込み
            this.DataSource = Settings.Connection.DataSource;
            this.UserId = Settings.Connection.UserId;
            this.Password = Settings.Connection.Password;
            this.Namespace = Settings.Generation.Namespace;
            this.OutputDestnation = Settings.Generation.OutputDestnation;
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

            // 検索結果がある場合
            if (this.DatabaseObjects.Count > 0)
            {
                // 生成情報を展開する
                this.IsExpandedGeneration = true;
            }
        }

        /// <summary>
        /// エンティティを生成します。
        /// </summary>
        public async void Generate()
        {
            // 生成情報の入力検証
            if (!this.ValidateAll("Generation"))
            {
                this.Messenger.Raise(new InteractionMessage(this.GetErrorProperties().First() + ".Focus"));
                return;
            }
            // 出力先が存在しない場合はエラー
            if (!Directory.Exists(this.OutputDestnation))
            {
                this.SetError(nameof(this.OutputDestnation), @"出力先が存在しません");
                this.Messenger.Raise(new InteractionMessage(nameof(this.OutputDestnation) + ".Focus"));
                return;
            }

            this.IsBusy = true;
            var result = await this.generator.Generate(this.OutputDestnation, this.builder, this.Namespace, this.CheckedItems).ConfigureAwait(false);
            this.IsBusy = false;

            if (result)
            {
                this.Messenger.Raise(new InformationMessage("エンティティの生成が完了しました。", ProductInfo.Title, "Information"));
            }
        }

        /// <summary>
        /// 出力先選択ダイアログを開きます。
        /// </summary>
        public void OpenOutputDestnationSelectionDialog()
        {
            var message = new FolderSelectionMessage("FolderDialog.Open")
            {
                Title = @"出力フォルダを指定",
                DialogPreference = Helper.IsWindows8OrGreater
                    ? FolderSelectionDialogPreference.CommonItemDialog
                    : FolderSelectionDialogPreference.FolderBrowser,
                SelectedPath = Directory.Exists(this.OutputDestnation) ? this.OutputDestnation : ""
            };
            this.Messenger.Raise(message);

            if (Directory.Exists(message.Response))
            {
                this.OutputDestnation = message.Response;
            }
        }

        /// <summary>
        /// 保存時の設定に戻します。
        /// </summary>
        //private void RevertToSavedSettings()
        //{
        //    this.DataSource = Settings.Connection.DataSource;
        //    this.UserId = Settings.Connection.UserId;
        //    this.Password = Settings.Connection.Password;
        //}
    }
}

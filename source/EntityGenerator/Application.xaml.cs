﻿using EntityGenerator.Configs;
using EntityGenerator.Properties;
using EntityGenerator.Serialization;
using Livet;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows;
using WpfUtility.Lifetime;

namespace EntityGenerator
{
    /// <summary>
    /// App.xaml の相互作用ロジック
    /// </summary>
    sealed partial class Application : IDisposableHolder
    {
        /// <summary>
        /// 基本CompositeDisposable。
        /// </summary>
        private readonly LivetCompositeDisposable compositeDisposable = new LivetCompositeDisposable();

        /// <summary>
        /// 静的コンストラクタ。
        /// </summary>
        static Application()
        {
            AppDomain.CurrentDomain.UnhandledException += (sender, args) => ReportException(sender, args.ExceptionObject as Exception);
        }

        /// <summary>
        /// 起動イベント。
        /// </summary>
        /// <param name="e">イベント引数</param>
        protected override void OnStartup(StartupEventArgs e)
        {
#if !DEBUG
            // 多重起動防止チェック
            var appInstance = new WpfUtility.Desktop.ApplicationInstance().AddTo(this);
            if (appInstance.IsFirst)
#endif
            {
                this.DispatcherUnhandledException += (sender, args) =>
                {
                    ReportException(sender, args.Exception);
                    args.Handled = true;
                };

                // UIDispatcherの設定
                DispatcherHelper.UIDispatcher = this.Dispatcher;

                // Dapperのマッピング設定
                DapperConfig.RegisterMappings("EntityGenerator.Entities");

                // アプリケーション設定の読み込み
                LocalSettingsProvider.Instance.Load();

                // 親メソッド呼び出し
                base.OnStartup(e);
            }
#if !DEBUG
            else
            {
                this.Shutdown();
            }
#endif
        }

        /// <summary>
        /// 終了イベント。
        /// </summary>
        /// <param name="e">イベント引数</param>
        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);

            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        /// <summary>
        /// 例外を表示します。
        /// </summary>
        /// <param name="ex">例外</param>
        public static void ShowException(Exception ex)
        {
            MessageBox.Show(ex.Message, ProductInfo.Title);
        }

        /// <summary>
        /// 例外を報告します。
        /// </summary>
        /// <param name="sender">イベント発生元</param>
        /// <param name="exception">例外</param>
        private static void ReportException(object sender, Exception exception)
        {
            #region const
            const string messageFormat = @"
===========================================================
ERROR, date = {0}, sender = {1},
{2}
";
            const string path = "error.log";
            #endregion

            try
            {
                var message = string.Format(messageFormat, DateTimeOffset.Now, sender, exception);

                Debug.WriteLine(message);
                File.AppendAllText(path, message);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }

            // 終了
            Current.Shutdown();
        }

        #region IDisposableHolder members
        ICollection<IDisposable> IDisposableHolder.CompositeDisposable => this.compositeDisposable;

        /// <summary>
        /// このインスタンスによって使用されているリソースを全て破棄します。
        /// </summary>
        public void Dispose()
        {
            this.compositeDisposable.Dispose();
        }
        #endregion
    }
}

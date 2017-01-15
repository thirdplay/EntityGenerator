using EntityGenerator.Configs;
using Livet;
using System;
using System.Diagnostics;
using System.IO;
using System.Windows;

namespace EntityGenerator
{
    /// <summary>
    /// App.xaml の相互作用ロジック
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// 起動イベント。
        /// </summary>
        /// <param name="e">イベント引数</param>
        protected override void OnStartup(StartupEventArgs e)
        {
            // UIDispatcherの設定
            DispatcherHelper.UIDispatcher = this.Dispatcher;

            // Dapperのマッピング設定
            DapperConfig.RegisterMappings("EntityGenerator.ViewModels");

            // 親メソッド呼び出し
            base.OnStartup(e);
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
        /// 例外報告
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
    }
}

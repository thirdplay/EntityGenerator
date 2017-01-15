using Oracle.ManagedDataAccess.Client;
using StackExchange.Profiling.Data;
using System;
using System.Data;
using System.Diagnostics;
using System.Text;

namespace EntityGenerator.DbProfilers
{
    /// <summary>
    /// トレースDBプロファイラ。
    /// </summary>
    public class TraceDbProfiler : IDbProfiler
    {
        private Stopwatch stopwatch;
        private string commandText;
        private string parameters;
        public object ExecuteType { get; private set; }

        #region IDbProfiler members

        /// <summary>
        /// アクティブ状態を表す値を取得します。
        /// </summary>
        public bool IsActive => true;

        /// <summary>
        /// コマンドが完了された時に呼ばれます。
        /// </summary>
        /// <param name="profiledDbCommand">SQLステートメントを表すインターフェイス</param>
        /// <param name="sqlExecuteType">SQL文のカテゴリ</param>
        /// <param name="reader">取得結果セットを読み込むインターフェイス</param>
        public void ExecuteFinish(IDbCommand profiledDbCommand, SqlExecuteType executeType, System.Data.Common.DbDataReader reader)
        {
            this.commandText = profiledDbCommand.CommandText;
            var sb = new StringBuilder();
            foreach (OracleParameter param in profiledDbCommand.Parameters)
            {
                sb.Append($"{param.ParameterName}: {param.Value},");
            }
            this.parameters = "{" + sb.ToString() + "}";

            if (executeType != SqlExecuteType.Reader)
            {
                stopwatch.Stop();
                Console.WriteLine($"SqlExecute:{commandText}");
                Console.WriteLine($"SqlParameters:{parameters}");
            }
        }

        /// <summary>
        /// コマンドが開始された時に呼ばれます。
        /// </summary>
        /// <param name="profiledDbCommand">SQLステートメントを表すインターフェイス</param>
        /// <param name="sqlExecuteType">SQL文のカテゴリ</param>
        public void ExecuteStart(IDbCommand profiledDbCommand, SqlExecuteType sqlExecuteType)
        {
            stopwatch = Stopwatch.StartNew();
        }

        /// <summary>
        /// エラー発生時に呼ばれます。
        /// </summary>
        /// <param name="profiledDbCommand">SQLステートメントを表すインターフェイス</param>
        /// <param name="sqlExecuteType">SQL文のカテゴリ</param>
        /// <param name="exception">発生した例外</param>
        public void OnError(IDbCommand profiledDbCommand, SqlExecuteType sqlExecuteType, Exception exception)
        {
            Console.WriteLine($"SqlError:{profiledDbCommand.CommandText}");
            Console.WriteLine(exception);
        }

        /// <summary>
        /// Readerが完了した時に呼ばれます。
        /// </summary>
        /// <param name="reader">取得結果セットを読み込むインターフェイス</param>
        public void ReaderFinish(IDataReader reader)
        {
            stopwatch.Stop();
            Console.WriteLine($"SqlExecute:{commandText}");
            Console.WriteLine($"SqlParameters:{parameters}");
        }

        #endregion
    }
}

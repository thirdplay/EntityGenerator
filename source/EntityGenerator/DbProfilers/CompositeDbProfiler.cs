using StackExchange.Profiling.Data;
using System;
using System.Data;
using System.Data.Common;

namespace EntityGenerator.DbProfilers
{
    /// <summary>
    /// DBプロファイラを複合するプロファイラ。
    /// </summary>
    public class CompositeDbProfiler : IDbProfiler
    {
        /// <summary>
        /// プロバイダ。
        /// </summary>
        private readonly IDbProfiler[] _profilers;

        /// <summary>
        /// コンストラクタ。
        /// </summary>
        /// <param name="dbProfilers">DBプロバイダ</param>
        public CompositeDbProfiler(params IDbProfiler[] dbProfilers)
        {
            this._profilers = dbProfilers;
        }

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
        public void ExecuteFinish(IDbCommand profiledDbCommand, SqlExecuteType executeType, DbDataReader reader)
        {
            foreach (var item in this._profilers)
            {
                if (item != null && item.IsActive)
                {
                    item.ExecuteFinish(profiledDbCommand, executeType, reader);
                }
            }
        }

        /// <summary>
        /// コマンドが開始された時に呼ばれます。
        /// </summary>
        /// <param name="profiledDbCommand">SQLステートメントを表すインターフェイス</param>
        /// <param name="sqlExecuteType">SQL文のカテゴリ</param>
        public void ExecuteStart(IDbCommand profiledDbCommand, SqlExecuteType executeType)
        {
            foreach (var item in this._profilers)
            {
                if (item != null && item.IsActive)
                {
                    item.ExecuteStart(profiledDbCommand, executeType);
                }
            }
        }

        /// <summary>
        /// エラー発生時に呼ばれます。
        /// </summary>
        /// <param name="profiledDbCommand">SQLステートメントを表すインターフェイス</param>
        /// <param name="sqlExecuteType">SQL文のカテゴリ</param>
        /// <param name="exception">発生した例外</param>
        public void OnError(IDbCommand profiledDbCommand, SqlExecuteType executeType, Exception exception)
        {
            foreach (var item in _profilers)
            {
                if (item != null && item.IsActive)
                {
                    item.OnError(profiledDbCommand, executeType, exception);
                }
            }
        }

        /// <summary>
        /// Readerが完了した時に呼ばれます。
        /// </summary>
        /// <param name="reader">取得結果セットを読み込むインターフェイス</param>
        public void ReaderFinish(IDataReader reader)
        {
            foreach (var item in _profilers)
            {
                if (item != null && item.IsActive)
                {
                    item.ReaderFinish(reader);
                }
            }
        }

        #endregion
    }
}

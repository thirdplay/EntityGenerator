using EntityGenerator.DbProfilers;
using Oracle.ManagedDataAccess.Client;
using StackExchange.Profiling;
using StackExchange.Profiling.Data;
using System.Data.Common;

namespace EntityGenerator.Models
{
    /// <summary>
    /// Oracleへの接続の生成機能を提供します。
    /// </summary>
    public static class OracleConnectionFactory
    {
        /// <summary>
        /// 新しいOracleデータベースへの接続を作成します。
        /// </summary>
        /// <param name="connectionString">接続文字列</param>
        /// <returns>接続インスタンス</returns>
        public static DbConnection CreateConnection(string connectionString)
        {
            return new ProfiledDbConnection(new OracleConnection(connectionString),
                new CompositeDbProfiler(MiniProfiler.Current, new TraceDbProfiler()));
        }
    }
}
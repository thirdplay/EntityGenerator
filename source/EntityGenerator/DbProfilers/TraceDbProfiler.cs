using Oracle.ManagedDataAccess.Client;
using StackExchange.Profiling.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityGenerator.DbProfilers
{
    public class TraceDbProfiler : IDbProfiler
    {
        //static readonly Logger sqlLogger = NLog.LogManager.GetLogger("Sql");

        public bool IsActive
        {
            get { return true; }
        }

        public object ExecuteType { get; private set; }

        public void OnError(System.Data.IDbCommand profiledDbCommand, SqlExecuteType executeType, System.Exception exception)
        {
            Console.WriteLine($"SqlError:{profiledDbCommand.CommandText}");
        }

        // 大事なのは↓の3つ

        Stopwatch stopwatch;
        string commandText;
        string parameters;

        // コマンドが開始された時に呼ばれる(ExecuteReaderとかExecuteNonQueryとか)
        public void ExecuteStart(System.Data.IDbCommand profiledDbCommand, SqlExecuteType executeType)
        {
            stopwatch = Stopwatch.StartNew();
        }

        // コマンドが完了された時に呼ばれる
        public void ExecuteFinish(System.Data.IDbCommand profiledDbCommand, SqlExecuteType executeType, System.Data.Common.DbDataReader reader)
        {
            commandText = profiledDbCommand.CommandText;
            var sb = new StringBuilder();
            foreach(OracleParameter param in profiledDbCommand.Parameters)
            {
                sb.Append($"{param.ParameterName}: {param.Value},");
            }
            parameters = sb.ToString();

            if (executeType != SqlExecuteType.Reader)
            {
                stopwatch.Stop();
                Console.WriteLine($"SqlExecute:{commandText}");
                Console.WriteLine($"SqlParameters:{parameters}");
            }
        }

        // Readerが完了した時に呼ばれる
        public void ReaderFinish(System.Data.IDataReader reader)
        {
            stopwatch.Stop();
            Console.WriteLine($"SqlExecute:{commandText}");
            Console.WriteLine($"SqlParameters:{parameters}");
        }
    }
}

using Dapper;
using EntityGenerator.DbProfilers;
using EntityGenerator.Models;
using Livet;
using Oracle.ManagedDataAccess.Client;
using StackExchange.Profiling;
using StackExchange.Profiling.Data;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.RegularExpressions;

namespace EntityGenerator.ViewModels
{
    /// <summary>
    /// メインウィンドウのViewModel。
    /// </summary>
    public class MainWindowViewModel : ViewModel
    {
        private Models.EntityGenerator generator = new Models.EntityGenerator();

        /// <summary>
        /// 初期化。
        /// </summary>
        public void Initialize()
        {
        }

        /// <summary>
        /// エンティティを生成します。
        /// </summary>
        public void Generate()
        {
            generator.Generate("DEMO", "DEMO", "XE");
        }
    }
}

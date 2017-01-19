using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityGenerator.Models
{
    /// <summary>
    /// テーブル定義を表すクラス。
    /// </summary>
    public class TableDefinition
    {
        /// <summary>
        /// テーブル名
        /// </summary>
        [Column("TABLE_NAME")]
        public string TableName { get; set; }

        /// <summary>
        /// 所有者
        /// </summary>
        [Column("OWNER")]
        public string Owner { get; set; }

        /// <summary>
        /// カラム名
        /// </summary>
        [Column("COLUMN_NAME")]
        public string ColumnName { get; set; }

        /// <summary>
        /// データタイプ
        /// </summary>
        [Column("DATA_TYPE")]
        public string DataType { get; set; }

        /// <summary>
        /// デフォルト値
        /// </summary>
        [Column("DATA_DEFAULT")]
        public string DataDefault { get; set; }

        /// <summary>
        /// NULLを許容するかどうか
        /// </summary>
        /// <remarks>許容する場合は'Y'</remarks>
        [Column("NULLABLE")]
        public string Nullable { get; set; }

        /// <summary>
        /// カラムID
        /// </summary>
        /// <remarks>1から始まる連番</remarks>
        [Column("COLUMN_ID")]
        public string ColumnId { get; set; }

        /// <summary>
        /// データ長
        /// </summary>
        [Column("DATA_LENGTH")]
        public decimal? DataLength { get; set; }

        /// <summary>
        /// 小数点以下の桁数
        /// </summary>
        /// <remarks>NUMBER型の場合</remarks>
        [Column("DATA_SCALE")]
        public decimal? DataScale { get; set; }

        /// <summary>
        /// 全体の桁数
        /// </summary>
        /// <remarks>NUMBER型の場合</remarks>
        [Column("DATA_PRECISION")]
        public decimal? DataPrecision { get; set; }

        /// <summary>
        /// 制約種別
        /// </summary>
        /// <remarks>プライマリキーの場合は 'P'、 ユニークキーの場合は 'U'</remarks>
        [Column("CONSTRAINT_TYPE")]
        public string ConstraintType { get; set; }

        /// <summary>
        /// 位置
        /// </summary>
        /// <remarks>プライマリキーの場合</remarks>
        [Column("POSITION")]
        public decimal? Position { get; set; }
    }
}

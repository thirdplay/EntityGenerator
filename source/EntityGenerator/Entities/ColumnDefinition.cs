namespace EntityGenerator.Entities
{
    /// <summary>
    /// カラム定義を表すクラス。
    /// </summary>
    public class ColumnDefinition
    {
        /// <summary>
        /// テーブル名
        /// </summary>
        public string TableName { get; set; }

        /// <summary>
        /// テーブルコメント
        /// </summary>
        public string TableComments { get; set; }

        /// <summary>
        /// 所有者
        /// </summary>
        public string Owner { get; set; }

        /// <summary>
        /// カラム名
        /// </summary>
        public string ColumnName { get; set; }

        /// <summary>
        /// カラムコメント
        /// </summary>
        public string ColumnComments { get; set; }

        /// <summary>
        /// データタイプ
        /// </summary>
        public string DataType { get; set; }

        /// <summary>
        /// デフォルト値
        /// </summary>
        public string DataDefault { get; set; }

        /// <summary>
        /// NULLを許容するかどうか
        /// </summary>
        /// <remarks>許容する場合は'Y'</remarks>
        public string Nullable { get; set; }

        /// <summary>
        /// カラムID
        /// </summary>
        /// <remarks>1から始まる連番</remarks>
        public string ColumnId { get; set; }

        /// <summary>
        /// データ長
        /// </summary>
        public decimal? DataLength { get; set; }

        /// <summary>
        /// 小数点以下の桁数
        /// </summary>
        /// <remarks>NUMBER型の場合</remarks>
        public decimal? DataScale { get; set; }

        /// <summary>
        /// 全体の桁数
        /// </summary>
        /// <remarks>NUMBER型の場合</remarks>
        public decimal? DataPrecision { get; set; }

        /// <summary>
        /// 制約種別
        /// </summary>
        /// <remarks>プライマリキーの場合は 'P'、 ユニークキーの場合は 'U'</remarks>
        public string ConstraintType { get; set; }

        /// <summary>
        /// 位置
        /// </summary>
        /// <remarks>プライマリキーの場合</remarks>
        public decimal? Position { get; set; }
    }
}

﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace EntityGenerator.Models
{
    /// <summary>
    /// 汎用補助機能を提供します。
    /// </summary>
    internal static class Helper
    {
        /// <summary>
        /// 静的なコンストラクタ
        /// </summary>
        static Helper()
        {
            var version = Environment.OSVersion.Version;
            IsWindows8OrGreater = (version.Major == 6 && version.Minor >= 2) || version.Major > 6;
        }

        /// <summary>
        /// Windows 8 またはそれ以降のバージョンで動作しているかどうかを確認します。
        /// </summary>
        public static bool IsWindows8OrGreater { get; private set; }

        /// <summary>
        /// デザイナーのコンテキストで実行されているかどうかを取得します。
        /// </summary>
        public static bool IsInDesignMode => DesignerProperties.GetIsInDesignMode(new DependencyObject());
    }
}

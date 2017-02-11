using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace EntityGenerator.Views.Controls
{
    /// <summary>
    /// チェックボックス付きのツリービュー。
    /// </summary>
    public partial class CheckTreeView : TreeView
    {
        /// <summary>
        /// 静的なコンストラクタ。
        /// </summary>
        static CheckTreeView()
        {
            //DefaultStyleKeyProperty.OverrideMetadata(typeof(CheckTreeView), new FrameworkPropertyMetadata(typeof(CheckTreeView)));
        }

        /// <summary>
        /// コンストラクタ。
        /// </summary>
        public CheckTreeView()
        {
        }

        /// <summary>
        /// チェックボックスのクリック処理。
        /// </summary>
        public void CheckBox_Click(object sender, RoutedEventArgs e)
        {
            var checkBox = (CheckBox)sender;
            var source = (CheckTreeSource)checkBox.DataContext;

            source.UpdateChildStatus();
            source.UpdateParentStatus();
        }
    }
}

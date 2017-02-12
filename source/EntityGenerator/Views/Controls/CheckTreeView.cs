using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CheckTreeView), new FrameworkPropertyMetadata(typeof(CheckTreeView)));
        }

        /// <summary>
        /// コンストラクタ。
        /// </summary>
        public CheckTreeView()
        {
        }

        #region CheckedItems 依存関係プロパティ

        /// <summary>
        /// チェック中のコレクションを取得または設定します。
        /// </summary>
        public IList CheckedItems
        {
            get { return (IList)this.GetValue(CheckedItemsProperty); }
            set { this.SetValue(CheckedItemsProperty, value); }
        }

        public static readonly DependencyProperty CheckedItemsProperty =
            DependencyProperty.Register(nameof(CheckedItems), typeof(IList), typeof(CheckTreeView));

        #endregion

        /// <summary>
        /// <see cref="ItemsSource"/> プロパティが変更されたときに呼び出されます。
        /// </summary>
        /// <param name="oldValue">変更前の値</param>
        /// <param name="newValue">変更後の値</param>
        protected override void OnItemsSourceChanged(IEnumerable oldValue, IEnumerable newValue)
        {
            base.OnItemsSourceChanged(oldValue, newValue);
            var genericArgs = newValue.GetType().GetGenericArguments();
            if (genericArgs.Length > 0 && genericArgs[0] == typeof(CheckTreeSource))
            {
                foreach (CheckTreeSource item in newValue)
                {
                    item.PropertyChanged += Item_PropertyChanged;
                }

                Item_PropertyChanged(this, new PropertyChangedEventArgs("IsCheck"));
            }
        }

        /// <summary>
        /// コレクションのプロパティが変更されたときに呼び出されます。
        /// </summary>
        /// <param name="sender">呼び出し元</param>
        /// <param name="e">イベント引数</param>
        private void Item_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var list = new List<CheckTreeSource>();
            var itemsSource = this.ItemsSource as ObservableCollection<CheckTreeSource>;
            if (itemsSource != null)
            {
                foreach (var item in itemsSource)
                {
                    if (item.IsChecked.HasValue && item.IsChecked.Value)
                    {
                        list.Add(item);
                        list.AddRange(item.GetCheckedChild());
                    }
                }
            }
            this.CheckedItems = list;
        }
    }
}

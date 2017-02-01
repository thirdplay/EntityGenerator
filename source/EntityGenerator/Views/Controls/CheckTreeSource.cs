using Livet;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityGenerator.Views.Controls
{
    /// <summary>
    /// チェックボックス付きのツリービューのデータソース。
    /// </summary>
    public class CheckTreeSource : NotificationObject
    {
        #region IsExpanded 変更通知プロパティ
        private bool _IsExpanded;
        /// <summary>
        /// ツリーが展開されているかを表す値を取得または設定します。
        /// </summary>
        public bool IsExpanded
        {
            get { return _IsExpanded; }
            set
            { 
                if (_IsExpanded != value)
                {
                    _IsExpanded = value;
                    RaisePropertyChanged();
                }
            }
        }
        #endregion

        #region IsChecked 変更通知プロパティ
        private bool? _IsChecked;
        /// <summary>
        /// チェック状態を表す値を取得または設定します。
        /// </summary>
        public bool? IsChecked
        {
            get { return _IsChecked; }
            set
            { 
                if (_IsChecked != value)
                {
                    _IsChecked = value;
                    RaisePropertyChanged();
                }
            }
        }
        #endregion

        #region Text 変更通知プロパティ
        private string _Text;
        /// <summary>
        /// 表示文字列を取得または設定します。
        /// </summary>
        public string Text
        {
            get { return _Text; }
            set
            { 
                if (_Text != value)
                {
                    _Text = value;
                    RaisePropertyChanged();
                }
            }
        }
        #endregion

        #region Parent 変更通知プロパティ
        private CheckTreeSource _Parent;
        /// <summary>
        /// 親要素を取得または設定します。
        /// </summary>
        public CheckTreeSource Parent
        {
            get { return _Parent; }
            set
            { 
                if (_Parent != value)
                {
                    _Parent = value;
                    RaisePropertyChanged();
                }
            }
        }
        #endregion

        #region Children 変更通知プロパティ
        private ObservableCollection<CheckTreeSource> _Children;
        /// <summary>
        /// 子要素を取得または設定します。
        /// </summary>
        public ObservableCollection<CheckTreeSource> Children
        {
            get { return _Children; }
            set
            { 
                if (_Children != value)
                {
                    _Children = value;
                    RaisePropertyChanged();
                }
            }
        }
        #endregion

        /// <summary>
        /// 子要素を追加します。
        /// </summary>
        /// <param name="child">子要素</param>
        public void Add(CheckTreeSource child)
        {
            this.Children = this.Children ?? new ObservableCollection<CheckTreeSource>();
            child.Parent = this;
            this.Children.Add(child);
        }

        /// <summary>
        /// 子要素のチェック状態を集計してチェック状態を更新します。
        /// </summary>
        public void UpdateParentStatus()
        {
            if (null != this.Parent)
            {
                int isCheckedNull = 0;
                int isCheckedOn = 0;
                int isCheckedOff = 0;
                if (null != this.Parent.Children)
                {
                    foreach (var item in this.Parent.Children)
                    {
                        if (!item.IsChecked.HasValue)
                        {
                            isCheckedNull += 1;
                        }
                        else if (item.IsChecked.Value)
                        {
                            isCheckedOn += 1;
                        }
                        else
                        {
                            isCheckedOff += 1;
                        }
                    }
                }
                if (0 < isCheckedNull || 0 < isCheckedOn || 0 < isCheckedOff)
                {
                    if (0 < isCheckedNull)
                    {
                        this.Parent.IsChecked = null;
                    }
                    else if ((0 < isCheckedOn) && (0 < isCheckedOff))
                    {
                        this.Parent.IsChecked = null;
                    }
                    else if (0 < isCheckedOn)
                    {
                        this.Parent.IsChecked = true;
                    }
                    else
                    {
                        this.Parent.IsChecked = false;
                    }
                }
                this.Parent.UpdateParentStatus();
            }
        }

        /// <summary>
        /// 子要素のチェック状態を更新します。
        /// </summary>
        public void UpdateChildStatus()
        {
            if (null != this.IsChecked)
            {
                if (null != this.Children)
                {
                    foreach (var item in this.Children)
                    {
                        item.IsChecked = this.IsChecked;
                        item.UpdateChildStatus();
                    }
                }
            }
        }
    }
}

﻿using Livet;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

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
            get { return this._IsExpanded; }
            set
            { 
                if (this._IsExpanded != value)
                {
                    this._IsExpanded = value;
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
            get { return this._IsChecked; }
            set
            { 
                if (this._IsChecked != value)
                {
                    this._IsChecked = value;
                    RaisePropertyChanged();
                }
            }
        }
        #endregion

        #region Header 変更通知プロパティ
        private string _Header;
        /// <summary>
        /// ラベルを取得または設定します。
        /// </summary>
        public string Header
        {
            get { return this._Header; }
            set
            { 
                if (this._Header != value)
                {
                    this._Header = value;
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
            get { return this._Parent; }
            set
            { 
                if (this._Parent != value)
                {
                    this._Parent = value;
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
            get { return this._Children; }
            set
            { 
                if (this._Children != value)
                {
                    this._Children = value;
                    RaisePropertyChanged();
                }
            }
        }
        #endregion

        /// <summary>
        /// コンストラクタ。
        /// </summary>
        /// <param name="header">ラベル</param>
        /// <param name="isChecked">チェック状態</param>
        public CheckTreeSource(string header = "", bool? isChecked = false)
        {
            this.Header = header;
            this.IsChecked = isChecked;
            this.Children = new ObservableCollection<CheckTreeSource>();
        }

        /// <summary>
        /// 子要素を追加します。
        /// </summary>
        /// <param name="child">子要素</param>
        public void Add(CheckTreeSource child)
        {
            child.Parent = this;
            child.PropertyChanged += (sender, e) => RaisePropertyChanged(nameof(this.Children));
            this.Children.Add(child);
        }

        /// <summary>
        /// チェック状態の子要素を取得します。
        /// </summary>
        /// <returns>子要素のリスト</returns>
        public List<CheckTreeSource> GetCheckedChild()
        {
            var result = new List<CheckTreeSource>();
            if (this.Children == null) { return result; }

            if (this.IsChecked.HasValue && !this.IsChecked.Value)
            {
                return result;
            }
            else if (this.IsChecked.HasValue && this.IsChecked.Value)
            {
                result.AddRange(this.Children);
            }
            foreach (var item in this.Children)
            {
                if (item.IsChecked.HasValue && item.IsChecked.Value)
                {
                    result.Add(item);
                }
                result.AddRange(item.GetCheckedChild());
            }

            return result;
        }

        /// <summary>
        /// 子要素のチェック状態を集計してチェック状態を更新します。
        /// </summary>
        public void UpdateParentStatus()
        {
            if (this.Parent == null)
            {
                return;
            }

            int isCheckedNull = 0;
            int isCheckedOn = 0;
            int isCheckedOff = 0;
            if (this.Parent.Children != null)
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
            if (isCheckedNull > 0 || isCheckedOn  > 0 || isCheckedOff > 0)
            {
                if (isCheckedNull > 0)
                {
                    this.Parent.IsChecked = null;
                }
                else if ((isCheckedOn > 0) && (isCheckedOff > 0))
                {
                    this.Parent.IsChecked = null;
                }
                else if (isCheckedOn > 0)
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

        /// <summary>
        /// 子要素のチェック状態を更新します。
        /// </summary>
        public void UpdateChildStatus()
        {
            if (this.IsChecked == null) { return; }
            if (this.Children == null) { return; }
            foreach (var item in this.Children)
            {
                item.IsChecked = this.IsChecked;
                item.UpdateChildStatus();
            }
        }

        /// <summary>
        /// クリック処理。
        /// </summary>
        public void OnClick()
        {
            this.UpdateChildStatus();
            this.UpdateParentStatus();
        }
    }
}

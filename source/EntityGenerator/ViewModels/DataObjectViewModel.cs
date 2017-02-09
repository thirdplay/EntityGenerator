using Livet;
using System.Collections.ObjectModel;

namespace EntityGenerator.ViewModels
{
    /// <summary>
    /// データオブジェクト情報を提供します。
    /// </summary>
    public class DataObjectViewModel : ViewModel
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

        #region Header 変更通知プロパティ
        private string _Header;
        /// <summary>
        /// ラベルを取得または設定します。
        /// </summary>
        public string Header
        {
            get { return _Header; }
            set
            { 
                if (_Header != value)
                {
                    _Header = value;
                    RaisePropertyChanged();
                }
            }
        }
        #endregion

        #region Parent 変更通知プロパティ
        private DataObjectViewModel _Parent;
        /// <summary>
        /// 親要素を取得または設定します。
        /// </summary>
        public DataObjectViewModel Parent
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
        private ObservableCollection<DataObjectViewModel> _Children;
        /// <summary>
        /// 子要素を取得または設定します。
        /// </summary>
        public ObservableCollection<DataObjectViewModel> Children
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
        /// コンストラクタ。
        /// </summary>
        /// <param name="header">ラベル</param>
        /// <param name="isChecked">チェック状態</param>
        public DataObjectViewModel(string header = "", bool? isChecked = false)
        {
            this.Header = header;
            this.IsChecked = isChecked;
            this.Children = new ObservableCollection<DataObjectViewModel>();
        }

        /// <summary>
        /// 子要素を追加します。
        /// </summary>
        /// <param name="child">子要素</param>
        public void Add(DataObjectViewModel child)
        {
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

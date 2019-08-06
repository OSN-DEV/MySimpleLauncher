using MySimpleLauncher.Model;
using System;
using System.Windows;
using System.Windows.Controls;

namespace MySimpleLauncher.UI {
    /// <summary>
    /// カテゴリ名編集
    /// </summary>
    public partial class EditCategoryName : Window {

        #region Declaration
        private string _displayName = "";

        public bool IsChanged {
            get { return !(this._displayName == this.cDisplayName.Text); }
        }
        public string DisplayName {
            get { return this.cDisplayName.Text; }
        }
        #endregion

        #region Constructor
        private EditCategoryName() {
            InitializeComponent();
        }

        internal EditCategoryName(Window owner) : this() {
            this.Owner = owner;
        }

        internal EditCategoryName(Window owner, CategoryModel model) : this(owner) {
            this.Owner = owner;
            this.DataContext = model;
            this._displayName = model.DisplayName;
        }
        #endregion

        #region Event
        private void Window_Activated(object sender, EventArgs e) {
            this.cDisplayName.Focus();
            this.cDisplayName.SelectAll();
        }
        private void cDisplayName_TextChanged(object sender, TextChangedEventArgs e) {
            this.cOK.IsEnabled = (0 < this.cDisplayName.Text.Length);
        }

        private void cOK_Click(object sender, RoutedEventArgs e) {
            this.DialogResult = true;
        }
        #endregion
    }
}

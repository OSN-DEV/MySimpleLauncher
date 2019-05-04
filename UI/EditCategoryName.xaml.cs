using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using MySimpleLauncher.Model;

namespace MySimpleLauncher.UI {
    /// <summary>
    /// EditCategoryName.xaml の相互作用ロジック
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

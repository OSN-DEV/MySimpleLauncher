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
using MySimpleLauncher.Data;
using System.Collections.ObjectModel;
using MyLib.Data.Sqlite;

namespace MySimpleLauncher.UI {
    /// <summary>
    /// Category削除確認
    /// </summary>
    public partial class CategoryDeleteConfirm : Window {

        #region Declaration
        private CategoryModel _categoryModel;
        private ObservableCollection<CategoryModel> _categoryList = new ObservableCollection<CategoryModel>();
        #endregion

        #region Public Property
        internal long MoveCategory {
            private set; get;
        } = -1;
        #endregion

        #region Constructor

        private CategoryDeleteConfirm() {
            InitializeComponent();
        }

        internal CategoryDeleteConfirm(Window owner, CategoryModel currentModel, ObservableCollection<CategoryModel> categoryList) : this() {
            this.Owner = owner;
            this.cConfirmMessage.Text = string.Format("[{0}]を削除しますか？", currentModel.DisplayName);
            this._categoryModel = currentModel;
            this.cCategoryList.DataContext = this._categoryList;
            
            foreach(var model in categoryList) {
                if (model.Id == currentModel.Id) {
                    continue;
                }
                this._categoryList.Add(model);
            }
            this.Initialize();
        }
        #endregion

        #region Event
        private void OK_Click(object sender, RoutedEventArgs e) {
            if (true == this.cMoveItems.IsChecked) {
                this.MoveCategory = (this.cCategoryList.SelectedItem as CategoryModel).Id;
            }
            this.DialogResult = true;
        }


        private void DeleteItems_Checked(object sender, RoutedEventArgs e) {
            if (null != cCategoryList) {
                this.cCategoryList.IsEnabled = !(true == this.cDeleteItems.IsChecked ? true : false);
                if (this.cCategoryList.IsEnabled) {
                    this.cCategoryList.Focus();
                }
            }
        }
        #endregion

        #region Private Method
        /// <summary>
        /// initialize screen
        /// </summary>
        private void Initialize() {
            if (0 == this._categoryList.Count) {
                this.cMoveItems.IsEnabled = false;
                return;
            }
            this.cCategoryList.SelectedIndex = 0;
            this.cCategoryList.IsEnabled = false;
        }
        #endregion

    }
}

using Microsoft.Win32;
using MySimpleLauncher.Model;
using MySimpleLauncher.Util;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media.Imaging;

namespace MySimpleLauncher.UI {
    /// <summary>
    /// ItemData.xaml の相互作用ロジック
    /// </summary>
    public partial class EditItem : Window {

        #region Declaration
        private ItemModel _model = null;

        internal ItemModel Model { get { return this._model; } }

        #endregion

        #region Constructor
        private EditItem() {
            InitializeComponent();
        }

        internal EditItem(Window owner, bool isReadOnly = false, ItemModel model = null) : this() {
            this.Owner = owner;
            if (null == model) {
                this._model = new ItemModel();
            } else {
                this._model = model;
            }
            this._model.IsReadOnly = isReadOnly;
            this.DataContext = this._model;
            this.cEdit.Visibility = isReadOnly ? Visibility.Visible : Visibility.Hidden;
            this.cOK.Visibility = isReadOnly ? Visibility.Hidden : Visibility.Visible;
            this.cOK.IsEnabled = (0 < this._model.DisplayName?.Length);
            this.DataContext = this._model;
        }
        #endregion

        #region Event
        private void Window_Loaded(object sender, RoutedEventArgs e) {
            this.cDisplayName.Focus();
        }

        private void Ok_Click(object sender, RoutedEventArgs e) {
            this._model.Icon = (BitmapImage)this.cIcon.Source;
            this.DialogResult = true;
        }

        private void Edit_Click(object sender, RoutedEventArgs e) {
            this._model.IsReadOnly = false;
            this.cEdit.Visibility = Visibility.Hidden;
            this.cOK.Visibility = Visibility.Visible;
        }

        private void DisplayName_TextChanged(object sender, TextChangedEventArgs e) {
            this.cOK.IsEnabled = (0 < this._model.DisplayName.Length);
        }

        private void FilePath_TextValueChanged(object sender, EventArgs e) {
            if (null != this.cIcon.Source) {
                return;
            }

            if (0 < this.cFilePath.Text.Length && System.IO.File.Exists(this.cFilePath.Text)) {
                using (var icon = System.Drawing.Icon.ExtractAssociatedIcon(this.cFilePath.Text)) {
                    this.cIcon.Source = Imaging.CreateBitmapSourceFromHIcon(icon.Handle, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
                }
            } else {
            }
        }

        private void Icon_MouseDown(object sender, MouseButtonEventArgs e) {
            var dialog = new OpenFileDialog();
            //dialog.Filter = "jpg|*.jpg|png|*.png";
            dialog.Filter = "jpg, png|*.jpg;*.png";
            dialog.FilterIndex = 0;
            dialog.Title = "アイコン画像を選択";
            if (true != dialog.ShowDialog()) {
                return;
            }
            this.cIcon.Source = AppCommon.GetBitmapImage(dialog.FileName);
        }
        #endregion

        #region Private Method
        #endregion
    }
}

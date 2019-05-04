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
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using MySimpleLauncher.Model;
using Microsoft.Win32;
using MySimpleLauncher.Util;

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
            dialog.Filter = "jpg, png|*.jpg, *.png";
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

        public void Test() {

            _model.IsReadOnly = true;

            _model.DisplayName = "display name";
            _model.FilePath = "file path";
            _model.User = "user";
            _model.Password = "password";
            _model.Comment = "comment";

            _model.UserId = "user id";
            _model.LastName = "last name";
            _model.FirstName = "first name";
            _model.Mail = "mail address";
            _model.Birthday = "9999/99/99";
            _model.Prefecture = "北海道";
            _model.Address1 = "Address1";
            _model.Address2 = "Address2";
            _model.Tel = "999-9999-9999";
            _model.SecretQuestion1 = "question1";
            _model.SecretAnswer1 = "answer1";
            _model.SecretQuestion2 = "question2";
            _model.SecretAnswer2 = "answer2";
            _model.SecretQuestion3 = "question3";
            _model.SecretAnswer3 = "answer3";

            _model.UserKey1 = "user key1";
            _model.UserValue1 = "user value1";
            _model.UserKey2 = "user key2";
            _model.UserValue2 = "user value2";
            _model.UserKey3 = "user key3";
            _model.UserValue3 = "user value3";
            _model.UserKey4 = "user key4";
            _model.UserValue4 = "user value4";
            _model.UserKey5 = "user key5";
            _model.UserValue5 = "user value5";
            _model.UserKey6 = "user key6";
            _model.UserValue6 = "user value6";
            _model.UserKey7 = "user key7";
            _model.UserValue7 = "user value7";
            _model.UserKey8 = "user key8";
            _model.UserValue8 = "user value8";
            _model.UserKey9 = "user key9";
            _model.UserValue9 = "user value9";
            _model.UserKey10 = "user key10";
            _model.UserValue10 = "user value10";

            this.DataContext = _model;

        }


    }
}

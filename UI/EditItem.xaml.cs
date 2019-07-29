using Microsoft.Win32;
using MySimpleLauncher.Model;
using MySimpleLauncher.Util;
using System;
using System.IO;
using System.Net;
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
        private readonly ItemModel _model = null;

        internal ItemModel Model { get { return this._model; } }

        private readonly System.Windows.Forms.WebBrowser _browser = new System.Windows.Forms.WebBrowser();
        private bool _loadingIcon = false;
        #endregion

        #region Constructor
        private EditItem() {
            InitializeComponent();
            this._browser.DocumentCompleted += Browser_DocumentCompleted;
        }

        internal EditItem(Window owner, bool isReadOnly = false, ItemModel model = null) : this() {
            this.Owner = owner;
            var localModel = new ItemModel();
            if (null == model) {
                this._model = new ItemModel();
            } else {
                this._model = model;
                localModel.CopyFrom(model);
            }
            localModel.IsReadOnly = isReadOnly;
            this.DataContext = localModel;
            this.cIcon.ByteSource = localModel.Icon;
            this.cEdit.Visibility = isReadOnly ? Visibility.Visible : Visibility.Hidden;
            this.cOK.Visibility = isReadOnly ? Visibility.Hidden : Visibility.Visible;
            this.cOK.IsEnabled = (0 < this._model.DisplayName?.Length);
        }
        #endregion

        #region Event
        private void Window_Loaded(object sender, RoutedEventArgs e) {
            this.cDisplayName.Focus();
        }

        private void Ok_Click(object sender, RoutedEventArgs e) {
            var model = this.DataContext as ItemModel;
            this._model.CopyFrom(model);
            this._model.Icon = (byte[])this.cIcon.ByteSource;
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
            this.cIcon.ClearSource();
            if (0 < this.cFilePath.Text.Length && this.cFilePath.Text.StartsWith("http")) {
                if (!this._loadingIcon) {
                    this._loadingIcon = true;
                    this._browser.Navigate(this.cFilePath.Text);
                }
            } else if (0 < this.cFilePath.Text.Length && File.Exists(this.cFilePath.Text)) {
                this.cIcon.SetAppIcon(this.cFilePath.Text);
            } else if (0 < this.cFilePath.Text.Length && Directory.Exists(this.cFilePath.Text)) {
                this.cIcon.SetDirectoryIcon(this.cFilePath.Text);
            } else {
                ((ItemModel)this.DataContext).Icon = null;
            }
        }

        private void Icon_MouseDown(object sender, MouseButtonEventArgs e) {
            var dialog = new OpenFileDialog() {
                Filter = "jpg, png|*.jpg;*.png",
                FilterIndex = 0,
                Title = "アイコン画像を選択"
            };
            if (true != dialog.ShowDialog()) {
                return;
            }
            this.cIcon.SetImageFile(dialog.FileName);
        }

        private void Browser_DocumentCompleted(object sender, System.Windows.Forms.WebBrowserDocumentCompletedEventArgs e) {
            if (e.Url.AbsolutePath != (sender as System.Windows.Forms.WebBrowser).Url.AbsolutePath) {
                return;
            }
            string iconUrl = "";
            foreach (System.Windows.Forms.HtmlElement linkTag in this._browser.Document.GetElementsByTagName("link")) {
                string relAttribute = linkTag.GetAttribute("rel");
                string url;
                if (relAttribute == "shortcut icon" || relAttribute == "icon") {
                    url = linkTag.GetAttribute("href");
                    if (url.StartsWith("http")) {        //完全なURLの場合
                        iconUrl = url;
                    } else if (url.StartsWith("/")) {    //絶対パスの場合
                        //iconUrl = "http://" + e.Url.Host + url;
                        iconUrl = e.Url.Scheme + "://" + e.Url.Host + url;
                    } else {                                //相対パスの場合
                        iconUrl = e.Url.ToString() + url;
                    }
                    break;
                }
            }
            if (0 == iconUrl.Length) {
                // iconUrl = "http://" + e.Url.Host + "/favicon.ico";
                iconUrl = e.Url.Scheme + "://" + e.Url.Host + "/favicon.ico";
            }
            SetFavicon(iconUrl);
        }
        #endregion

        #region Private Method
        private void SetFavicon(string url) {
            try {
                using (var stream = new MemoryStream()) {
                    var resStream = WebRequest.Create(url).GetResponse().GetResponseStream();
                    resStream.CopyTo(stream);
                    this.cIcon.ByteSource = stream.GetBuffer();
                }
            } catch (Exception ex) {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
            this._loadingIcon = false;
        }

        #endregion
    }
}

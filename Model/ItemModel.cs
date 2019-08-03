using MySimpleLauncher.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;


namespace MySimpleLauncher.Model {
    internal class ItemModel : INotifyPropertyChanged {

        #region Declaration
        private bool _isReadOnly = false;
        public event PropertyChangedEventHandler PropertyChanged;

        public bool IsReadOnly {
            set {
                this._isReadOnly = value;
                this.OnPropertyChanged("IsReadOnly");
            }
            get {
                return this._isReadOnly;
            }
        }

        public long Id { set; get; }
        public long CategoryId { set; get; }
        public int RowOrder { set; get; }

        // General
        private string _displayName = "";
        public string DisplayName {
            set {
                this._displayName = value;
                this.OnPropertyChanged("DisplayName");
            }
            get {
                return this._displayName;
            }
        }

        private byte[] _icon = null;
        public byte[] Icon {
            set {
                this._icon = value;
                this.OnPropertyChanged("Icon");
                this.OnPropertyChanged("IconSource");
            }
            get {
                return this._icon;
            }
        }

        public IList<byte> IconList {
            set {
                this._icon = value.ToArray();
                this.OnPropertyChanged("Icon");
            }
            get {
                return (IList<byte>)this._icon.ToList();
            }
        }
        public BitmapSource IconSource {
            set { }
            get {
                if (null == this._icon) {
                    return null;
                } else {
                    try {
                        // バインドの処理がまともに動かないのでひとまずやっつけで作成
                        using (var stream = new System.IO.MemoryStream(this._icon)) {
                            var bitmapDecoder = BitmapDecoder.Create(
                                                stream,
                                                BitmapCreateOptions.PreservePixelFormat,
                                                BitmapCacheOption.OnLoad);
                            var writable = new WriteableBitmap(bitmapDecoder.Frames.Single());
                            writable.Freeze();
                            return (BitmapSource)writable;
                        }
                    } catch {
                        return null;
                    }
                }
            }
        }
        public string FilePath { set; get; }
        public string User { set; get; }
        public string Password { set; get; }
        public string Comment { set; get; }

        // Account
        public string UserId { set; get; }
        public string FirstName { set; get; }
        public string LastName { set; get; }
        public string NickName { set; get; }
        public string Sex { set; get; }
        public string Mail { set; get; }
        public string Birthday { set; get; }
        public string ZipCode { set; get; }
        public string Prefecture { set; get; }
        public string Address1 { set; get; }
        public string Address2 { set; get; }
        public string Tel { set; get; }
        public string SecretQuestion1 { set; get; }
        public string SecretAnswer1 { set; get; }
        public string SecretQuestion2 { set; get; }
        public string SecretAnswer2 { set; get; }
        public string SecretQuestion3 { set; get; }
        public string SecretAnswer3 { set; get; }

        // Others
        public string UserKey1 { set; get; }
        public string UserValue1 { set; get; }
        public string UserKey2 { set; get; }
        public string UserValue2 { set; get; }
        public string UserKey3 { set; get; }
        public string UserValue3 { set; get; }
        public string UserKey4 { set; get; }
        public string UserValue4 { set; get; }
        public string UserKey5 { set; get; }
        public string UserValue5 { set; get; }
        public string UserKey6 { set; get; }
        public string UserValue6 { set; get; }
        public string UserKey7 { set; get; }
        public string UserValue7 { set; get; }
        public string UserKey8 { set; get; }
        public string UserValue8 { set; get; }
        public string UserKey9 { set; get; }
        public string UserValue9 { set; get; }
        public string UserKey10 { set; get; }
        public string UserValue10 { set; get; }

        // Additional Information
        public string DateModified { set; get; }
        public string Type { set; get; }
        public string Size { set; get; }
        #endregion

        #region Constructor
        internal ItemModel() { }

        internal ItemModel(ItemsTable table) {
            this.Id = table.Id;
            this.CategoryId = table.CategoryId;
            this.DisplayName = table.DisplayName;
            this.Icon = table.Icon;
            this.FilePath = table.FilePath;
            this.User = table.User;
            this.Password = table.Password;
            this.Comment = table.Comment;
            this.UserId = table.UserId;
            this.FirstName = table.FirstName;
            this.LastName = table.LastName;
            this.NickName = table.NickName;
            this.Sex = table.Sex;
            this.Mail = table.Mail;
            this.Birthday = table.Birthday;
            this.ZipCode = table.ZipCode;
            this.Prefecture = table.Prefecture;
            this.Address1 = table.Address1;
            this.Address2 = table.Address2;
            this.Tel = table.Tel;
            this.SecretQuestion1 = table.SecretQuestion1;
            this.SecretAnswer1 = table.SecretAnswer1;
            this.SecretQuestion2 = table.SecretQuestion2;
            this.SecretAnswer2 = table.SecretAnswer2;
            this.SecretQuestion3 = table.SecretQuestion3;
            this.SecretAnswer3 = table.SecretAnswer3;
            this.UserKey1 = table.UserKey1;
            this.UserValue1 = table.UserValue1;
            this.UserKey2 = table.UserKey2;
            this.UserValue2 = table.UserValue2;
            this.UserKey3 = table.UserKey3;
            this.UserValue3 = table.UserValue3;
            this.UserKey4 = table.UserKey4;
            this.UserValue4 = table.UserValue4;
            this.UserKey5 = table.UserKey5;
            this.UserValue5 = table.UserValue5;
            this.UserKey6 = table.UserKey6;
            this.UserValue6 = table.UserValue6;
            this.UserKey7 = table.UserKey7;
            this.UserValue7 = table.UserValue7;
            this.UserKey8 = table.UserKey8;
            this.UserValue8 = table.UserValue8;
            this.UserKey9 = table.UserKey9;
            this.UserValue9 = table.UserValue9;
            this.UserKey10 = table.UserKey10;
            this.UserValue10 = table.UserValue10;
            this.RowOrder = table.RowOrder;
        }
        #endregion

        #region Public Method
        public override string ToString() {
            return this.DisplayName;
        }
        #endregion

        #region Internal Method
        internal void CopyFrom(ItemModel source) {
            this.Id = source.Id;
            this.CategoryId = source.CategoryId;
            this.DisplayName = source.DisplayName;
            this.Icon = source.Icon;
            this.FilePath = source.FilePath;
            this.User = source.User;
            this.Password = source.Password;
            this.Comment = source.Comment;
            this.UserId = source.UserId;
            this.FirstName = source.FirstName;
            this.LastName = source.LastName;
            this.NickName = source.NickName;
            this.Sex = source.Sex;
            this.Mail = source.Mail;
            this.Birthday = source.Birthday;
            this.ZipCode = source.ZipCode;
            this.Prefecture = source.Prefecture;
            this.Address1 = source.Address1;
            this.Address2 = source.Address2;
            this.Tel = source.Tel;
            this.SecretQuestion1 = source.SecretQuestion1;
            this.SecretAnswer1 = source.SecretAnswer1;
            this.SecretQuestion2 = source.SecretQuestion2;
            this.SecretAnswer2 = source.SecretAnswer2;
            this.SecretQuestion3 = source.SecretQuestion3;
            this.SecretAnswer3 = source.SecretAnswer3;
            this.UserKey1 = source.UserKey1;
            this.UserValue1 = source.UserValue1;
            this.UserKey2 = source.UserKey2;
            this.UserValue2 = source.UserValue2;
            this.UserKey3 = source.UserKey3;
            this.UserValue3 = source.UserValue3;
            this.UserKey4 = source.UserKey4;
            this.UserValue4 = source.UserValue4;
            this.UserKey5 = source.UserKey5;
            this.UserValue5 = source.UserValue5;
            this.UserKey6 = source.UserKey6;
            this.UserValue6 = source.UserValue6;
            this.UserKey7 = source.UserKey7;
            this.UserValue7 = source.UserValue7;
            this.UserKey8 = source.UserKey8;
            this.UserValue8 = source.UserValue8;
            this.UserKey9 = source.UserKey9;
            this.UserValue9 = source.UserValue9;
            this.UserKey10 = source.UserKey10;
            this.UserValue10 = source.UserValue10;
            this.RowOrder = source.RowOrder;
        }

        internal string GetSearchKeyword() {
            var keyword = new System.Text.StringBuilder();
            keyword.Append(this.DisplayName)
                .Append(this.FilePath)
                .Append(this.User)
                .Append(this.Comment)
                .Append(this.UserId)
                .Append(this.FirstName)
                .Append(this.LastName)
                .Append(this.Mail);
            return keyword.ToString();
        }
        #endregion

        #region Protecte Method
        protected void OnPropertyChanged(string name) {
            PropertyChangedEventHandler handler = PropertyChanged;
            handler?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        #endregion
    }
}

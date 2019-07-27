using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using MySimpleLauncher.Data;
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
        public string DisplayName { set; get; }
        private BitmapImage _icon = null;
        public BitmapImage Icon {
            set {
                this._icon = value;
                this.OnPropertyChanged("Icon");
            }
            get {
                return this._icon;
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

        #region Internal Method
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

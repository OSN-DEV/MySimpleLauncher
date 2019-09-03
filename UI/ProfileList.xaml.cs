using Microsoft.Win32;
using MyLib.Data.Sqlite;
using MyLib.File;
using MySimpleLauncher.Data;
using MySimpleLauncher.Model;
using MySimpleLauncher.Util;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace MySimpleLauncher.UI {
    // https://anderson02.com/cs/wpf/wpf-10/
    // https://rksoftware.wordpress.com/2016/12/11/001-66/
    // https://www.sejuku.net/blog/57146
    // https://codeday.me/jp/qa/20190124/190629.html

    public partial class ProfileList : Window {
        #region Declaration
        private ObservableCollection<ProfileModel> _model;
        private List<ProfileModel> _deleteModels = new List<ProfileModel>();

        internal ProfileModel CurrentModel {
            get; private set;
        } = null;

        internal ProfileModel SelectedModel {
            get; private set;
        }

        internal List<ProfileModel> DeleteModels {
            get { return this._deleteModels;  }
        }
        #endregion

        #region Constructor
        internal ProfileList() {
            InitializeComponent();
        }

        internal ProfileList(ProfileModel model) {
            InitializeComponent();
            this.CurrentModel = model;
        }
        #endregion

        #region Event
        private void Window_Loaded(object sender, RoutedEventArgs e) {
            var database = new SystemDatabase();
            if (!database.CreateDatabaseIfNeed()) {
                AppCommon.ShowErrorMsg(string.Format(ErrorMsg.FailToCreate, "system database"));
                this.Close();
            }
            this._model = new ObservableCollection<ProfileModel>();
            using (var table = new ProfilesTable()) {
                table.OpenDatabase();
                table.SelectAll();
                while (table.Read()) {
                    this._model.Add(new ProfileModel {
                        Id = table.Id,
                        FilePath = table.FilePath,
                        DisplayName = table.DisplayName,
                        RowOrder = table.RowOrder
                    });
                }
            }

            this.cProfileList.DataContext = this._model;
        }

        private void Create_Click(object sender, RoutedEventArgs e) {
            var dialog = new SaveFileDialog();
            dialog.FileName = "profile.db";
            dialog.Filter = "db ファイル|*.db";
            dialog.FilterIndex = 0;
            dialog.AddExtension = true;
            if (true != dialog.ShowDialog()) {
                return;
            }
            var profile = new FileOperator(dialog.FileName);
            if (this.IsRegisterProfile(profile.FilePath)) {
                return;
            }
            profile.Delete();
            using (var database = new ProfileDatabase(profile.FilePath)) {
                try {
                    database.SetPassWord(ProfileDatabase.Password);
                    database.Open();
                    database.BeginTrans();
                    if (database.ExecuteNonQuery(CategoriesTable.CreateTable()) < 0) {
                        AppCommon.ShowErrorMsg(string.Format(ErrorMsg.FailToCreate, "categories table"));
                        return;
                    }
                    if (database.ExecuteNonQuery(ItemsTable.CreateTable()) < 0) {
                        AppCommon.ShowErrorMsg(string.Format(ErrorMsg.FailToCreate, "items table"));
                        return;
                    }
                    database.CommitTrans();
                } catch (Exception ex) {
                    AppCommon.ShowErrorMsg(ex.Message);
                } finally {
                    database.RollbackTrans();
                }
            }
            if (this.InsertProfile(profile)) {
                this.cProfileList.DataContext = this._model;
            }
        }

        private void Open_Click(object sender, RoutedEventArgs e) {
            var dialog = new OpenFileDialog();
            dialog.Filter = "db ファイル|*.db";
            dialog.FilterIndex = 0;
            dialog.Title = "プロファイルを選択";
            if (true != dialog.ShowDialog()) {
                return;

            }
            var profile = new FileOperator(dialog.FileName);
            if (this.IsRegisterProfile(profile.FilePath)) {
                return;
            }
            if (this.InsertProfile(profile)) {
                this.cProfileList.DataContext = this._model;
            }
        }

        private void Edit_Click(object sender, RoutedEventArgs e) {
            var model = ((Button)sender).Tag as ProfileModel;
            var dialog = new EditProfileName(this, model);
            if (true != dialog.ShowDialog()) {
                dialog.Close();
                return;
            }
            if (dialog.IsChanged) {
                using (var table = new ProfilesTable()) {
                    if (0 == table.UpdateDisplayNameById(model)) {
                        AppCommon.ShowErrorMsg(string.Format(ErrorMsg.FailToUpdate, "display name"));
                    }
                }
                if (null != this.CurrentModel && model.Id == this.CurrentModel.Id) {
                    this.CurrentModel.DisplayName = model.DisplayName;
                }
            }
            dialog.Close();
        }

        private void Delete_Click(object sender, RoutedEventArgs e) {
            var model = ((Button)sender).Tag as ProfileModel;
            var dialog = new ProfileDeleteConfirm(this, model);
            if (true != dialog.ShowDialog()) {
                dialog.Close();
                return;
            }

            using (var table = new ProfilesTable()) {
                if (table.DeleteById(model.Id) < 0) {
                    AppCommon.ShowErrorMsg(string.Format(ErrorMsg.FailToDelete, "profile"));
                }
                if (true == dialog.DeleteFile) {
                    new FileOperator(model.FilePath).Delete();
                }
            }
            dialog.Close();
            this._model.Remove(model);
            this._deleteModels.Add(model);
        }

        private void ProfileList_MouseDoubleClick(object sender, MouseButtonEventArgs e) {
            var dataContext = ((FrameworkElement)e.OriginalSource).DataContext;
            if (dataContext is ProfileModel) {
                this.SelectedModel = (ProfileModel)dataContext;
                this.DialogResult = true;
            }
        }
        #endregion


        #region Private Method
        /// <summary>
        /// check whether profile is already registered.
        /// </summary>
        /// <param name="profile">file</param>
        /// <returns>true:registered, false:otherwise</returns>
        private bool IsRegisterProfile(string profile) {
            bool result = false;
            var table = new ProfilesTable();
            if (0 < table.SelectCountByFilePath(profile)) {
                AppCommon.ShowErrorMsg(ErrorMsg.ProfileIsRegistered);
                result = true;
            }
            return result;
        }

        /// <summary>
        /// insert profile data and add list if success.
        /// </summary>
        /// <param name="profile">profile information</param>
        /// <returns>true:success, false:otherwise</returns>
        private bool InsertProfile(FileOperator profile) {
            var model = new ProfileModel();
            model.FilePath = profile.FilePath;
            model.DisplayName = profile.NameWithoutExtension;
            var table = new ProfilesTable();

            var result = false;
            var id = table.Insert(model);
            if (id < 0) {
                AppCommon.ShowErrorMsg(string.Format(ErrorMsg.FailToInsert, "profile data"));
            } else {
                // model.Id = table.SelectIdByFilePath(model.FilePath);
                model.Id = id;
                result = true;
                this._model.Add(model);
            }
            return result;
        }

        #endregion


    }
}


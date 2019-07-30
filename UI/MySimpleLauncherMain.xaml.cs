
using MyLib.File;
using MyLib.Util;
using MySimpleLauncher.Component;
using MySimpleLauncher.Data;
using MySimpleLauncher.Model;
using MySimpleLauncher.Util;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

namespace MySimpleLauncher.UI {
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MySimpleLauncherMain : Window {

        #region Declaration
        private readonly CustomContextMenu _categoryMenu = new CustomContextMenu();
        private readonly CustomContextMenu _itemMenu = new CustomContextMenu();
        private readonly AppSettings _settings;
        private ProfileModel _currentProfile = null;
        private readonly System.Windows.Forms.NotifyIcon _notifyIcon = new System.Windows.Forms.NotifyIcon();

        private readonly ObservableCollection<CategoryModel> _categoryList = new ObservableCollection<CategoryModel>();
        private readonly ObservableCollection<ItemModel> _itemList = new ObservableCollection<ItemModel>();

        private ProfileDatabase _profileDatabase;
        private static MySimpleLauncherMain _self;
        private static string _assemblyName;
        private static bool _findSelf = false;

        private delegate bool SendVKeyNativeDelegate(uint keyStroke, NativeMethods.KeySet keyset);
        private readonly HotKeyHelper _hotkey;
        private bool _allowClosing = false;

        /// <summary>
        /// Id for context menu
        /// </summary>
        private enum ContextMenuId : int {
            Add,
            Edit,
            Detail,
            Delete
        }
        #endregion

        #region Constructor
        public MySimpleLauncherMain() {
            InitializeComponent();
            this.CreateContextMenu();
            this.SetUpNotifyIcon();
            this._settings = AppSettings.GetInstance();
            this._settings.Load();
            _self = this;
            _assemblyName = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name;

            this._hotkey = new HotKeyHelper(this);
            this._hotkey.Register(ModifierKeys.Control | ModifierKeys.Shift | ModifierKeys.Alt,
                                  Key.A,
                                  (_, __) => {
                                      if (!this.ShowInTaskbar) {
                                          NotifyMenuShow_Click(null, null);
                                      }
                                  }
                                  );
        }
        #endregion

        #region Event
        /// <summary>
        /// window loaded
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MySimpleLauncherMain_Loaded(object sender, RoutedEventArgs e) {
            this.cCategoryList.DataContext = this._categoryList;
            this.cItemList.DataContext = this._itemList;

            this.cDisplayMenuShowStatusBar.IsChecked = this._settings.ShowStatusBar;
            this.cFileStatus.Visibility = this._settings.ShowStatusBar ? Visibility.Visible : Visibility.Collapsed;

            this.cCategoryList.ContextMenu = this._categoryMenu;
            this.cItemList.ContextMenu = this._itemMenu;

            if (this._settings.CurrentProfileId < 0) {
                this.ClearScreen();
            } else {
                using (var table = new ProfilesTable()) {
                    table.SelectById(this._settings.CurrentProfileId);
                    if (table.Read()) {
                        this._currentProfile = new ProfileModel(table);
                        if (System.IO.File.Exists(table.FilePath)) {
                            this.cProfileList.Content = this._currentProfile.DisplayName;
                            this._profileDatabase = new ProfileDatabase(table.FilePath);

                            this.ShowCategoryList();
                            this.cCategoryList.SelectedIndex = this._settings.CategoryListSelectedIndex;
                        } else {
                            AppCommon.ShowErrorMsg(string.Format(ErrorMsg.ProfileNotFound, table.FilePath));
                            this.ClearScreen();
                        }
                    } else {
                        this.ClearScreen();
                    }
                }
            }
        }

        /// <summary>
        /// window closing
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MySimpleLauncherMain_Closing(object sender, System.ComponentModel.CancelEventArgs e) {
            if (!this._allowClosing) {
                e.Cancel = true;
                this.WindowState = WindowState.Minimized;
                this.ShowInTaskbar = false;
                this._notifyIcon.Visible = true;
            }
        }

        /// <summary>
        /// window closed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MySimpleLauncherMain_Closed(object sender, EventArgs e) {
            this._hotkey.Dispose();
        }

        /// <summary>
        /// [File] - [Exit]
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FileMenuExit_Click(object sender, RoutedEventArgs e) {
            this._allowClosing = true;
            this.Close();
        }

        /// <summary>
        /// [Display] - [Show Status Bar]
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DisplayMenuShowStatusBar_Click(object sender, RoutedEventArgs e) {
            if (this.cDisplayMenuShowStatusBar.IsChecked) {
                this.cFileStatus.Visibility = Visibility.Visible;
            } else {
                this.cFileStatus.Visibility = Visibility.Collapsed;
            }
            this._settings.ShowStatusBar = this.cDisplayMenuShowStatusBar.IsChecked;
            this._settings.Save();
        }

        /// <summary>
        /// key down
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MySimpleLauncherMain_KeyDown(object sender, KeyEventArgs e) {
            if (e.Key == Key.A && IsModifierPressed(ModifierKeys.Control) && IsModifierPressed(ModifierKeys.Shift)) {
                e.Handled = true;
                if (null != this.cCategoryList.SelectedItem) {
                    this.ItemContextMenuAdd_Click(null, null);
                }
            } else if (IsModifierPressed(ModifierKeys.Alt)) {
                this.cAppMenu.Visibility = (this.cAppMenu.Visibility == Visibility.Visible) ? Visibility.Collapsed : Visibility.Visible;
            } else if (e.Key == Key.Escape) {
                this.WindowState = WindowState.Minimized;
                this.ShowInTaskbar = false;
                this._notifyIcon.Visible = true;
            }
        }

        /// <summary>
        /// profile list click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ProfileList_Click(object sender, RoutedEventArgs e) {
            var profileList = new ProfileList(this._currentProfile) {
                Owner = this
            };
            if (true == profileList.ShowDialog()) {
                this.ClearScreen();
                if (!System.IO.File.Exists(profileList.SelectedModel.FilePath)) {
                    AppCommon.ShowErrorMsg(string.Format(ErrorMsg.ProfileNotFound, profileList.SelectedModel.FilePath));
                    return;
                }
                this._profileDatabase = new ProfileDatabase(profileList.SelectedModel.FilePath);
                this._currentProfile = profileList.SelectedModel;
                this.cProfileList.Content = this._currentProfile.DisplayName;
                this._settings.CurrentProfileId = this._currentProfile.Id;
                this._settings.Save();
                this._categoryMenu.IsEnabled = true;

                this.ShowCategoryList();
            } else if (0 < profileList.DeleteModels.Count) {
                var result = profileList.DeleteModels.Where<ProfileModel>(x => x.Id == this._settings.CurrentProfileId);
                if (null != result) {
                    this.ClearScreen();
                }
            } else {
                if (null != profileList.CurrentModel) {
                    this._currentProfile = profileList.CurrentModel;
                    this.cProfileList.Content = this._currentProfile.DisplayName;
                }
            }
            profileList.Close();
        }

        /// <summary>
        /// category list key down
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CategoryList_KeyDown(object sender, KeyEventArgs e) {
            var selectedIndex = this.cCategoryList.SelectedIndex;
            bool updateOrder = false;
            if (Key.U == e.Key && this.IsModifierPressed(ModifierKeys.Shift) && this.IsModifierPressed(ModifierKeys.Control)) {
                if (selectedIndex <= 0) {
                    return;
                }
                var model = this._categoryList[selectedIndex];
                this._categoryList.Remove(model);
                selectedIndex--;
                this._categoryList.Insert(selectedIndex, model);
                updateOrder = true;
                e.Handled = true;
            } else if (Key.D == e.Key && this.IsModifierPressed(ModifierKeys.Shift) && this.IsModifierPressed(ModifierKeys.Control)) {
                if (-1 == selectedIndex || this._categoryList.Count - 1 <= selectedIndex) {
                    return;
                }
                var model = this._categoryList[selectedIndex];
                this._categoryList.Remove(model);
                selectedIndex++;
                this._categoryList.Insert(selectedIndex, model);
                updateOrder = true;
                e.Handled = true;
            }
            if (updateOrder) {
                using (var table = new CategoriesTable(this._profileDatabase)) {
                    if (table.UpdateRowOrdersByIds(this._categoryList) == 0) {
                        AppCommon.ShowErrorMsg(string.Format(ErrorMsg.FailToUpdate, "category"));
                    } else {
                        this.cCategoryList.SelectedIndex = selectedIndex;
                    }
                }
            }
        }

        /// <summary>
        /// category list selection changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CategoryList_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            var model = this.cCategoryList.SelectedItem as CategoryModel;
            this.ShowItemList(model);
            this._settings.CategoryListSelectedIndex = this.cCategoryList.SelectedIndex;
            this._settings.Save();
            this._itemMenu.IsEnabled = true;
        }

        /// <summary>
        /// category context menu add click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CategoryContextMenuAdd_Click(object sender, RoutedEventArgs e) {
            var dialog = new EditCategoryName(this);
            if (true != dialog.ShowDialog()) {
                return;
            }
            using (var table = new CategoriesTable(this._profileDatabase)) {
                var model = new CategoryModel() {
                    DisplayName = dialog.DisplayName,
                    RowOrder = this._categoryList.Count,
                };
                model.DisplayName = dialog.DisplayName;
                model.RowOrder = this._categoryList.Count;
                model.Id = table.Insert(model);
                if (model.Id < 0) {
                    AppCommon.ShowErrorMsg(string.Format(ErrorMsg.FailToInsert, "category"));
                    return;
                }
                this._categoryList.Add(model);
            }
        }

        /// <summary>
        /// category context menu edit click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CategoryContextMenuEdit_Click(object sender, RoutedEventArgs e) {
            if (!((this._categoryMenu.Tag as ListViewItem)?.DataContext is CategoryModel model)) {
                return;
            }
            var dialog = new EditCategoryName(this, model);
            if (true != dialog.ShowDialog()) {
                return;
            }
            using (var table = new CategoriesTable(this._profileDatabase)) {
                model.DisplayName = dialog.DisplayName;
                if (0 == table.UpdateById(model)) {
                    AppCommon.ShowErrorMsg(string.Format(ErrorMsg.FailToUpdate, "category"));
                    return;
                }
            }
        }

        /// <summary>
        /// category context menu delete click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CategoryContextMenuDelete_Click(object sender, RoutedEventArgs e) {
            if (!((this._categoryMenu.Tag as ListViewItem)?.DataContext is CategoryModel model)) {
                return;
            }
            var dialog = new CategoryDeleteConfirm(this, model, this._categoryList);
            if (true != dialog.ShowDialog()) {
                return;
            }

            try {
                this._profileDatabase.Open();
                this._profileDatabase.BeginTrans();

                using (var categoriesTable = new CategoriesTable(this._profileDatabase))
                using (var itemsTable = new ItemsTable(this._profileDatabase)) {
                    if (0 == categoriesTable.DeleteById(model)) {
                        AppCommon.ShowErrorMsg(string.Format(ErrorMsg.FailToDelete, "Category"));
                    }
                    if (-1 == dialog.MoveCategory) {
                        itemsTable.DeleteByCateogyId(model.Id);
                    } else {
                        itemsTable.UpdateCategoryIdByCategoryId(model.Id, dialog.MoveCategory);
                    }
                    this._profileDatabase.CommitTrans();
                }
            } finally {
                this._profileDatabase.RollbackTrans();
                this._profileDatabase.Close();
            }
        }

        /// <summary>
        /// category context menu opening
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CategoryList_ContextMenuOpening(object sender, ContextMenuEventArgs e) {
            if (!this._categoryMenu.IsEnabled) {
                e.Handled = true;
                return;
            }
            var model = this.cCategoryList.GetItemAt(Mouse.GetPosition(this.cCategoryList));
            var isItemSelected = (null != model);
            this._categoryMenu.SetMenuItemEnabled((int)ContextMenuId.Edit, isItemSelected);
            this._categoryMenu.SetMenuItemEnabled((int)ContextMenuId.Delete, isItemSelected);
            this._categoryMenu.Tag = model;
        }

        /// <summary>
        /// item context menu add click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ItemContextMenuAdd_Click(object sender, RoutedEventArgs e) {

            var dialog = new EditItem(this);
            if (true != dialog.ShowDialog()) {
                return;
            }
            using (var table = new ItemsTable(this._profileDatabase)) {
                var model = dialog.Model;
                model.CategoryId = ((CategoryModel)this.cCategoryList.SelectedItem).Id;
                model.Id = table.Insert(model);
                if (model.Id < 0) {
                    AppCommon.ShowErrorMsg(string.Format(ErrorMsg.FailToInsert, "item"));
                    return;
                }
                this._itemList.Add(model);
            }
        }

        /// <summary>
        /// item context menu edit click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ItemContextMenuEdit_Click(object sender, RoutedEventArgs e) {
            var model = (this._itemMenu.Tag as ListViewItem)?.DataContext as ItemModel;
            var dialog = new EditItem(this, false, model);
            if (true != dialog.ShowDialog()) {
                return;
            }
            this.UpdateItems(model);
        }

        /// <summary>
        /// item context menu detaill click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ItemContextMenuDetail_Click(object sender, RoutedEventArgs e) {
            var model = (this._itemMenu.Tag as ListViewItem)?.DataContext as ItemModel;
            var dialog = new EditItem(this, true, model);
            if (true != dialog.ShowDialog()) {
                return;
            }
            if (!model.IsReadOnly) {
                this.UpdateItems(model);
            }
        }

        /// <summary>
        /// item context menu delete click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ItemContextMenuDelete_Click(object sender, RoutedEventArgs e) {
            var model = (this._itemMenu.Tag as ListViewItem)?.DataContext as ItemModel;
            var dialog = new ItemDeleteConfirm(this, model);
            if (true != dialog.ShowDialog()) {
                dialog.Close();
                return;
            }

            using (var table = new ItemsTable(this._profileDatabase)) {
                if (table.DeleteById(model.Id) < 0) {
                    AppCommon.ShowErrorMsg(string.Format(ErrorMsg.FailToDelete, "profile"));
                }
                if (true == dialog.DeleteFile) {
                    new FileOperator(model.FilePath).Delete();
                }
            }
            dialog.Close();
            this._itemList.Remove(model);
        }


        private void ItemList_ContextMenuOpening(object sender, ContextMenuEventArgs e) {
            if (null == this.cCategoryList.SelectedItem) {
                e.Handled = true;
            } else {
                var item = this.cItemList.GetItemAt(Mouse.GetPosition(this.cItemList));
                var isItemSelected = (null != item);
                this._itemMenu.SetMenuItemEnabled((int)ContextMenuId.Edit, isItemSelected);
                this._itemMenu.SetMenuItemEnabled((int)ContextMenuId.Delete, isItemSelected);
                this._itemMenu.SetMenuItemEnabled((int)ContextMenuId.Detail, isItemSelected);
                this._itemMenu.Tag = item;
            }
        }

        private void ItemList_Drop(object sender, DragEventArgs e) {
            if (!(e.Data.GetData(DataFormats.FileDrop) is string[] files)) {
                return;
            }
            using (var table = new ItemsTable(this._profileDatabase)) {
                table.OpenDatabase();
                table.BeginTrans();
                foreach (var file in files) {
                    var fileUtil = FileUtil.Create(file);
                    var model = new ItemModel();
                    if (fileUtil.IsFile) {
                        model.DisplayName = ((FileOperator)fileUtil).NameWithoutExtension;
                    } else {
                        model.DisplayName = fileUtil.Name;
                    }
                    if (fileUtil.Exists()) {
                        if (fileUtil.IsDirectory) {
                            //model.Icon = AppCommon.GetBitmapImageFromFolderIcon(fileUtil.FilePath);
                        } else {
                            //model.Icon = AppCommon.GetBitmapImageFromAppIcon(fileUtil.FilePath);
                        }
                    }
                    model.FilePath = file;
                    model.CategoryId = ((CategoryModel)this.cCategoryList.SelectedItem).Id;
                    model.Id = table.Insert(model);
                    if (model.Id < 0) {
                        AppCommon.ShowErrorMsg(string.Format(ErrorMsg.FailToInsert, "item"));
                        return;
                    }
                    this._itemList.Add(model);
                }
                table.CommitTrans();
            }
        }

        private void ItemList_MouseDoubleClick(object sender, MouseButtonEventArgs e) {
            if (!(cItemList.GetItemAt(Mouse.GetPosition(this.cItemList))?.DataContext is ItemModel model)) {
                return;
            }
            MyLibUtil.RunApplication(model.FilePath, false);
        }

        private void ItemList_KeyDown(object sender, KeyEventArgs e) {
            if (e.Key == Key.U && IsModifierPressed(ModifierKeys.Control)) {
                e.Handled = true;
                var itemsModel = this.cItemList.SelectedItem as ItemModel;
                MyLibUtil.RunApplication(itemsModel.FilePath, false);
            } else if (e.Key == Key.V && IsModifierPressed(ModifierKeys.Control)) {
                e.Handled = true;
                Task.Run(() => {
                    System.Threading.Thread.Sleep(200);
                    _findSelf = false;
                    NativeMethods.EnumWindows(new NativeMethods.EnumWindowsDelegate(EnumWindowCallBack), IntPtr.Zero);
                });
            }
        }


        private void ItemList_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            this.cFileInfo.Text = "";
            if (!(this.cItemList.SelectedItem is ItemModel model)) {
                return;
            }
            var util = FileUtil.Create(model.FilePath);
            if (null != util) {
                var status = new StringBuilder();
                status.Append("Modifeied : " + util.LastWriteTimeString);
                status.Append(" ");
                if (!util.IsDirectory) {
                    status.Append("Size : " + util.FileSizeString);
                }
                this.cFileInfo.Text = status.ToString();
            }
        }

        private void NotifyMenuShow_Click(object sender, EventArgs e) {
            this.WindowState = WindowState.Normal;
            this.ShowInTaskbar = true;
            this._notifyIcon.Visible = false;

        }

        private void NotifyMenuExit_Click(object sender, EventArgs e) {
            this._notifyIcon.Dispose();
            System.Windows.Application.Current.Shutdown();
        }
        #endregion

        #region Public Method
        /// <summary>
        /// send key
        /// </summary>
        /// <param name="hWnd"></param>
        public void SendKey(IntPtr hWnd) {
            NativeMethods.SetForegroundWindow(hWnd);
            DoEvents();

            var model = this.cItemList.SelectedItem as ItemModel;
            var sendStrings = new StringBuilder();
            sendStrings.Append(model.User)
                .Append("{TAB}")
                .Append(model.Password);
            System.Windows.Forms.SendKeys.SendWait(sendStrings.ToString());
            DoEvents();
            System.Windows.Forms.SendKeys.SendWait("{ENTER}");
        }

        #endregion

        #region Private Method
        /// <summary>
        /// Create context menu for CategoryList
        /// </summary>
        private void CreateContextMenu() {
            CustomContextMenu.MenuItemData CreateItem(int id, string text, RoutedEventHandler handler, bool isDelete = false) {
                var item = new CustomContextMenu.MenuItemData {
                    Id = id,
                    Text = text
                };
                item.Click += handler;
                if (isDelete) {
                    item.ForeGround = "#FF0000";
                }
                return item;
            }

            this._categoryMenu.AddItem(CreateItem((int)ContextMenuId.Add, "Add", CategoryContextMenuAdd_Click));
            this._categoryMenu.AddItem(CreateItem((int)ContextMenuId.Edit, "Edit", CategoryContextMenuEdit_Click));
            this._categoryMenu.AddSeparator();
            this._categoryMenu.AddItem(CreateItem((int)ContextMenuId.Delete, "Delete", CategoryContextMenuDelete_Click, true));

            this._itemMenu.AddItem(CreateItem((int)ContextMenuId.Add, "Add", ItemContextMenuAdd_Click));
            this._itemMenu.AddItem(CreateItem((int)ContextMenuId.Edit, "Edit", ItemContextMenuEdit_Click));
            this._itemMenu.AddItem(CreateItem((int)ContextMenuId.Detail, "Detail", ItemContextMenuDetail_Click));
            this._itemMenu.AddSeparator();
            this._itemMenu.AddItem(CreateItem((int)ContextMenuId.Delete, "Delete", ItemContextMenuDelete_Click, true));
        }

        /// <summary>
        /// set up notify icon
        /// </summary>
        private void SetUpNotifyIcon() {
            this._notifyIcon.Text = "My Simple Launcher";
            this._notifyIcon.Icon = new System.Drawing.Icon("app_icon.ico");
            var menu = new System.Windows.Forms.ContextMenuStrip();
            var menuItemShow = new System.Windows.Forms.ToolStripMenuItem {
                Text = "show"
            };
            menuItemShow.Click += this.NotifyMenuShow_Click;
            menu.Items.Add(menuItemShow);

            var menuItemExit = new System.Windows.Forms.ToolStripMenuItem {
                Text = "exit"
            };
            menuItemExit.Click += this.NotifyMenuExit_Click;
            menu.Items.Add(menuItemExit);

            this._notifyIcon.ContextMenuStrip = menu;
            this._notifyIcon.Visible = false;
        }

        /// <summary>
        /// celar show data and set enabled each control
        /// </summary>
        private void ClearScreen() {
            this.cProfileList.Content = Wording.NoProfile;
            //this.cCategoryList.DataContext = null;
            this._categoryMenu.IsEnabled = false;
            //this.cItemList.DataContext = null;
            this._itemMenu.IsEnabled = false;

            this._categoryList.Clear();
            this._itemList.Clear();
        }

        /// <summary>
        /// show category list
        /// </summary>
        private void ShowCategoryList() {
            using (var table = new CategoriesTable(this._profileDatabase)) {
                table.SelectAll();
                while (table.Read()) {
                    this._categoryList.Add(new CategoryModel(table));
                }
            }
        }

        /// <summary>
        /// show item list
        /// </summary>
        private void ShowItemList(CategoryModel model) {
            this._itemList.Clear();
            if (null == model) {
                return;
            }

            using (var table = new ItemsTable(this._profileDatabase)) {
                table.SelectAllById(model.Id);
                while (table.Read()) {
                    this._itemList.Add(new ItemModel(table));
                }
            }
        }

        /// <summary>
        /// update items table
        /// </summary>
        /// <param name="model">model</param>
        private void UpdateItems(ItemModel model) {
            using (var table = new ItemsTable(this._profileDatabase)) {
                int count = table.UpdateById(model);
                if (0 == count) {
                    AppCommon.ShowErrorMsg(string.Format(ErrorMsg.FailToUpdate, "item"));
                    return;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hWnd"></param>
        /// <param name="lparam"></param>
        /// <returns></returns>
        private static bool EnumWindowCallBack(IntPtr hWnd, IntPtr lparam) {
            if (!NativeMethods.IsWindowVisible(hWnd)) {
                return true;
            }

            int length = NativeMethods.GetWindowTextLength(hWnd);
            if (0 < length) {
                NativeMethods.GetWindowThreadProcessId(hWnd, out uint processId);
                var proccess = Process.GetProcessById((int)processId);

                if (_findSelf) {
                    _self.Dispatcher.BeginInvoke(new Action(() => {
                    _self.SendKey(hWnd);
                    }));

                    _findSelf = false;
                    return false;
                } else if (proccess.ProcessName == _assemblyName) {
                    _findSelf = true;
                }
            }
            return true;
        }

        /// <summary>
        /// same as visual basic doevent
        /// </summary>
        private static void DoEvents() {
            DispatcherFrame frame = new DispatcherFrame();
            var callback = new DispatcherOperationCallback(ExitFrames);
            Dispatcher.CurrentDispatcher.BeginInvoke(DispatcherPriority.Background, callback, frame);
            Dispatcher.PushFrame(frame);
        }
        private static object ExitFrames(object obj) {
            ((DispatcherFrame)obj).Continue = false;
            return null;
        }

        /// <summary>
        /// check if modifiered key is pressed
        /// </summary>
        /// <param name="key">modifier key</param>
        /// <returns>true:modifiered key is pressed, false:otherwise</returns>
        private bool IsModifierPressed(ModifierKeys key) {
            return (Keyboard.Modifiers & key) != ModifierKeys.None;
        }

        #endregion

    }
}

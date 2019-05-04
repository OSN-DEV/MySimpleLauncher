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
using System.Windows.Navigation;
using System.Windows.Shapes;
using MySimpleLauncher.Component;
using MySimpleLauncher.Data;
using MySimpleLauncher.Model;
using MySimpleLauncher.Util;
using System.Collections.ObjectModel;
using MyLib.Data.Sqlite;
using MyLib.Util;
using MyLib.File;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Windows.Threading;

namespace MySimpleLauncher.UI {
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MySimpleLauncherMain : Window {


        #region Declaration
        // https://qiita.com/katabamisan/items/081547f42512e93a31ab
        // http://d.hatena.ne.jp/maeyan/20091227/1261848549
        // https://ja.stackoverflow.com/questions/29568/c%E3%81%AB%E3%81%A6%E9%9D%9E%E3%82%A2%E3%82%AF%E3%83%86%E3%82%A3%E3%83%96%E3%81%AE%E3%83%97%E3%83%AD%E3%82%BB%E3%82%B9%E3%81%B8%E3%82%AD%E3%83%BC%E5%85%A5%E5%8A%9B%E3%81%97%E3%81%9F%E3%81%84
        private CustomContextMenu _categoryMenu = new CustomContextMenu();
        private CustomContextMenu _itemMenu = new CustomContextMenu();
        private AppSettings _settings;
        private ProfileModel _currentProfile = null;
        private System.Windows.Forms.NotifyIcon _notifyIcon = new System.Windows.Forms.NotifyIcon();

        private ObservableCollection<CategoryModel> _categoryList = new ObservableCollection<CategoryModel>();
        private ObservableCollection<ItemModel> _itemList = new ObservableCollection<ItemModel>();

        private Database _profileDatabase;
        private static MySimpleLauncherMain _self;
        private static string _assemblyName;
        private static bool _findSelf = false;

        public delegate bool EnumWindowsDelegate(IntPtr hWnd, IntPtr lparam);
        private delegate bool SendVKeyNativeDelegate(uint keyStroke, KeySet keyset);
        const uint WM_KEYDOWN = 0x100;

        private static class NativeMethods {
            [DllImport("user32.dll")]
            [return: MarshalAs(UnmanagedType.Bool)]
            public extern static bool EnumWindows(EnumWindowsDelegate lpEnumFunc, IntPtr lparam);

            [DllImport("user32")]
            public static extern bool IsWindowVisible(IntPtr hWnd);

            [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
            public static extern int GetWindowTextLength(IntPtr hWnd);

            [DllImport("User32.dll")]
            internal static extern uint GetWindowThreadProcessId(IntPtr hWnd, [Out] out uint lpdwProcessId);

            [DllImport("user32.dll")]
            public static extern IntPtr GetMessageExtraInfo();

            [DllImport("user32.dll")]
            [return: MarshalAs(UnmanagedType.Bool)]
            public static extern bool SetForegroundWindow(IntPtr hWnd);

            [DllImport("User32.dll", EntryPoint = "SendInput", SetLastError = true)]
            internal static extern uint SendInput32(uint nInputs, INPUT32[] pInputs, int cbSize);

            [DllImport("User32.dll", EntryPoint = "SendInput", SetLastError = true)]
            internal static extern uint SendInput64(uint nInputs, INPUT64[] pInputs, int cbSize);
        }

        /// <summary>
        /// Id for context menu
        /// </summary>
        private enum ContextMenuId : int {
            Add,
            Edit,
            Detail,
            Delete
        }

        private static class Flags {
            public const int None = 0x00;
            public const int KeyDown = 0x00;
            public const int KeyUp = 0x02;
            public const int ExtendeKey = 0x01;
            public const int Unicode = 0x04;
            public const int ScanCode = 0x08;
        }

        private static class KeyStroke {
            public const int KeyDown = 0x100;
            public const int KeyUp = 0x101;
            public const int SysKeyDown = 0x104;
            public const int SysKeyup = 0x105;
        }

        private class KeySet {
            public ushort VirtualKey;
            public ushort ScanCode;
            public uint Flag;
            public KeySet(byte[] pair) : this(pair, Flags.None) {
            }
            public KeySet(byte[] pair, uint flag) {
                this.VirtualKey = KeySetPair.VirtualKey(pair);
                this.ScanCode = KeySetPair.ScanCode(pair);
                this.Flag = flag;
            }
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        internal struct MOUSEINPUT32 {
            public uint __Unused0; // See INPUT32 structure

            public int X;
            public int Y;
            public uint MouseData;
            public uint Flags;
            public uint Time;
            public IntPtr ExtraInfo;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        internal struct KEYBDINPUT32 {
            public uint __Unused0; // See INPUT32 structure

            public ushort VirtualKey;
            public ushort ScanCode;
            public uint Flags;
            public uint Time;
            public IntPtr ExtraInfo;
        }

        [StructLayout(LayoutKind.Explicit)]
        internal struct INPUT32 {
            [FieldOffset(0)]
            public uint Type;
            [FieldOffset(0)]
            public MOUSEINPUT32 Mouse;
            [FieldOffset(0)]
            public KEYBDINPUT32 Keyboard;
        }

        // INPUT.KI (40). vk: 8, sc: 10, fl: 12, t: 16, ex: 24
        [StructLayout(LayoutKind.Explicit, Size = 40)]
        internal struct INPUT64 {
            [FieldOffset(0)]
            public uint Type;
            [FieldOffset(8)]
            public ushort VirtualKey;
            [FieldOffset(10)]
            public ushort ScanCode;
            [FieldOffset(12)]
            public uint Flags;
            [FieldOffset(16)]
            public uint Time;
            [FieldOffset(24)]
            public IntPtr ExtraInfo;
        }

        private static class InputType {
            public const uint Mouse = 0;
            public const uint Keyboard = 1;
            public const uint Hardware = 2;
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

            // https://blog.hiros-dot.net/?page_id=4104
            this.cCategoryList.ContextMenu = this._categoryMenu;
            this.cItemList.ContextMenu = this._itemMenu;

            if (this._settings.CurrentProfileId < 0) {
                this.ClearScreen();
            } else {
                using (var table = new ProfilesTable()) {
                    table.SelectById(this._settings.CurrentProfileId);
                    if (table.Read()) {
                        if (System.IO.File.Exists(table.FilePath)) {
                            this._currentProfile = new ProfileModel(table);
                            this.cProfileList.Content = this._currentProfile.DisplayName;
                            this._profileDatabase = new Database(table.FilePath);

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
            e.Cancel = true;
            this.WindowState = WindowState.Minimized;
            this.ShowInTaskbar = false;
            this._notifyIcon.Visible = true;
        }

        /// <summary>
        /// profile list click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ProfileList_Click(object sender, RoutedEventArgs e) {
            var profileList = new ProfileList(this._currentProfile);
            profileList.Owner = this;
            if (true == profileList.ShowDialog()) {
                this.ClearScreen();
                if (!System.IO.File.Exists(profileList.SelectedModel.FilePath)) {
                    AppCommon.ShowErrorMsg(string.Format(ErrorMsg.ProfileNotFound, profileList.SelectedModel.FilePath));
                    return;
                }
                this._profileDatabase = new Database(profileList.SelectedModel.FilePath);
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
        /// category list selection changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CategoryList_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            var model = this.cCategoryList.SelectedItem as CategoryModel;
            this.ShowItemList(model);
            this._settings.CategoryListSelectedIndex = this.cCategoryList.SelectedIndex;
            this._settings.Save();
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
                var model = new CategoryModel();
                model.DisplayName = dialog.DisplayName;
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
            var model = (this._categoryMenu.Tag as ListViewItem)?.DataContext as CategoryModel;
            if (null == model) {
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
            // todo : show delete confirm dialog
            // todo : move items if need
            // todo : delete items if need
            // todo : delete category
            var model = (this._categoryMenu.Tag as ListViewItem)?.DataContext as CategoryModel;
            if (null == model) {
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
                var item = cItemList.GetItemAt(Mouse.GetPosition(this.cItemList));
                var isItemSelected = (null != item);
                this._itemMenu.SetMenuItemEnabled((int)ContextMenuId.Edit, isItemSelected);
                this._itemMenu.SetMenuItemEnabled((int)ContextMenuId.Delete, isItemSelected);
                this._itemMenu.SetMenuItemEnabled((int)ContextMenuId.Detail, isItemSelected);
                this._itemMenu.Tag = item;
            }
        }

        private void ItemList_Drop(object sender, DragEventArgs e) {
            var files = e.Data.GetData(DataFormats.FileDrop) as string[];
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
            var model = this.cItemList.GetItemAt(Mouse.GetPosition(this.cItemList))?.DataContext as ItemModel;
            if (null == model) {
                return;
            }
            MyLib.Util.MyLibUtil.RunApplication(model.FilePath, false);
        }

        private void ItemList_KeyDown(object sender, KeyEventArgs e) {
            if (e.Key == Key.U && ((Keyboard.Modifiers & ModifierKeys.Control) != ModifierKeys.None)) {
                e.Handled = true;
                var itemsModel = this.cItemList.SelectedItem as ItemModel;
                MyLibUtil.RunApplication(itemsModel.FilePath, false);
            } else if (e.Key == Key.V && ((Keyboard.Modifiers & ModifierKeys.Control) != ModifierKeys.None)) {
                e.Handled = true;
                Task.Run(() => {
                    System.Threading.Thread.Sleep(200);
                    _findSelf = false;
                    NativeMethods.EnumWindows(new EnumWindowsDelegate(EnumWindowCallBack), IntPtr.Zero);
                });
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
                var item = new CustomContextMenu.MenuItemData();
                item.Id = id;
                item.Text = text;
                item.Click += handler;
                if (isDelete) {
                    item.ForeGround = "#FF0000";
                    item.MouseOverColor = "#FF9999";
                    item.PressedColor = "#FF0000";
                }
                return item;
            }

            this._categoryMenu.AddItem(CreateItem((int)ContextMenuId.Add, "Add", CategoryContextMenuAdd_Click));
            this._categoryMenu.AddItem(CreateItem((int)ContextMenuId.Edit, "Edit", CategoryContextMenuEdit_Click));
            this._categoryMenu.AddSeparator();
            this._categoryMenu.AddItem(CreateItem((int)ContextMenuId.Delete, "Delete", CategoryContextMenuDelete_Click));

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
            var menuItemShow = new System.Windows.Forms.ToolStripMenuItem();
            menuItemShow.Text = "show";
            menuItemShow.Click += this.NotifyMenuShow_Click;
            menu.Items.Add(menuItemShow);

            var menuItemExit = new System.Windows.Forms.ToolStripMenuItem();
            menuItemExit.Text = "exit";
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
        /// get next windows handle
        /// </summary>
        /// <returns></returns>
        private void GetNextWindowsHandle() {
            NativeMethods.EnumWindows(new EnumWindowsDelegate(EnumWindowCallBack), IntPtr.Zero);
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
                uint processId;
                NativeMethods.GetWindowThreadProcessId(hWnd, out processId);
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
        /// send key for 32bit
        /// </summary>
        /// <param name="keyStroke"></param>
        /// <param name="keyset"></param>
        /// <returns></returns>
        private bool SendVKeyNative32(INPUT32[] inputs) {
            var count = (uint)inputs.Length;
            if (NativeMethods.SendInput32(count, inputs, Marshal.SizeOf(typeof(INPUT32))) != count) {
                return false;
            }
            DoEvents();
            return true;
        }

        /// <summary>
        /// Create INPUT32 Instance
        /// </summary>
        /// <param name="keyStroke"></param>
        /// <param name="keyset"></param>
        /// <returns></returns>
        private INPUT32 CreateInput32(uint keyStroke, KeySet keyset) {
            var input = new INPUT32();
            input.Type = InputType.Keyboard;
            input.Keyboard.Flags = keyStroke | keyset.Flag;
            input.Keyboard.VirtualKey = keyset.VirtualKey;
            // input.Keyboard.ScanCode =(ushort)(NativeMethods.MapVirtualKey3((uint)keyset.VirtualKey, NativeMethods.MAPVK_VK_TO_VSC, _keyboardLayout) & 0xFFU); ;
            input.Keyboard.ScanCode = 0;
            input.Keyboard.Time = 0;
            input.Keyboard.ExtraInfo = NativeMethods.GetMessageExtraInfo();
            return input;
        }

        /// <summary>
        /// send key for 64bit
        /// </summary>
        /// <param name="keyStroke"></param>
        /// <param name="keyset"></param>
        /// <returns></returns>
        private bool SendVKeyNative64(uint keyStroke, KeySet keyset) {
            var input = new INPUT64();
            input.Type = InputType.Keyboard;
            input.Flags = keyStroke | keyset.Flag;
            input.VirtualKey = keyset.VirtualKey;
            // input.ScanCode = (ushort)(NativeMethods.MapVirtualKey3((uint)keyset.VirtualKey, NativeMethods.MAPVK_VK_TO_VSC, _keyboardLayout) & 0xFFU);
            input.ScanCode = 0;
            input.Time = 0;
            input.ExtraInfo = NativeMethods.GetMessageExtraInfo();
            INPUT64[] inputs = { input };
            if (NativeMethods.SendInput64(1, inputs, Marshal.SizeOf(typeof(INPUT64))) != 1) {
                return false;
            }
            DoEvents();
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
        #endregion

    }
}

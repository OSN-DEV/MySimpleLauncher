using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static MyKeyChangerForDebug.AppSettingData;

namespace MyKeyChangerForDebug {
    public partial class TaskTrayMenu : Component {

        #region Declaration
        public class ObserveStateChangedEventArgs : EventArgs {
            public bool Observerd { set;  get; }
            public ObserveStateChangedEventArgs(bool observed) {
                this.Observerd = observed;
            }
        }
        public delegate void ObserveStateChangedHandler(object sender, ObserveStateChangedEventArgs e);
        public event ObserveStateChangedHandler OnObserveStateChanged;

        public class MappingModeChangedEventArgs : EventArgs {
            public MappingMode Mode { set; get; }
            public MappingModeChangedEventArgs(MappingMode mode) {
                this.Mode = mode;
            }
        }
        public delegate void MappingModeChangedHandler(object sender, MappingModeChangedEventArgs e);
        public event MappingModeChangedHandler OnMappingModeChanged;

        public event EventHandler OnExitClicked;

        ToolStripMenuItem _itemObserve;
        ToolStripMenuItem _itemModeAndroid;
        ToolStripMenuItem _itemModeVisualStudio;
        #endregion

        #region Constructor
        public TaskTrayMenu() {
            InitializeComponent();
            this.Initialize();
        }

        public TaskTrayMenu(IContainer container) {
            container.Add(this);
            InitializeComponent();
            this.Initialize();
        }
        #endregion

        #region Public Method
        /// <summary>
        /// set observe menu checked
        /// </summary>
        /// <param name=""></param>
        public void SetObserveChecked(bool isChecked) {
            _itemObserve.Checked = isChecked;
            this.SetNotifyIcon();
        }

        /// <summary>
        /// set 
        /// </summary>
        /// <param name="mode"></param>
        public void SetMode(MappingMode mode) {
            bool androidChecked = (MappingMode.Android == mode);
            _itemModeAndroid.Checked = androidChecked;
            _itemModeVisualStudio.Checked = !androidChecked;
            this.SetNotifyIcon();
        }
        #endregion

        #region Event
        private void OnItemObserveCheckedClicked(object sender, EventArgs e) {
            _itemObserve.Checked = !_itemObserve.Checked;
            this.SetNotifyIcon();
            OnObserveStateChanged?.Invoke(this, new ObserveStateChangedEventArgs(_itemObserve.Checked));
        }

        private void OnItemModeAndroidClicked(object sender, EventArgs e) {
            if (_itemModeAndroid.Checked) {
                return;
            }
            SetMode(MappingMode.Android);
            OnMappingModeChanged?.Invoke(this, new MappingModeChangedEventArgs(MappingMode.Android));
        }

        private void OnItemModeVisualStudioClicked(object sender, EventArgs e) {
            if (_itemModeVisualStudio.Checked) {
                return;
            }
            SetMode(MappingMode.VisualStudio);
            OnMappingModeChanged?.Invoke(this, new MappingModeChangedEventArgs(MappingMode.VisualStudio));
        }

        private void OnItemExitClicked(object sender, EventArgs e) {
            OnExitClicked?.Invoke(this, new EventArgs());
        }
        #endregion

        #region Private Method
        /// <summary>
        /// initialize this component
        /// </summary>
        private void Initialize() {
            this.cMenu.SuspendLayout();
            // create context menu by code, bacuase I can't find the way to create nested menu. 
            this.cMenu.Items.Clear();

            _itemObserve = new ToolStripMenuItem();
            _itemObserve.Text = "observe";
            _itemObserve.ToolTipText = "observe to input ten key";
            _itemObserve.Click += OnItemObserveCheckedClicked;
            this.cMenu.Items.Add(_itemObserve);

            ToolStripMenuItem itemMappingMode = new ToolStripMenuItem();
            itemMappingMode.Text = "mode";
            this.cMenu.Items.Add(itemMappingMode);

            _itemModeAndroid = new ToolStripMenuItem();
            _itemModeAndroid.Text = "Android";
            _itemModeAndroid.ToolTipText = "Key Assign is for Android";
            _itemModeAndroid.Click += OnItemModeAndroidClicked;
            itemMappingMode.DropDown.Items.Add(_itemModeAndroid);

            _itemModeVisualStudio = new ToolStripMenuItem();
            _itemModeVisualStudio.Text = ".Net";
            _itemModeVisualStudio.ToolTipText = "Key Assign is for .Net";
            _itemModeVisualStudio.Click += OnItemModeVisualStudioClicked;
            itemMappingMode.DropDown.Items.Add(_itemModeVisualStudio);

            ToolStripSeparator separator = new ToolStripSeparator();
            this.cMenu.Items.Add(separator);

            ToolStripMenuItem itemExit = new ToolStripMenuItem();
            itemExit.Text = "Exit";
            itemExit.ToolTipText = "Exit Application";
            itemExit.Click += OnItemExitClicked;
            this.cMenu.Items.Add(itemExit);
            this.cMenu.ResumeLayout();
        }

        /// <summary>
        /// set notify icon acconrding to current setting
        /// </summary>
        private void SetNotifyIcon() {
            if (this._itemObserve.Checked) {
                this.cNotify.Icon = this._itemModeAndroid.Checked ? 
                    Properties.Resources.android : Properties.Resources.visaulstudio;
            } else {
                this.cNotify.Icon = this._itemModeAndroid.Checked ? 
                    Properties.Resources.android_disabled : Properties.Resources.visualstudio_disabled;
            }
        }
        #endregion
    }
}

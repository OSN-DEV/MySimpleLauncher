using MyLib.Data;
using MySimpleLauncher.Util;

namespace MySimpleLauncher.Data {
    public class AppSettings : AppDataBase<AppSettings> {
        #region Declaration   
        public long CurrentProfileId { set; get; } = -1;
        public int CategoryListSelectedIndex { set; get; } = -1;
        public bool ShowStatusBar { set; get; }

        public double WindowPosX { set; get; } = -1;
        public double WindowPosY { set; get; } = -1;
        public double WindowSizeW { set; get; } = -1;
        public double WindowSizeH { set; get; } = -1;
        public double CategoryListW { set; get; } = -1;

        private readonly string _settingFile = AppCommon.GetAppPath() + @"\app.settings";
        #endregion

        #region Public Method
        /// <summary>
        /// load settings
        /// </summary>
        public void Load() {
            var instance = GetInstance().LoadFromXml(this._settingFile);
            if (null != instance) {
                this.CurrentProfileId = instance.CurrentProfileId;
                this.CategoryListSelectedIndex = instance.CategoryListSelectedIndex;
                this.ShowStatusBar = instance.ShowStatusBar;
                this.WindowPosX = instance.WindowPosX;
                this.WindowPosY = instance.WindowPosY;
                this.WindowSizeW = instance.WindowSizeW;
                this.WindowSizeH = instance.WindowSizeH;
                this.CategoryListW = instance.CategoryListW;
            }
        }

        /// <summary>
        /// save settings
        /// </summary>
        public void Save() {
            GetInstance().SaveToXml(this._settingFile, this);
        }
        #endregion
    }
}

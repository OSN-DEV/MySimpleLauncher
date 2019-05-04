using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyLib.Data;
using MySimpleLauncher.Util;

namespace MySimpleLauncher.Data {
    public class AppSettings : AppDataBase<AppSettings> {
        #region Declaration   
        public long CurrentProfileId { set; get; } = -1;
        public int CategoryListSelectedIndex { set; get; } = -1;

        private string _settingFile = AppCommon.GetAppPath() + @"\app.settings";
        #endregion

        #region Constructor
        public AppSettings() {
        }
        #endregion

        #region Public Method
        public void Load() {
            var instance = GetInstance().LoadFromXml(this._settingFile);
            if (null != instance) {
                this.CurrentProfileId = instance.CurrentProfileId;
                this.CategoryListSelectedIndex = instance.CategoryListSelectedIndex;
            }
        }
        public void Save() {
            GetInstance().SaveToXml(this._settingFile, this);
        }
        #endregion

    }
}

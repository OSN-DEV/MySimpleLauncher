using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySimpleLauncher.Util;
using MyLib.Data.Sqlite;

namespace MySimpleLauncher.Data {
    internal class SystemDatabase : Database {
        #region Declaration
        private static string _databaseFile = AppCommon.GetAppPath() + "appdata.db";
        #endregion

        #region Constructor
        public SystemDatabase() : base(_databaseFile) {
        }
        #endregion

        #region Public Method
        /// <summary>
        /// create system database file if not exist
        /// </summary>
        /// <returns>true:if success, false:otherwise</returns>
        public bool CreateDatabaseIfNeed() {
            if (System.IO.File.Exists(_databaseFile)) {
                return true;
            }

            base.Open();
            base.BeginTrans();
            int result = base.ExecuteNonQuery(ProfilesTable.CreateTable());
            base.CommitTrans();

            return (-1 < result);
        }
        #endregion
    }
}

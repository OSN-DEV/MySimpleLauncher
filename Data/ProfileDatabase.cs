﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySimpleLauncher.Util;
using MyLib.Data.Sqlite;
using MyLib.File;

namespace MySimpleLauncher.Data {
    internal class ProfileDatabase : Database {
        #region Declaration
        public static string Password = "";

        private enum DbVersion {
            Ver00 = 0,
            Ver01 = 1,
            Current = Ver00

        };
        private delegate List<SqlBuilder> CreateSqls();
        #endregion

        #region Constructor
        public ProfileDatabase(string database) : base(database, (int)DbVersion.Current) {
        }

        public override void Open() {
            base.Open(Password);
        }
        #endregion

        #region Protected Method
        protected override void UpgradeDatabase(int currentVersion, int newVersion, Database database) {
            CreateSqls func = null;
            if (currentVersion < (int)DbVersion.Current) {
                if (newVersion == (int)DbVersion.Ver01) {
                    func = CreateSqlsFrom1To2;
                }
            }

            var sqls = func.Invoke();
            foreach(var sql in sqls) {
                database.ExecuteNonQuery(sql);
            }
        }
        #endregion

        #region Private Method
        private List<SqlBuilder> CreateSqlsFrom1To2() {
            var sqls = new List<SqlBuilder>();
            var sql = new SqlBuilder();
            //sql.AppendSql("ALTER TABLE items ADD COLUMN sex TEXT");
            //sql.AppendSql("ALTER TABLE items ADD COLUMN nick_name TEXT");
            sql.AppendSql("ALTER TABLE items ADD COLUMN tel TEXT");
            sqls.Add(sql);
            return sqls;
        }
        #endregion
    }
}

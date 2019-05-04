using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyLib.Data.Sqlite;
using MySimpleLauncher.Model;

namespace MySimpleLauncher.Data {
    internal class ProfilesTable : TableBase {

        #region Declaration
        public int Id { set; get; }
        public string FilePath { set; get; }
        public string DisplayName { set; get; }
        public int RowOrder { set; get; }
        #endregion

        #region Public Method
        public override void OpenDatabase() {
            if (null == base.Database) {
                base.Database = new SystemDatabase();
            }
            base.Database.Open();
        }

        public override void InitializeMember() {
            this.Id = -1;
            this.FilePath = "";
            this.DisplayName = "";
            this.RowOrder = 0;
        }
        #endregion


        #region internal Method

        /// <summary>
        /// return create table sql
        /// </summary>
        /// <returns>sql</returns>
        internal static SqlBuilder CreateTable() {
            var sql = new SqlBuilder();
            sql.AppendSql("CREATE TABLE profiles (")
               .AppendSql(" id           INTEGER PRIMARY KEY AUTOINCREMENT")
               .AppendSql(",file_path    TEXT UNIQUE")
               .AppendSql(",display_name TEXT    NOT NULL")
               .AppendSql(",row_order    INTEGER DEFAULT 0")
               .AppendSql(")");
            return sql;
        }
        
        /// <summary>
        /// get record count by filePath
        /// </summary>
        /// <param name="filePath">file path</param>
        /// <returns>count of record</returns>
        internal int SelectCountByFilePath(string filePath) {
            int count = 0;
            var sql = new SqlBuilder();
            sql.AppendSql(" SELECT COUNT(id) FROM profiles")
                .AppendSql("WHERE file_path = @file_path");
            var paramList = new ParameterList();
            paramList.Add("@file_path", filePath);
            using (var database = new SystemDatabase()) {
                database.Open();
                using (var record = database.OpenRecordset(sql, paramList)) {
                    if (record.Read()) {
                        count = record.GetInt(0);
                    }
                }
            }
            return count;
        }

        /// <summary>
        /// select record by id
        /// </summary>
        /// <param name="id">profile id</param>
        internal void SelectById(long id) {
            var sql = new SqlBuilder();
            sql.AppendSql(" SELECT * FROM profiles")
                .AppendSql("WHERE id = @id");
            var paramList = new ParameterList();
            paramList.Add("@id", id);
            this.OpenDatabase();
            base._record = base.Database.OpenRecordset(sql, paramList);
        }

        /// <summary>
        /// select id by file path
        /// </summary>
        /// <param name="filePath">file path</param>
        /// <returns>profile id</returns>
        internal int SelectIdByFilePath(string filePath) {
            var sql = new SqlBuilder();
            sql.AppendSql(" SELECT id FROM profiles")
                .AppendSql("WHERE file_path = @file_path");
            var paramList = new ParameterList();
            paramList.Add("@file_path", filePath);
            int id = -1;
            using (var database = new SystemDatabase()) {
                database.Open();
                using (var record = database.OpenRecordset(sql, paramList)) {
                    if (record.Read()) {
                        id = record.GetInt("id");
                    }
                }
            }
            return id;
        }

        /// <summary>
        /// select all record
        /// </summary>
        internal void SelectAll() {
            var sql = new SqlBuilder();
            sql.AppendSql("SELECT ")
                .AppendSql(" id")
                .AppendSql(",file_path")
                .AppendSql(",display_name")
                .AppendSql(",row_order")
                .AppendSql("FROM profiles")
                .AppendSql("ORDER BY row_order")
                .AppendSql("        ,id");
            this.OpenDatabase();
            base._record = base.Database.OpenRecordset(sql);
        }

        /// <summary>
        /// insert profile data
        /// </summary>
        /// <param name="model">insert data</param>
        /// <returns>row id</returns>
        internal long Insert(ProfileModel model) {
            var sql = new SqlBuilder();
            sql.AppendSql("INSERT INTO profiles")
                .AppendSql("(")
                .AppendSql(" file_path")
                .AppendSql(",display_name")
                .AppendSql(",row_order")
                .AppendSql(")")
                .AppendSql("VALUES")
                .AppendSql("(")
                .AppendSql(" @file_path")
                .AppendSql(",@display_name")
                .AppendSql(",@row_order")
                .AppendSql(")");
            var paramList = new ParameterList();
            paramList.Add("@file_path", model.FilePath);
            paramList.Add("@display_name", model.DisplayName);
            paramList.Add("@row_order", model.RowOrder);

            var id = -1L;
            using (var database = new SystemDatabase()) {
                database.Open();
                id = database.Insert(sql, paramList);
            }
            return id;
        }

        /// <summary>
        /// update display name by id
        /// </summary>
        /// <param name="model">profile model</param>
        /// <returns>affected record count</returns>
        internal int UpdateDisplayNameById(ProfileModel model) {
            var sql = new SqlBuilder();
            sql.AppendSql("UPDATE profiles SET")
                .AppendSql(" display_name = @display_name")
                .AppendSql("WHERE id = @id");
            var paramList = new ParameterList();
            paramList.Add("@display_name", model.DisplayName)
                .Add("@id", model.Id);
            var count = 0;
            using (var database = new SystemDatabase()) {
                database.Open();
                count = database.ExecuteNonQuery(sql, paramList);
            }
            return count;
        }

        /// <summary>
        /// delete by id
        /// </summary>
        /// <param name="id">id</param>
        /// <returns>delete record count</returns>
        internal int DeleteById(long id) {
            var sql = new SqlBuilder();
            sql.AppendSql("DELETE FROM profiles")
                .AppendSql("WHERE id = @id");
            var paramList = new ParameterList();
            paramList.Add("@id", id);
            var count = 0;
            using (var database = new SystemDatabase()) {
                database.Open();
                count = database.ExecuteNonQuery(sql, paramList);
            }
            return count;
        }
        #endregion

        #region Protected Method
        protected override void ReadData() {
            this.Id = base.GetInt("id");
            this.FilePath = base.GetString("file_path");
            this.DisplayName = base.GetString("display_name");
            this.RowOrder = base.GetInt("row_order");
        }
        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyLib.Data.Sqlite;
using MySimpleLauncher.Model;

namespace MySimpleLauncher.Data {
    internal class CategoriesTable : TableBase {

        #region Declaration
        public long Id { set; get; }
        public string DisplayName { set; get; }
        public int RowOrder { set; get; }
        #endregion

        #region Constructor
        public CategoriesTable(Database database) {
            base.Database = database;
        }
        #endregion

        #region Public Method
        public override void OpenDatabase() {
            base.Database.Open();
        }

        public override void InitializeMember() {
            this.Id = -1;
            this.DisplayName = "";
            this.RowOrder = 0;
        }
        #endregion

        #region Internal Method
        /// <summary>
        /// return create table sql
        /// </summary>
        /// <returns>sql</returns>
        internal static SqlBuilder CreateTable() {
            var sql = new SqlBuilder();
            sql.AppendSql("CREATE TABLE categories (")
               .AppendSql(" id           INTEGER PRIMARY KEY AUTOINCREMENT")
               .AppendSql(",display_name TEXT    NOT NULL")
               .AppendSql(",row_order    INTEGER DEFAULT 0")
               .AppendSql(")");
            return sql;
        }

        /// <summary>
        /// insert category data
        /// </summary>
        /// <param name="model">insert data</param>
        /// <returns>row id</returns>
        internal long Insert(CategoryModel model) {
            var sql = new SqlBuilder();
            sql.AppendSql("INSERT INTO categories")
                .AppendSql("(")
                .AppendSql(" display_name")
                .AppendSql(",row_order")
                .AppendSql(")")
                .AppendSql("VALUES")
                .AppendSql("(")
                .AppendSql(" @display_name")
                .AppendSql(",@row_order")
                .AppendSql(")");
            var paramList = new ParameterList();
            paramList.Add("@display_name", model.DisplayName);
            paramList.Add("@row_order", model.RowOrder);

            var id = -1L;

            this.OpenDatabase();
            id = base.Database.Insert(sql, paramList);

            return id;
        }

        /// <summary>
        /// update category data by id
        /// </summary>
        /// <param name="model">insert data</param>
        /// <returns>affected row count</returns>
        internal long UpdateById(CategoryModel model) {
            var sql = new SqlBuilder();
            sql.AppendSql("UPDATE categories SET")
                .AppendSql(" display_name =@display_name")
                .AppendSql("WHERE id = @id");
            var paramList = new ParameterList();
            paramList.Add("@display_name", model.DisplayName);
            paramList.Add("@id", model.Id);

            this.OpenDatabase();
            return base.Database.ExecuteNonQuery(sql, paramList);
        }

        /// <summary>
        /// delete profile data by id
        /// </summary>
        /// <param name="model">insert data</param>
        /// <returns>affected row count</returns>
        internal long DeleteById(CategoryModel model) {
            var sql = new SqlBuilder();
            sql.AppendSql("DELETE FROM categories")
                .AppendSql("WHERE id = @id");
            var paramList = new ParameterList();
            paramList.Add("@id", model.Id);
            return base.Database.ExecuteNonQuery(sql, paramList);
        }

        /// <summary>
        /// select all record
        /// </summary>
        internal void SelectAll() {
            var sql = new SqlBuilder();
            sql.AppendSql("SELECT ")
                .AppendSql(" id")
                .AppendSql(",display_name")
                .AppendSql(",row_order")
                .AppendSql("FROM categories")
                .AppendSql("ORDER BY row_order")
                .AppendSql("        ,id");
            this.OpenDatabase();
            base._record = base.Database.OpenRecordset(sql);
        }
        #endregion

        #region Protected Method
        protected override void ReadData() {
            this.Id = base.GetInt("id");
            this.DisplayName = base.GetString("display_name");
            this.RowOrder = base.GetInt("row_order");
        }
        #endregion
    }
}

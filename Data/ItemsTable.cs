using MyLib.Data.Sqlite;
using MySimpleLauncher.Component;
using MySimpleLauncher.Model;
using System.Windows.Media.Imaging;

namespace MySimpleLauncher.Data {
    internal class ItemsTable : TableBase {

        #region Declaration
        internal long Id { set; get; }
        internal long CategoryId { set; get; }
        internal string DisplayName { set; get; }
        internal BitmapImage Icon { set; get; }
        internal string FilePath { set; get; }
        internal string User { set; get; }
        internal string Password { set; get; }
        internal string Comment { set; get; }
        internal string UserId { set; get; }
        internal string FirstName { set; get; }
        internal string LastName { set; get; }
        internal string NickName { set; get; }
        internal string Sex { set; get; }
        internal string Mail { set; get; }
        internal string Birthday { set; get; }
        internal string ZipCode { set; get; }
        internal string Prefecture { set; get; }
        internal string Address1 { set; get; }
        internal string Address2 { set; get; }
        internal string Tel { set; get; }
        internal string SecretQuestion1 { set; get; }
        internal string SecretAnswer1 { set; get; }
        internal string SecretQuestion2 { set; get; }
        internal string SecretAnswer2 { set; get; }
        internal string SecretQuestion3 { set; get; }
        internal string SecretAnswer3 { set; get; }
        internal string UserKey1 { set; get; }
        internal string UserValue1 { set; get; }
        internal string UserKey2 { set; get; }
        internal string UserValue2 { set; get; }
        internal string UserKey3 { set; get; }
        internal string UserValue3 { set; get; }
        internal string UserKey4 { set; get; }
        internal string UserValue4 { set; get; }
        internal string UserKey5 { set; get; }
        internal string UserValue5 { set; get; }
        internal string UserKey6 { set; get; }
        internal string UserValue6 { set; get; }
        internal string UserKey7 { set; get; }
        internal string UserValue7 { set; get; }
        internal string UserKey8 { set; get; }
        internal string UserValue8 { set; get; }
        internal string UserKey9 { set; get; }
        internal string UserValue9 { set; get; }
        internal string UserKey10 { set; get; }
        internal string UserValue10 { set; get; }
        internal int RowOrder { set; get; }
        #endregion

        #region Constructor
        public ItemsTable(Database database) {
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
            sql.AppendSql("CREATE TABLE items (")
                .AppendSql(" id               INTEGER PRIMARY KEY AUTOINCREMENT")
                .AppendSql(",category_id      INTEGER REFERENCES categories (id)")
                .AppendSql(",display_name     TEXT    NOT NULL")
                .AppendSql(",icon             BLOB")
                .AppendSql(",file_path        TEXT")
                .AppendSql(",user             TEXT")
                .AppendSql(",password         TEXT")
                .AppendSql(",comment          TEXT")
                .AppendSql(",user_id          TEXT")
                .AppendSql(",first_name       TEXT")
                .AppendSql(",last_name        TEXT")
                .AppendSql(",nick_name        TEXT")
                .AppendSql(",sex              TEXT")
                .AppendSql(",mail             TEXT")
                .AppendSql(",birthday         TEXT")
                .AppendSql(",zip_code         TEXT")
                .AppendSql(",prefecture       TEXT")
                .AppendSql(",address1         TEXT")
                .AppendSql(",address2         TEXT")
                .AppendSql(",tel              TEXT")
                .AppendSql(",secret_question1 TEXT")
                .AppendSql(",secret_answer1   TEXT")
                .AppendSql(",secret_question2 TEXT")
                .AppendSql(",secret_answer2   TEXT")
                .AppendSql(",secret_question3 TEXT")
                .AppendSql(",secret_answer3   TEXT")
                .AppendSql(",user_key1        TEXT")
                .AppendSql(",user_value1      TEXT")
                .AppendSql(",user_key2        TEXT")
                .AppendSql(",user_value2      TEXT")
                .AppendSql(",user_key3        TEXT")
                .AppendSql(",user_value3      TEXT")
                .AppendSql(",user_key4        TEXT")
                .AppendSql(",user_value4      TEXT")
                .AppendSql(",user_key5        TEXT")
                .AppendSql(",user_value5      TEXT")
                .AppendSql(",user_key6        TEXT")
                .AppendSql(",user_value6      TEXT")
                .AppendSql(",user_key7        TEXT")
                .AppendSql(",user_value7      TEXT")
                .AppendSql(",user_key8        TEXT")
                .AppendSql(",user_value8      TEXT")
                .AppendSql(",user_key9        TEXT")
                .AppendSql(",user_value9      TEXT")
                .AppendSql(",user_key10       TEXT")
                .AppendSql(",user_value10     TEXT")
                .AppendSql(",row_order        INTEGER")
                .AppendSql(",search_keyword   TEXT")
                .AppendSql(")");
            return sql;
        }

        /// <summary>
        /// insert items data
        /// </summary>
        /// <param name="model">insert data</param>
        /// <returns>row id</returns>
        internal long Insert(ItemModel model) {
            var sql = new SqlBuilder();
            sql.AppendSql("INSERT INTO items")
                .AppendSql("(")
                .AppendSql(" category_id")
                .AppendSql(",display_name")
                .AppendSql(",icon")
                .AppendSql(",file_path")
                .AppendSql(",user")
                .AppendSql(",password")
                .AppendSql(",comment")
                .AppendSql(",user_id")
                .AppendSql(",first_name")
                .AppendSql(",last_name")
                .AppendSql(",nick_name")
                .AppendSql(",sex")
                .AppendSql(",mail")
                .AppendSql(",birthday")
                .AppendSql(",zip_code")
                .AppendSql(",prefecture")
                .AppendSql(",address1")
                .AppendSql(",address2")
                .AppendSql(",tel")
                .AppendSql(",secret_question1")
                .AppendSql(",secret_answer1")
                .AppendSql(",secret_question2")
                .AppendSql(",secret_answer2")
                .AppendSql(",secret_question3")
                .AppendSql(",secret_answer3")
                .AppendSql(",user_key1")
                .AppendSql(",user_value1")
                .AppendSql(",user_key2")
                .AppendSql(",user_value2")
                .AppendSql(",user_key3")
                .AppendSql(",user_value3")
                .AppendSql(",user_key4")
                .AppendSql(",user_value4")
                .AppendSql(",user_key5")
                .AppendSql(",user_value5")
                .AppendSql(",user_key6")
                .AppendSql(",user_value6")
                .AppendSql(",user_key7")
                .AppendSql(",user_value7")
                .AppendSql(",user_key8")
                .AppendSql(",user_value8")
                .AppendSql(",user_key9")
                .AppendSql(",user_value9")
                .AppendSql(",user_key10")
                .AppendSql(",user_value10")
                .AppendSql(",row_order")
                .AppendSql(",search_keyword")
                .AppendSql(")")
                .AppendSql("VALUES")
                .AppendSql("(")
                .AppendSql(" @category_id")
                .AppendSql(",@display_name")
                .AppendSql(",@icon")
                .AppendSql(",@file_path")
                .AppendSql(",@user")
                .AppendSql(",@password")
                .AppendSql(",@comment")
                .AppendSql(",@user_id")
                .AppendSql(",@first_name")
                .AppendSql(",@last_name")
                .AppendSql(",@nick_name")
                .AppendSql(",@sex")
                .AppendSql(",@mail")
                .AppendSql(",@birthday")
                .AppendSql(",@zip_code")
                .AppendSql(",@prefecture")
                .AppendSql(",@address1")
                .AppendSql(",@address2")
                .AppendSql(",@tel")
                .AppendSql(",@secret_question1")
                .AppendSql(",@secret_answer1")
                .AppendSql(",@secret_question2")
                .AppendSql(",@secret_answer2")
                .AppendSql(",@secret_question3")
                .AppendSql(",@secret_answer3")
                .AppendSql(",@user_key1")
                .AppendSql(",@user_value1")
                .AppendSql(",@user_key2")
                .AppendSql(",@user_value2")
                .AppendSql(",@user_key3")
                .AppendSql(",@user_value3")
                .AppendSql(",@user_key4")
                .AppendSql(",@user_value4")
                .AppendSql(",@user_key5")
                .AppendSql(",@user_value5")
                .AppendSql(",@user_key6")
                .AppendSql(",@user_value6")
                .AppendSql(",@user_key7")
                .AppendSql(",@user_value7")
                .AppendSql(",@user_key8")
                .AppendSql(",@user_value8")
                .AppendSql(",@user_key9")
                .AppendSql(",@user_value9")
                .AppendSql(",@user_key10")
                .AppendSql(",@user_value10")
                .AppendSql(",@row_order")
                .AppendSql(",@search_keyword")
                .AppendSql(")");
            var paramList = new ParameterList();
            paramList.Add("@category_id", model.CategoryId);
            paramList.Add("@display_name", model.DisplayName);
            paramList.Add("@icon", model.Icon.ConvertToBytes());
            paramList.Add("@file_path", model.FilePath);
            paramList.Add("@user", model.User);
            paramList.Add("@password", model.Password);
            paramList.Add("@comment", model.Comment);
            paramList.Add("@user_id", model.UserId);
            paramList.Add("@first_name", model.FirstName);
            paramList.Add("@last_name", model.LastName);
            paramList.Add("@nick_name", model.NickName);
            paramList.Add("@sex", model.Sex);
            paramList.Add("@mail", model.Mail);
            paramList.Add("@birthday", model.Birthday);
            paramList.Add("@zip_code", model.ZipCode);
            paramList.Add("@prefecture", model.Prefecture);
            paramList.Add("@address1", model.Address1);
            paramList.Add("@address2", model.Address2);
            paramList.Add("@tel", model.Tel);
            paramList.Add("@secret_question1", model.SecretQuestion1);
            paramList.Add("@secret_answer1", model.SecretAnswer1);
            paramList.Add("@secret_question2", model.SecretQuestion2);
            paramList.Add("@secret_answer2", model.SecretAnswer2);
            paramList.Add("@secret_question3", model.SecretQuestion3);
            paramList.Add("@secret_answer3", model.SecretAnswer3);
            paramList.Add("@user_key1", model.UserKey1);
            paramList.Add("@user_value1", model.UserValue1);
            paramList.Add("@user_key2", model.UserKey2);
            paramList.Add("@user_value2", model.UserValue2);
            paramList.Add("@user_key3", model.UserKey3);
            paramList.Add("@user_value3", model.UserValue3);
            paramList.Add("@user_key4", model.UserKey4);
            paramList.Add("@user_value4", model.UserValue4);
            paramList.Add("@user_key5", model.UserKey5);
            paramList.Add("@user_value5", model.UserValue5);
            paramList.Add("@user_key6", model.UserKey6);
            paramList.Add("@user_value6", model.UserValue6);
            paramList.Add("@user_key7", model.UserKey7);
            paramList.Add("@user_value7", model.UserValue7);
            paramList.Add("@user_key8", model.UserKey8);
            paramList.Add("@user_value8", model.UserValue8);
            paramList.Add("@user_key9", model.UserKey9);
            paramList.Add("@user_value9", model.UserValue9);
            paramList.Add("@user_key10", model.UserKey10);
            paramList.Add("@user_value10", model.UserValue10);
            paramList.Add("@row_order", model.RowOrder);
            paramList.Add("@search_keyword", model.GetSearchKeyword());

            this.OpenDatabase();
            return base.Database.Insert(sql, paramList);
        }

        /// <summary>
        /// update items data by id
        /// </summary>
        /// <param name="model">insert data</param>
        /// <returns>affected row count</returns>
        internal int UpdateById(ItemModel model) {
            var sql = new SqlBuilder();
            sql.AppendSql("UPDATE items SET")
                .AppendSql(" category_id = @category_id")
                .AppendSql(",display_name = @display_name")
                .AppendSql(",icon = @icon")
                .AppendSql(",file_path = @file_path")
                .AppendSql(",user = @user")
                .AppendSql(",password = @password")
                .AppendSql(",comment = @comment")
                .AppendSql(",user_id = @user_id")
                .AppendSql(",first_name = @first_name")
                .AppendSql(",last_name = @last_name")
                .AppendSql(",nick_name = @nick_name")
                .AppendSql(",sex = @sex")
                .AppendSql(",mail = @mail")
                .AppendSql(",birthday = @birthday")
                .AppendSql(",zip_code = @zip_code")
                .AppendSql(",prefecture = @prefecture")
                .AppendSql(",address1 = @address1")
                .AppendSql(",address2 = @address2")
                .AppendSql(",tel = @tel")
                .AppendSql(",secret_question1 = @secret_question1")
                .AppendSql(",secret_answer1 = @secret_answer1")
                .AppendSql(",secret_question2 = @secret_question2")
                .AppendSql(",secret_answer2 = @secret_answer2")
                .AppendSql(",secret_question3 = @secret_question3")
                .AppendSql(",secret_answer3 = @secret_answer3")
                .AppendSql(",user_key1 = @user_key1")
                .AppendSql(",user_value1 = @user_value1")
                .AppendSql(",user_key2 = @user_key2")
                .AppendSql(",user_value2 = @user_value2")
                .AppendSql(",user_key3 = @user_key3")
                .AppendSql(",user_value3 = @user_value3")
                .AppendSql(",user_key4 = @user_key4")
                .AppendSql(",user_value4 = @user_value4")
                .AppendSql(",user_key5 = @user_key5")
                .AppendSql(",user_value5 = @user_value5")
                .AppendSql(",user_key6 = @user_key6")
                .AppendSql(",user_value6 = @user_value6")
                .AppendSql(",user_key7 = @user_key7")
                .AppendSql(",user_value7 = @user_value7")
                .AppendSql(",user_key8 = @user_key8")
                .AppendSql(",user_value8 = @user_value8")
                .AppendSql(",user_key9 = @user_key9")
                .AppendSql(",user_value9 = @user_value9")
                .AppendSql(",user_key10 = @user_key10")
                .AppendSql(",user_value10 = @user_value10")
                .AppendSql(",row_order = @row_order")
                .AppendSql(",search_keyword = @search_keyword")
                .AppendSql("WHERE id = @id");

            var paramList = new ParameterList();
            paramList.Add("@id", model.Id);
            paramList.Add("@category_id", model.CategoryId);
            paramList.Add("@display_name", model.DisplayName);
            paramList.Add("@icon", model.Icon.ConvertToBytes());
            paramList.Add("@file_path", model.FilePath);
            paramList.Add("@user", model.User);
            paramList.Add("@password", model.Password);
            paramList.Add("@comment", model.Comment);
            paramList.Add("@user_id", model.UserId);
            paramList.Add("@first_name", model.FirstName);
            paramList.Add("@last_name", model.LastName);
            paramList.Add("@nick_name", model.NickName);
            paramList.Add("@sex", model.Sex);
            paramList.Add("@mail", model.Mail);
            paramList.Add("@birthday", model.Birthday);
            paramList.Add("@zip_code", model.ZipCode);
            paramList.Add("@prefecture", model.Prefecture);
            paramList.Add("@address1", model.Address1);
            paramList.Add("@address2", model.Address2);
            paramList.Add("@tel", model.Tel);
            paramList.Add("@secret_question1", model.SecretQuestion1);
            paramList.Add("@secret_answer1", model.SecretAnswer1);
            paramList.Add("@secret_question2", model.SecretQuestion2);
            paramList.Add("@secret_answer2", model.SecretAnswer2);
            paramList.Add("@secret_question3", model.SecretQuestion3);
            paramList.Add("@secret_answer3", model.SecretAnswer3);
            paramList.Add("@user_key1", model.UserKey1);
            paramList.Add("@user_value1", model.UserValue1);
            paramList.Add("@user_key2", model.UserKey2);
            paramList.Add("@user_value2", model.UserValue2);
            paramList.Add("@user_key3", model.UserKey3);
            paramList.Add("@user_value3", model.UserValue3);
            paramList.Add("@user_key4", model.UserKey4);
            paramList.Add("@user_value4", model.UserValue4);
            paramList.Add("@user_key5", model.UserKey5);
            paramList.Add("@user_value5", model.UserValue5);
            paramList.Add("@user_key6", model.UserKey6);
            paramList.Add("@user_value6", model.UserValue6);
            paramList.Add("@user_key7", model.UserKey7);
            paramList.Add("@user_value7", model.UserValue7);
            paramList.Add("@user_key8", model.UserKey8);
            paramList.Add("@user_value8", model.UserValue8);
            paramList.Add("@user_key9", model.UserKey9);
            paramList.Add("@user_value9", model.UserValue9);
            paramList.Add("@user_key10", model.UserKey10);
            paramList.Add("@user_value10", model.UserValue10);
            paramList.Add("@row_order", model.RowOrder);
            paramList.Add("@search_keyword", model.GetSearchKeyword());

            this.OpenDatabase();
            return base.Database.ExecuteNonQuery(sql, paramList);
        }

        /// <summary>
        /// select all record
        /// </summary>
        internal void SelectAllById(long id) {
            var sql = new SqlBuilder();
            sql.AppendSql("SELECT ")
                .AppendSql(" *")
                .AppendSql("FROM items")
                .AppendSql("WHERE category_id = @category_id")
                .AppendSql("ORDER BY row_order")
                .AppendSql("        ,id");
            this.OpenDatabase();

            var paramList = new ParameterList();
            paramList.Add("@category_id", id);
            
            this.OpenDatabase();
            base._record = base.Database.OpenRecordset(sql, paramList);
        }

        /// <summary>
        /// delete by id
        /// </summary>
        /// <param name="id">id</param>
        /// <returns>delete record count</returns>
        internal int DeleteById(long id) {
            var sql = new SqlBuilder();
            sql.AppendSql("DELETE FROM items")
                .AppendSql("WHERE id = @id");
            var paramList = new ParameterList();
            paramList.Add("@id", id);
            this.OpenDatabase();
            return base.Database.ExecuteNonQuery(sql, paramList);
        }

        /// <summary>
        /// update category id by category id
        /// </summary>
        /// <param name="oldCateogryId">old category id</param>
        /// <param name="newCateogoryId">new category id</param>
        /// <returns>affected rows</returns>
        internal int UpdateCategoryIdByCategoryId(long oldCateogryId, long newCateogoryId) {
            var sql = new SqlBuilder();
            sql.AppendSql("UPDATE items SET")
                .AppendSql("category_id = @new_category_id")
                .AppendSql("WHERE category_id = @old_category_id");
            var paramList = new ParameterList();
            paramList.Add("@new_category_id", newCateogoryId);
            paramList.Add("@old_category_id", oldCateogryId);
            return base.Database.ExecuteNonQuery(sql, paramList);
        }

        /// <summary>
        /// delete by id
        /// </summary>
        /// <param name="cateogryId">category id</param>
        /// <returns>delete record count</returns>
        internal int DeleteByCateogyId(long cateogryId) {
            var sql = new SqlBuilder();
            sql.AppendSql("DELETE FROM items")
                .AppendSql("WHERE category_id = @category_id");
            var paramList = new ParameterList();
            paramList.Add("@category_id", cateogryId);
            return  base.Database.ExecuteNonQuery(sql, paramList);
        }
        #endregion

        #region Protected Method
        protected override void ReadData() {
            this.Id = base.GetInt("id");
            this.CategoryId = base.GetInt("category_id");
            this.DisplayName = base.GetString("display_name");
            this.Icon =  MySimpleLauncher.Util.AppCommon.GetBitmapImage(base.GetBlob("icon"));
            this.FilePath = base.GetString("file_path");
            this.User = base.GetString("user");
            this.Password = base.GetString("password");
            this.Comment = base.GetString("comment");
            this.UserId = base.GetString("user_id");
            this.FirstName = base.GetString("first_name");
            this.LastName = base.GetString("last_name");
            this.NickName = base.GetString("nick_name");
            this.Sex = base.GetString("sex");
            this.Mail = base.GetString("mail");
            this.Birthday = base.GetString("birthday");
            this.ZipCode = base.GetString("zip_code");
            this.Prefecture = base.GetString("prefecture");
            this.Address1 = base.GetString("address1");
            this.Address2 = base.GetString("address2");
            this.Tel = base.GetString("tel");
            this.SecretQuestion1 = base.GetString("secret_question1");
            this.SecretAnswer1 = base.GetString("secret_answer1");
            this.SecretQuestion2 = base.GetString("secret_question2");
            this.SecretAnswer2 = base.GetString("secret_answer2");
            this.SecretQuestion3 = base.GetString("secret_question3");
            this.SecretAnswer3 = base.GetString("secret_answer3");
            this.UserKey1 = base.GetString("user_key1");
            this.UserValue1 = base.GetString("user_value1");
            this.UserKey2 = base.GetString("user_key2");
            this.UserValue2 = base.GetString("user_value2");
            this.UserKey3 = base.GetString("user_key3");
            this.UserValue3 = base.GetString("user_value3");
            this.UserKey4 = base.GetString("user_key4");
            this.UserValue4 = base.GetString("user_value4");
            this.UserKey5 = base.GetString("user_key5");
            this.UserValue5 = base.GetString("user_value5");
            this.UserKey6 = base.GetString("user_key6");
            this.UserValue6 = base.GetString("user_value6");
            this.UserKey7 = base.GetString("user_key7");
            this.UserValue7 = base.GetString("user_value7");
            this.UserKey8 = base.GetString("user_key8");
            this.UserValue8 = base.GetString("user_value8");
            this.UserKey9 = base.GetString("user_key9");
            this.UserValue9 = base.GetString("user_value9");
            this.UserKey10 = base.GetString("user_key10");
            this.UserValue10 = base.GetString("user_value10");
            this.RowOrder = base.GetInt("row_order");
        }
        #endregion
    }
}

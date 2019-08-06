using MySimpleLauncher.Data;

namespace MySimpleLauncher.Model {
    internal class ProfileModel {
        #region Declaration
        public long Id { set; get; }
        public string FilePath { set; get; }
        public string DisplayName { set; get; }
        public int RowOrder { set; get; }
        public ProfileModel Model { get { return this; } }
        #endregion

        #region Constructor
        internal ProfileModel() { }

        internal ProfileModel(ProfilesTable table) {
            this.Id = table.Id;
            this.FilePath = table.FilePath;
            this.DisplayName = table.DisplayName;
            this.RowOrder = table.RowOrder;
        }
        #endregion
    }
}

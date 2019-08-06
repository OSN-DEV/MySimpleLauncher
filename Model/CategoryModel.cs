using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySimpleLauncher.Data;

namespace MySimpleLauncher.Model {
    internal class CategoryModel {
        #region Declaration
        public long Id { set; get; }
        public string DisplayName { set; get; }
        public int RowOrder { set; get; }
        public CategoryModel Model { get { return this; } }
        #endregion

        #region Constructor
        internal CategoryModel() { }

        internal CategoryModel(CategoriesTable table) {
            this.Id = table.Id;
            this.DisplayName = table.DisplayName;
            this.RowOrder = table.RowOrder;
        }

        #endregion
    }
}

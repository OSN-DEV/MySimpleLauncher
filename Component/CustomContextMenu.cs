using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace MySimpleLauncher.Component {
    internal class CustomContextMenu : ContextMenu {

        #region Declaration
        public class MenuItemData {
            public int Id { set; get; }
            public string Text { set; get; }
            public string ForeGround { set; get; } = "#333333";
            public string MouseOverColor { set; get; } = "#515151";
            public string PressedColor { set; get; } = "#151515";
            //public bool IsSeparator { set; get; }
            public RoutedEventHandler Click { set; get; }
        }
        Dictionary<int, MenuItem> _menuItems = new Dictionary<int, MenuItem>();

        public string MenuFontName { set; get; } = "Meiryo UI";
        public double MenuFontSize { set; get; } = 11;
        public string MenuHighlightColor { set; get; } = "#EEE";
        public Brush SeparatorColor { set; get; } = new SolidColorBrush(Color.FromRgb(0xDD, 0xDD, 0xDD));
        #endregion


        #region Constructor
        public CustomContextMenu() {
            // this.SetButtonStyle();
        }
        #endregion

        #region Public Method
        /// <summary>
        /// Add menu item
        /// </summary>
        /// <param name="data">menu item data</param>
        public void AddItem(MenuItemData data) {
            var menuItem = new MenuItem();
            menuItem.Header = data.Text;
            menuItem.Click += data.Click;
            menuItem.Tag = data;

            this._menuItems.Add(data.Id, menuItem);
            this.Items.Add(menuItem);
        }
        
        /// <summary>
        /// add separator
        /// </summary>
        public void AddSeparator() {
            this.Items.Add(new Separator());
        }
        #endregion


        #region Private Method
        /// <summary>
        /// set menu item enabled
        /// </summary>
        /// <param name="id">target menu item id</param>
        /// <param name="enabled">enabled</param>
        public void SetMenuItemEnabled(int id, bool enabled) {
            this._menuItems[id].IsEnabled = enabled;
        }
        #endregion
    }
}

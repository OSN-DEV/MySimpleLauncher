using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace MySimpleLauncher.Component {
    internal class CustomContextMenu : ContextMenu {

        #region Declaration
        internal class MenuItemData {
            internal int Id { set; get; }
            internal string Text { set; get; }
            internal string ForeGround { set; get; } = "#333333";
            internal RoutedEventHandler Click { set; get; }
            internal string FontName { set; get; } = "Meiryo UI";
            internal double FontSize { set; get; } = 11;
        }
        private Dictionary<int, MenuItem> _menuItems = new Dictionary<int, MenuItem>();
        #endregion

        #region Internal Method
        /// <summary>
        /// Add menu item
        /// </summary>
        /// <param name="data">menu item data</param>
        internal void AddItem(MenuItemData data) {
            var menuItem = new MenuItem();
            menuItem.Header = data.Text;
            menuItem.Click += data.Click;
            menuItem.Tag = data;
            menuItem.FontFamily = new FontFamily(data.FontName);
            menuItem.FontSize = data.FontSize;
            menuItem.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString(data.ForeGround));

            this._menuItems.Add(data.Id, menuItem);
            this.Items.Add(menuItem);
        }

        /// <summary>
        /// add separator
        /// </summary>
        internal void AddSeparator(string color = "#DDDDDD") {
            var separator = new Separator();
            separator.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString(color));
            this.Items.Add(separator);
        }
        #endregion


        #region Private Method
        /// <summary>
        /// set menu item enabled
        /// </summary>
        /// <param name="id">target menu item id</param>
        /// <param name="enabled">enabled</param>
        internal void SetMenuItemEnabled(int id, bool enabled) {
            this._menuItems[id].IsEnabled = enabled;
        }
        #endregion
    }
}

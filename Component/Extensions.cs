using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace MySimpleLauncher.Component {

    internal static class Extensions {

        #region ListView
        /// <summary>
        /// get ListItem from cursor position
        /// </summary>
        /// <param name="listView">this</param>
        /// <param name="clientRelativePosition">point</param>
        /// <returns>ListItem</returns>
        internal static ListViewItem GetItemAt(this ListView listView, Point clientRelativePosition) {
            var targetItem = VisualTreeHelper.HitTest(listView, clientRelativePosition).VisualHit;
            while (null != targetItem) {
                if (targetItem is ListViewItem) {
                    break;
                }
                targetItem = VisualTreeHelper.GetParent(targetItem);
            }
            return targetItem != null ? ((ListViewItem)targetItem) : null;
        }
        #endregion

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MySimpleLauncher.Component;
using MySimpleLauncher.Data;
using MySimpleLauncher.Model;
using MySimpleLauncher.Util;
using System.Collections.ObjectModel;
using MyLib.Data.Sqlite;
using System.IO;

namespace MySimpleLauncher.Component {

    internal static class Extensions {

        #region ListView
        /// <summary>
        /// get ListItem from cursor position
        /// </summary>
        /// <param name="listView">this</param>
        /// <param name="clientRelativePosition">point</param>
        /// <returns>ListItem</returns>
        public static ListViewItem GetItemAt(this ListView listView, Point clientRelativePosition) {
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

        #region BitmapImage
        /// <summary>
        /// convert StreamSource to byte array
        /// </summary>
        /// <param name="bitmapImage">this</param>
        /// <returns>byte array</returns>
        public static byte[] ConvertToBytes(this BitmapImage bitmapImage) {
            if(null == bitmapImage) {
                return null;
            }
            byte[] data = null;
            using (var stream = new MemoryStream()){
                var encoder = new BmpBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(bitmapImage));
                encoder.Save(stream);
                data = stream.ToArray();
            }
            return data;
        }
        public static BitmapImage ConvertToBitmapImage(this BitmapImage image, byte[] data) {
            return null;
        }
        #endregion
    }
}

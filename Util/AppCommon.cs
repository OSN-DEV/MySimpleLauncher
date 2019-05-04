using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using System.IO;

namespace MySimpleLauncher.Util {
    internal class AppCommon {
        /// <summary>
        /// get app binary path
        /// </summary>
        /// <returns>app executable path</returns>
        public static string GetAppPath() {
            string path = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            if (!path.EndsWith(@"\")) {
                path += @"\";
            }
            return path;
        }

        /// <summary>
        /// show error message
        /// </summary>
        /// <param name="message">message</param>

        public static void ShowErrorMsg(string message) {
            MessageBox.Show(message, "error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        /// <summary>
        /// get BitmapImage from file path
        /// </summary>
        /// <param name="filePath">image file path</param>
        /// <returns>BitmapImage</returns>
        public static BitmapImage GetBitmapImage(string filePath) {
            var bitmapImage = new BitmapImage();
            try {
                using (var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read)) {
                    bitmapImage.BeginInit();
                    bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                    bitmapImage.CreateOptions = BitmapCreateOptions.None;
                    bitmapImage.StreamSource = stream;
                    bitmapImage.EndInit();
                    bitmapImage.Freeze();
                }
            } catch  {
                bitmapImage = null;
            }
            return bitmapImage;
        }

        /// <summary>
        /// get BitmapImage from file path
        /// </summary>
        /// <param name="data">image data</param>
        /// <returns>BitmapImage</returns>
        public static BitmapImage GetBitmapImage(byte[] data) {
            var bitmapImage = new BitmapImage();
            try {
                using (var starem = new MemoryStream(data)) {
                    starem.Seek(0, SeekOrigin.Begin);
                    bitmapImage.BeginInit();
                    bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                    bitmapImage.CreateOptions = BitmapCreateOptions.None;
                    bitmapImage.StreamSource = starem;
                    bitmapImage.EndInit();
                    bitmapImage.Freeze();
                }
            } catch {
                bitmapImage = null;
            }
            return bitmapImage;
        }
    }
}

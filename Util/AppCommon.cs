using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using System.IO;
using System.Net;
using System.Drawing.Imaging;

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
    }
}

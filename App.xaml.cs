using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using MySimpleLauncher.Util;
using System.Threading;

namespace MySimpleLauncher
{
    /// <summary>
    /// App.xaml の相互作用ロジック
    /// </summary>
    public partial class App : Application {
        #region Declaration
        private Mutex _mutex = new Mutex(false, "app_mutex");
        private UI.MySimpleLauncherMain _mainWindow = null;
        #endregion

        #region Event
        private void Application_Startup(object sender, StartupEventArgs e) {
            if (!this._mutex.WaitOne(0, false)) {
                IntPtr hMWnd = NativeMethods.FindWindow(null, AppCommon.GetAppName());
                if (hMWnd != null && NativeMethods.IsWindow(hMWnd)) {
                    var hCWnd = NativeMethods.GetLastActivePopup(hMWnd);
                    if (hCWnd != null && NativeMethods.IsWindow(hCWnd) && NativeMethods.IsWindowVisible(hCWnd)) {
                        NativeMethods.ShowWindow(hCWnd, (int)NativeMethods.SW.SHOWNORMAL);
                        NativeMethods.SetForegroundWindow(hCWnd);
                    }
                }

                this._mutex.Close();
                this._mutex = null;
                Shutdown();
            } else {
                this._mainWindow = new UI.MySimpleLauncherMain();
                this._mainWindow.Show();
            }
        }

        private void Application_Exit(object sender, ExitEventArgs e) {
            if (this._mutex != null) {
                this._mutex.ReleaseMutex();
                this._mutex.Close();
                this._mutex = null;
            }
        }
        #endregion
    }
}

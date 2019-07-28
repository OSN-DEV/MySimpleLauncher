using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing.Imaging;
using System.Windows.Media.Imaging;
using System.IO;
using MySimpleLauncher.Util;
using System.Windows.Data;
using System.Windows;

namespace MySimpleLauncher.Component {
    public class CustomImage : System.Windows.Controls.Image {

        #region Declaration
        #endregion

        #region Public Property
        //public static readonly DependencyProperty ByteSourceProp =
        //    DependencyProperty.Register("ByteSource",
        //                                typeof(IList<byte>),
        //                                typeof(CustomImage),
        //                                new FrameworkPropertyMetadata() {
        //                                    // BindsTwoWayByDefault = true
        //                                });

        //public IList<byte> ByteSource {
        //    set {
        //        this.SetValue(ByteSourceProp, value);
        //        if (null != value) {
        //            this.Source = this.GetBitmapSource((byte[])value);
        //        } else {
        //            this.Source = null;
        //        }
        //    }
        //    get { return (IList<byte>)this.GetValue(ByteSourceProp); }
        //}
        private byte[] _byteSrouce = null;
        public byte[] ByteSource {
            set {
                this._byteSrouce = value;
                if (null == value) {
                    this.Source = null;
                } else {
                    this.Source = this.GetBitmapSource(value);
                }
            }
            get { return this._byteSrouce; }
        }
        #endregion

        #region Public Method
        /// <summary>
        /// Clear source
        /// </summary>
        public void ClearSource() {
            this.Source = null;
            this.ByteSource = null;
        }

        /// <summary>
        /// set app icon
        /// </summary>
        /// <param name="filePath">app file path</param>
        public void SetAppIcon(string filePath) {
            using (var icon = System.Drawing.Icon.ExtractAssociatedIcon(filePath))
            using (var stream = new MemoryStream()) {
                var bmp = icon.ToBitmap();
                bmp.Save(stream, ImageFormat.Png);
                this.ByteSource = stream.GetBuffer();
            }
        }

        /// <summary>
        /// set directory icon
        /// </summary>
        /// <param name="filePath">directory path</param>
        public void SetDirectoryIcon(string filePath) {
            var shinfo = new NativeMethods.SHFILEINFO();
            IntPtr hImg = NativeMethods.SHGetFileInfo(
              filePath, 0, out shinfo, (uint)System.Runtime.InteropServices.Marshal.SizeOf(typeof(NativeMethods.SHFILEINFO)),
              (uint)(NativeMethods.SHGFI.SHGFI_ICON | NativeMethods.SHGFI.SHGFI_LARGEICON));
            if (IntPtr.Zero != hImg) {
                using (var icon = System.Drawing.Icon.FromHandle(shinfo.hIcon))
                using (var stream = new MemoryStream()) {
                    var bmp = icon.ToBitmap();
                    bmp.Save(stream, ImageFormat.Png);
                    this.ByteSource = stream.GetBuffer();
                }
            } else {
                this.ClearSource();
            }
        }

        /// <summary>
        /// set image file
        /// </summary>
        /// <param name="filePath">file path</param>
        public void SetImageFile(string filePath) {
            using (var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            using (var memStream = new MemoryStream()) {
                stream.CopyTo(memStream);
                this.ByteSource = memStream.GetBuffer();
            }
        }
        #endregion

        #region  Private Method
        /// <summary>
        /// get BitmapSource from file path
        /// </summary>
        /// <param name="data">image data</param>
        /// <returns>BitmapSource</returns>
        private BitmapSource GetBitmapSource(byte[] data) {
            BitmapSource bitmapSource = null;
            try {
                using (var stream = new MemoryStream(data)) {
                    var bitmapDecoder = BitmapDecoder.Create(
                                        stream,
                                        BitmapCreateOptions.PreservePixelFormat,
                                        BitmapCacheOption.OnLoad);
                    var writable = new WriteableBitmap(bitmapDecoder.Frames.Single());
                    writable.Freeze();
                    bitmapSource = (BitmapSource)writable;
                }
            } catch {
            }
            return bitmapSource;
        }
        #endregion
    }
}

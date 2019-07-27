using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace MySimpleLauncher.Util {
    public class NativeMethods {
        public delegate bool EnumWindowsDelegate(IntPtr hWnd, IntPtr lparam);

        public static class Flags {
            public const int None = 0x00;
            public const int KeyDown = 0x00;
            public const int KeyUp = 0x02;
            public const int ExtendeKey = 0x01;
            public const int Unicode = 0x04;
            public const int ScanCode = 0x08;
        }

        public class KeySet {
            public ushort VirtualKey;
            public ushort ScanCode;
            public uint Flag;
            public KeySet(byte[] pair) : this(pair, Flags.None) {
            }
            public KeySet(byte[] pair, uint flag) {
                this.VirtualKey = KeySetPair.VirtualKey(pair);
                this.ScanCode = KeySetPair.ScanCode(pair);
                this.Flag = flag;
            }
        }

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public extern static bool EnumWindows(EnumWindowsDelegate lpEnumFunc, IntPtr lparam);

        [DllImport("user32")]
        public static extern bool IsWindowVisible(IntPtr hWnd);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int GetWindowTextLength(IntPtr hWnd);

        [DllImport("User32.dll")]
        internal static extern uint GetWindowThreadProcessId(IntPtr hWnd, [Out] out uint lpdwProcessId);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool SetForegroundWindow(IntPtr hWnd);
    }
}

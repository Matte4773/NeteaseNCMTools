using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace DirectUI
{
    class Win32
    {
        public enum Bool
        {
            False = 0,
            True
        };

        [StructLayout(LayoutKind.Sequential)]
        public struct Size
        {
            public Int32 cx;
            public Int32 cy;

            public Size(Int32 cx, Int32 cy)
            { this.cx = cx; this.cy = cy; }
        }
        
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        struct ARGB
        {
            public byte Blue;
            public byte Green;
            public byte Red;
            public byte Alpha;
        }
        
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct BLENDFUNCTION
        {
            public byte BlendOp;
            public byte BlendFlags;
            public byte SourceConstantAlpha;
            public byte AlphaFormat;
        }
        
        public const Int32 ULW_COLORKEY = 0x00000001;
        public const Int32 ULW_ALPHA = 0x00000002;
        public const Int32 ULW_OPAQUE = 0x00000004;

        public const byte AC_SRC_OVER = 0x00;
        public const byte AC_SRC_ALPHA = 0x01;

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern int UpdateLayeredWindow(IntPtr hwnd, IntPtr hdcDst, [In]ref Point pptDst, [In]ref System.Drawing.Size psize, IntPtr hdcSrc, [In]ref System.Drawing.Point pptSrc, int crKey, [In]ref BLENDFUNCTION pblend, int dwFlags);

        [DllImport("user32.dll", ExactSpelling = true, SetLastError = true)]
        public static extern IntPtr GetDC(IntPtr hWnd);

        [DllImport("user32.dll", ExactSpelling = true)]
        public static extern int ReleaseDC(IntPtr hWnd, IntPtr hDC);

        [DllImport("gdi32.dll", ExactSpelling = true, SetLastError = true)]
        public static extern IntPtr CreateCompatibleDC(IntPtr hDC);

        [DllImport("gdi32.dll", ExactSpelling = true, SetLastError = true)]
        public static extern Bool DeleteDC(IntPtr hdc);

        [DllImport("gdi32.dll", ExactSpelling = true)]
        public static extern IntPtr SelectObject(IntPtr hDC, IntPtr hObject);

        [DllImport("gdi32.dll", ExactSpelling = true, SetLastError = true)]
        public static extern Bool DeleteObject(IntPtr hObject);

        [DllImport("user32.dll", EntryPoint = "SendMessage")]
        public static extern int SendMessage(int hWnd, int wMsg, int wParam, int lParam);

        [DllImport("user32.dll", EntryPoint = "ReleaseCapture")]
        public static extern int ReleaseCapture();

        public static int LOWORD(int value)
        {
            return value & 65535;
        }
        public static int HIWORD(int value)
        {
            return value >> 16;
        }
    }
}

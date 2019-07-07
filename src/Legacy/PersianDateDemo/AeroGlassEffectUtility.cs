using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Windows.Interop;
using System.Windows;
using System.Windows.Media;

namespace Arash
{
    class AeroGlassEffectUtility
    {
        [DllImport("DwmApi.dll")]
        public static extern int DwmExtendFrameIntoClientArea(
        IntPtr hwnd,
        ref Margins pMarInset);

        public static Margins GetDpiAdjustedMargins(IntPtr windowHandle,
            int left, int right, int top, int bottom)
        {
            // Get the system DPI.
            System.Drawing.Graphics g = System.Drawing.Graphics.FromHwnd(windowHandle);
            float desktopDpiX = g.DpiX;
            float desktopDpiY = g.DpiY;
            // Set the margins.
            Arash.Margins margins = new Arash.Margins();
            margins.cxLeftWidth = Convert.ToInt32(left * (desktopDpiX / 96));
            margins.cxRightWidth = Convert.ToInt32(right * (desktopDpiX / 96));
            margins.cyTopHeight = Convert.ToInt32(top * (desktopDpiX / 96));
            margins.cyBottomHeight = Convert.ToInt32(right * (desktopDpiX / 96));
            return margins;
        }
        public static void ExtendGlass(Window win, int left, int right,
            int top, int bottom)
        {
            // Obtain the Win32 window handle for the WPF window.
            WindowInteropHelper windowInterop = new WindowInteropHelper(win);
            IntPtr windowHandle = windowInterop.Handle;
            // Adjust the margins to take the system DPI into account.
            Margins margins = GetDpiAdjustedMargins(
            windowHandle, left, right, top, bottom);
            // Extend the glass frame.
            int returnVal = DwmExtendFrameIntoClientArea(windowHandle, ref margins);
            HwndSource.FromHwnd(windowHandle).CompositionTarget.BackgroundColor = Colors.Transparent;
            if (returnVal < 0)
            {
                throw new NotSupportedException("Operation failed.");
            }
        }
    }
    [StructLayout(LayoutKind.Sequential)]
    public struct Margins
    {
        public int cxLeftWidth;
        public int cxRightWidth;
        public int cyTopHeight;
        public int cyBottomHeight;
    }

}

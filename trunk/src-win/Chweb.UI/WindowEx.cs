using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Runtime.InteropServices;
using System.Windows.Media;
using System.Windows.Interop;
using SD = System.Drawing;

namespace Chweb.UI
{
    public class WindowEx : Window
    {
        public bool EnableGlass { get; set; }

        [DllImport("DwmApi.dll")]
        static extern int DwmExtendFrameIntoClientArea(
            IntPtr hwnd, ref MARGINS pMarInset);

        [StructLayout(LayoutKind.Sequential)]
        public struct MARGINS
        {
            public int cxLeftWidth;
            public int cxRightWidth;
            public int cyTopHeight;
            public int cyBottomHeight;
        };

        public WindowEx()
        {
            Loaded += new RoutedEventHandler(GlassWindow_Loaded);
        }

        void GlassWindow_Loaded(object sender, RoutedEventArgs e)
        {
            if (EnableGlass)
            {
                Window wnd = (Window)sender;
                Brush originalBackground = wnd.Background;
                wnd.Background = Brushes.Transparent;

                try
                {
                    IntPtr mainWindowPtr = new WindowInteropHelper(wnd).Handle;
                    HwndSource mainWindowSrc = HwndSource.FromHwnd(mainWindowPtr);
                    mainWindowSrc.CompositionTarget.BackgroundColor = Color.FromArgb(0, 0, 0, 0);
                    SD.Graphics desktop = SD.Graphics.FromHwnd(mainWindowPtr);

                    // all -1 makes the entire window glass
                    MARGINS margins = new MARGINS()
                    {
                        cxLeftWidth = -1,
                        cxRightWidth = -1,
                        cyBottomHeight = -1,
                        cyTopHeight = -1
                    };

                    int ret = DwmExtendFrameIntoClientArea(
                        mainWindowSrc.Handle, ref margins);

                    if (ret != 0)
                    {
                        wnd.Background = originalBackground;
                    }
                }
                catch (DllNotFoundException)
                {
                    wnd.Background = originalBackground;
                }
            }
        }
    }
}

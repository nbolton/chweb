using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Interop;
using System.Runtime.InteropServices;
using SD = System.Drawing;
using System.IO;
using System.Diagnostics;
using Microsoft.Win32;
using System.ComponentModel;

namespace Chweb.UI
{
    /// <summary>
    /// Interaction logic for Chooser.xaml
    /// </summary>
    public partial class Chooser : WindowEx
    {
        public string Url { get; set; }

        private BrowserUtil util = new BrowserUtil();

        public Chooser()
        {
            InitializeComponent();

            this.IsVisibleChanged += new DependencyPropertyChangedEventHandler(
                Chooser_IsVisibleChanged);

            addBrowserButtons();
        }

        public void ReloadBrowserButtons()
        {
            BaseGrid.Children.Clear();
            addBrowserButtons();
        }

        private void addBrowserButtons()
        {
            // not sure i like all this
            int i = 0;
            const int iconSize = 24;
            const int iconPadding = 15;

            // add a new icon to the chooser for each browser found
            foreach (BrowserInfo browser in util.GetBrowsers())
            {
                // get the icon from the browser executable
                SD.Icon icon = SD.Icon.ExtractAssociatedIcon(browser.Path);
                SD.Bitmap bitmap = icon.ToBitmap();
                MemoryStream memoryStream = new MemoryStream();

                // save as PNG so we can get transparency, then seek to
                // start to get the first icon (not sure why this is needed)
                bitmap.Save(memoryStream, SD.Imaging.ImageFormat.Png);
                memoryStream.Seek(0, SeekOrigin.Begin);

                // decode the png so we can use it as a control image 
                PngBitmapDecoder ibd = new PngBitmapDecoder(
                    memoryStream,
                    BitmapCreateOptions.None,
                    BitmapCacheOption.Default);

                // margin in this case is always to the left of the button,
                // but not for the first icon (so it looks pretty!)
                int leftMargin = i == 0 ? 0 : (iconSize * i) + (iconPadding * i);

                Image image = new Image()
                {
                    Width = iconSize,
                    Height = iconSize,
                    Source = ibd.Frames.Single(),
                };

                // transparent buttons look nice on glass. also, set the tag
                // as the browser so we can get at it later in the on click.
                // all the rest is just aesthetic stuff.
                Button button = new Button()
                {
                    Content = image,
                    Tag = browser,
                    Background = Brushes.Transparent,
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Top,
                    Margin = new Thickness(leftMargin, 0, 0, 0),
                    Padding = new Thickness(3)
                };

                button.Click += new RoutedEventHandler(button_Click);

                BaseGrid.Children.Add(button);
                i++;
            }
        }

        void Chooser_IsVisibleChanged(
            object sender, DependencyPropertyChangedEventArgs e)
        {
            // on show, set window position to cursor so user doesn't have
            // to do any hard work :)
            if (Visibility == Visibility.Visible)
            {
                Win32Point point = new Win32Point();
                GetCursorPos(ref point);
                Left = point.X - 20;
                Top = point.Y - 35;
            }
        }

        void button_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            startBrowser((BrowserInfo)button.Tag);
        }

        private void startBrowser(BrowserInfo browser)
        {
            // start the browser...
            // this code should probably be in the BrowserInfo class
            Process.Start(new ProcessStartInfo(browser.Path, Url));

            // even though this hides the window anyway, its probably
            // good practice to make a call to close
            Close();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            // we need to keep the process running in the background
            // so it's fast for the user
            e.Cancel = true;
            base.OnClosing(e);
            Hide();
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct Win32Point
        {
            public Int32 X;
            public Int32 Y;
        };

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool GetCursorPos(ref Win32Point pt);
    }
}

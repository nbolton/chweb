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

namespace Chweb.UI
{
    /// <summary>
    /// Interaction logic for Chooser.xaml
    /// </summary>
    public partial class Chooser : WindowEx
    {
        public string Url { get; set; }

        public Chooser()
        {
            InitializeComponent();

            this.IsVisibleChanged += new DependencyPropertyChangedEventHandler(Chooser_IsVisibleChanged);

            EnableGlass = true;

            // TODO: get from registry:
            // HKEY_LOCAL_MACHINE\SOFTWARE\Clients\StartMenuInternet
            BrowserInfo[] browsers = {
                new BrowserInfo() { Path = @"C:\Users\Nick\AppData\Local\Google\Chrome\Application\chrome.exe" },
                new BrowserInfo() { Path = @"C:\Program Files\Internet Explorer\iexplore.exe" },
                new BrowserInfo() { Path = @"C:\Program Files\Mozilla Firefox\firefox.exe" }
            };

            int i = 0;
            const int iconSize = 24;
            const int iconPadding = 15;

            foreach (BrowserInfo browser in browsers)
            {
                SD.Icon icon = SD.Icon.ExtractAssociatedIcon(browser.Path);
                SD.Bitmap bitmap = icon.ToBitmap();
                MemoryStream memoryStream = new MemoryStream();

                bitmap.Save(memoryStream, SD.Imaging.ImageFormat.Png);
                memoryStream.Seek(0, SeekOrigin.Begin);

                PngBitmapDecoder ibd = new PngBitmapDecoder(
                    memoryStream,
                    BitmapCreateOptions.None,
                    BitmapCacheOption.Default);

                int leftMargin = i == 0 ? 0 : (iconSize * i) + (iconPadding * i);

                Image image = new Image() {
                    Width = iconSize,
                    Height = iconSize,
                    Source = ibd.Frames.Single(),
                };

                Button button = new Button() {
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

        [StructLayout(LayoutKind.Sequential)]
        internal struct Win32Point
        {
            public Int32 X;
            public Int32 Y;
        };

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool GetCursorPos(ref Win32Point pt);

        void Chooser_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
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
            BrowserInfo browser = (BrowserInfo)button.Tag;
            Process.Start(new ProcessStartInfo(browser.Path, Url));
            Hide();
        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            base.OnClosing(e);
            Hide();
        }
    }
}

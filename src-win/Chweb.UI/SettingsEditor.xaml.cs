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
using Chweb.UI.Properties;
using System.ComponentModel;

namespace Chweb.UI
{
    /// <summary>
    /// Interaction logic for Settings.xaml
    /// </summary>
    public partial class SettingsEditor : WindowEx
    {
        public event EventHandler Saved;

        private BrowserUtil util = new BrowserUtil();

        public SettingsEditor()
        {
            InitializeComponent();
        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void acceptButton_Click(object sender, RoutedEventArgs e)
        {
            Settings.Default.FirstBrowser = firstTextBox.Text;
            Settings.Default.Save();
            Close();

            OnSaved(EventArgs.Empty);
        }

        protected void OnSaved(EventArgs e)
        {
            if (Saved != null)
            {
                Saved(this, e);
            }
        }

        private void WindowEx_Loaded(object sender, RoutedEventArgs e)
        {
            firstTextBox.Text = Settings.Default.FirstBrowser;
            if (string.IsNullOrEmpty(firstTextBox.Text))
            {
                firstTextBox.Text = util.GetBrowsers().First().FileName;
            }
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            // hide instead of closing; if we close then the whole
            // application will exit (because this is the last window)
            e.Cancel = true;
            base.OnClosing(e);
            Hide();
        }
    }
}

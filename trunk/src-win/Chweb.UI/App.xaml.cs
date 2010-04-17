using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using Chweb.Service;
using Chweb.UI.Properties;
using System.ServiceModel;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Chweb.UI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private IpcService ipcService = new IpcService();
        private Chooser chooser = new Chooser();
        private SystemTray tray = new SystemTray();
        private SettingsEditor settings = new SettingsEditor();
        private ServiceHost serviceHost;

        public App()
        {
            Uri uri = new Uri(Settings.Default.IpcUri);
            serviceHost = new ServiceHost(ipcService, uri);

            tray.Initialize();
            tray.ShowRequest += new EventHandler(tray_ShowRequest);
            tray.ExitRequest += new EventHandler(tray_ExitRequest);
            tray.SettingsRequest += new EventHandler(tray_SettingsRequest);
            tray.RefreshRequest += new EventHandler(tray_RefreshRequest);

            ipcService.ShowChooserRequest +=
                new EventHandler<ShowChooserEventArgs>(
                    ipcService_ShowChooserRequest);

            settings.Saved += new EventHandler(settings_Saved);
        }

        void tray_RefreshRequest(object sender, EventArgs e)
        {
            chooser.ReloadBrowserButtons();
        }

        void settings_Saved(object sender, EventArgs e)
        {
            chooser.ReloadBrowserButtons();
        }

        void tray_SettingsRequest(object sender, EventArgs e)
        {
            settings.Show();
        }

        void tray_ExitRequest(object sender, EventArgs e)
        {
            Shutdown();
        }

        void tray_ShowRequest(object sender, EventArgs e)
        {
            showChooser(string.Empty);
        }

        void ipcService_ShowChooserRequest(object sender, ShowChooserEventArgs e)
        {
            showChooser(e.Url);
        }

        protected override void OnExit(ExitEventArgs e)
        {
            // don't leave messy tray icon behind
            tray.Hide();

            // probably not totally necessary
            serviceHost.Close();

            base.OnExit(e);
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            if (e.Args.Length > 0 && e.Args[0] == "--show")
            {
                string url = string.Empty;
                if (e.Args.Length > 1)
                {
                    url = e.Args[1];
                }
                showChooser(url);
            }

            serviceHost.Open();
        }

        private void showChooser(string url)
        {
            chooser.Url = url;
            chooser.Show();
        }
    }
}

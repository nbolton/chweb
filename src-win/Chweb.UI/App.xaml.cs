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
        private ServiceHost serviceHost;

        public App()
        {
            Uri uri = new Uri(Settings.Default.IpcUri);
            serviceHost = new ServiceHost(ipcService, uri);

            ipcService.ShowChooserRequest +=
                new EventHandler<ShowChooserEventArgs>(
                    ipcService_ShowChooserRequest);
        }

        void ipcService_ShowChooserRequest(object sender, ShowChooserEventArgs e)
        {
            showChooser(e.Url);
        }

        protected override void OnExit(ExitEventArgs e)
        {
            // probably not totally necessary
            serviceHost.Close();

            base.OnExit(e);
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            if (e.Args.Length > 0 && e.Args[0] == "--test")
            {
                showChooser("http://www.google.co.uk");
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

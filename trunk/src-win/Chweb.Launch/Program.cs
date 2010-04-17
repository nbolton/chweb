using System;
using System.Collections.Generic;
using System.Text;
using Chweb.Launch.IpcServiceReference;
using System.Windows;
using System.ServiceModel;
using System.Diagnostics;
using Chweb.Launch.Properties;
using System.Windows.Shapes;
using System.Reflection;
using System.IO;
using IOPath = System.IO.Path;
using System.Threading;

namespace Chweb.Launch
{
    class Program
    {
        const string chwebUiExec = "Chweb.UI.exe";

        private Mode m_mode;

        private enum Mode
        {
            ShowUi,
            MakeDefaultBrowser,
            ShowIcons,
            HideIcons
        }

        static int Main(string[] args)
        {
            // we shouldn't put failable logic in ctor
            Program p = new Program();

            try
            {
                p.run(args);
                return 0;
            }
            catch (Exception ex)
            {
                // always exit gracefully
                MessageBox.Show(
                    "Chweb failed to launch:\n\n" + ex.Message,
                    "Chweb launch error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);

                return 1;
            }
        }

        private void run(string[] args)
        {
            // url can be empty; just means browswer will launch with no page
            string url = string.Empty;

            // set program mode (m_mode default behaviour will show the UI)
            // we realy don't want to launch any functions until we're done
            // processing all of the arguments, as some methods may require
            // multiple arguments to work properly -- this looks like extra
            // work for nothing currently, but will make code cleaner in
            // future.
            foreach (string arg in args)
            {
                switch (arg)
                {
                    case "--make-default-browser":
                        m_mode = Mode.MakeDefaultBrowser;
                        break;

                    case "--show-icons":
                        m_mode = Mode.ShowIcons;
                        break;

                    case "--hide-icons":
                        m_mode = Mode.HideIcons;
                        break;
                    
                    default:
                        if (arg.StartsWith("-"))
                        {
                            throw new ArgumentException(
                                "Argument not supported: " + arg);
                        }

                        // if argument not recognised, and does not begin with
                        // - or --, then assume it's the url that the user 
                        // wants to launch
                        url = arg;
                        break;
                }
            }

            switch (m_mode)
            {
                case Mode.MakeDefaultBrowser:
                    makeDefaultBrowser();
                    break;

                default:
                    showUi(url, true);
                    break;
            }
        }

        private void makeDefaultBrowser()
        {
            // get the filename (with escaped slashes) and use the registry 
            // export format file as a string format to insert filenames
            string regFileCotnent = string.Format(
                Resources.ChwebRegistry, 
                getExecFileInfo().FullName.Replace(@"\", @"\\"));

            // write the reg file to temp and then use regedit to import
            string tempRegFile = IOPath.GetTempFileName();
            File.WriteAllText(tempRegFile, regFileCotnent);
            Process.Start("regedit", "/s " + tempRegFile);

            // now delete the temp file (no longer needed)
            File.Delete(tempRegFile);
        }

        private FileInfo getExecFileInfo()
        {
            return new FileInfo(Assembly.GetExecutingAssembly().Location);
        }

        private void showUi(string url, bool recover)
        {
            // use named pipes to make the wcf call (should be fastest)
            IpcServiceClient client = new IpcServiceClient(
                "NamedPipe_IIpcService");

            try
            {
                // make the call (will only work if ui proc is available)
                client.ShowChooser(url);
            }
            catch (EndpointNotFoundException)
            {
                if (recover)
                {
                    recoverFromNoEndpoint(url);
                }
                else
                {
                    // not allowed to attempt recovery, so just fail
                    throw new Exception(
                        "Unable to connect to UI. Endpoint not found:\n\n" + url);
                }
            }
        }

        private void recoverFromNoEndpoint(string url)
        {
            Debug.WriteLine("Endpoint not found (can't talk to UI).");

            if (isUiProcRunning())
            {
                // this should never happen -- hard to recover from
                throw new Exception(
                    "The UI process is running, but the " +
                    "endpoint is not available: \n" + url);
            }

            string uiExec = string.Format(
                @"{0}\{1}", getExecFileInfo().DirectoryName, chwebUiExec);

            ProcessStartInfo start = new ProcessStartInfo(
                uiExec, "--show " + url);

            try
            {
                Process.Start(start);
            }
            catch (Exception ex)
            {
                throw new Exception(
                    "Could not start UI:\n" +
                    "  " + uiExec + "\n" +
                    "  " + start.Arguments,
                    ex);
            }
        }

        private bool isUiProcRunning()
        {
            return Process.GetProcessesByName("Chweb.UI").Length != 0;
        }
    }
}

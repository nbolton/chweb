using System;
using System.Collections.Generic;
using System.Text;
using Chweb.Launch.IpcServiceReference;
using System.Windows;

namespace Chweb.Launch
{
    class Program
    {
        static int Main(string[] args)
        {
            try
            {
                string url = string.Empty;
                if (args.Length > 0)
                {
                    url = args[0];
                }

                IpcServiceClient client = new IpcServiceClient(
                    "NamedPipe_IIpcService");
                
                client.ShowChooser(url);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.Message, "Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
                return 1;
            }
            
            return 0;
        }
    }
}

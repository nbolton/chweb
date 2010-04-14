using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace Chweb.Service
{
    public class ShowChooserEventArgs : EventArgs
    {
        public string Url { get; set; }
        
        public ShowChooserEventArgs(string url)
        {
            Url = url;
        }
    }

    [ServiceBehavior(InstanceContextMode=InstanceContextMode.Single)]
    public class IpcService : IIpcService
    {
        public event EventHandler<ShowChooserEventArgs> ShowChooserRequest;

        public void ShowChooser(string url)
        {
            OnShowChooserRequest(this, new ShowChooserEventArgs(url));
        }

        public void OnShowChooserRequest(object sender, ShowChooserEventArgs e)
        {
            if (ShowChooserRequest != null)
            {
                ShowChooserRequest(this, e);
            }
        }
    }
}

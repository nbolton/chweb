using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace Chweb.Service
{
    [ServiceContract]
    public interface IIpcService
    {
        [OperationContract]
        void ShowChooser(string url);
    }
}

using System;
using System.IO;

namespace FlexibleProxy.Networking
{
    public class NetworkConnector : IConnector 
    {
        public event Action ContentReceived;

        public Stream ReceiveData()
        {
            return null;
        }

        public void SendData(Stream data)
        {
            
        }
    }
}

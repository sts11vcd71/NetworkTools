using System;

namespace FlexibleProxy.Networking
{
    public class NetworkConnector : IConnector 
    {
        public event Action ContentReceived;

        public byte[] ReceiveData()
        {
            return null;
        }

        public void SendData(byte[] data)
        {
            
        }
    }
}

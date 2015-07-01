using System;

namespace FlexibleProxy.Networking
{
    public interface IConnector
    {
        event Action ContentReceived;
        byte[] ReceiveData();
        void SendData(byte[] data);
    }
}

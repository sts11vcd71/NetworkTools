using System;
using System.Net.Sockets;
using FlexibleProxy.Settings;

namespace FlexibleProxy.Networking
{
    public class OutgoingTcpConnector : IOutgoingConnector
    {
        private TcpClient Connection { get; set; }
        private Channel ChannelSettings { get; set; }

        public OutgoingTcpConnector(Channel channelSettings)
        {
            Connection = new TcpClient();
            ChannelSettings = channelSettings;
        }

        public void Activate()
        {
            Connection.BeginConnect(ChannelSettings.DestinationHost, ChannelSettings.DestinationPort, ReceiveData, null);
        }

        public void Deactivate()
        {
            Connection.Close();
        }

        private void ReceiveData(IAsyncResult result)
        {
            //byte[] data = result.
            if (OnDataReceived != null)
            {
                OnDataReceived(null);
            }
        }

        public event Action<byte[]> OnDataReceived;

        public void SendData(byte[] data)
        {
            var stream = Connection.GetStream();
            stream.Write(data, 0, data.Length);
        }
    }
}

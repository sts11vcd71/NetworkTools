using System.Net;
using System.Net.Sockets;
using FlexibleProxy.Settings;

namespace FlexibleProxy.Networking
{
    public class IncommingTcpConnection : IConnector
    {
        private TcpListener Connection { get; set; }

        private Channel ChannelSettings { get; set; }

        public IncommingTcpConnection(Channel channelSetting)
        {
            ChannelSettings = channelSetting;
            Connection = new TcpListener(IPAddress.Any, channelSetting.SourcePort);
        }
        
        public void SendData(byte[] data)
        {
            
        }

        public void Activate()
        {
            Connection.Start();
        }

        public void Deactivate()
        {
            Connection.Stop();
        }
    }
}

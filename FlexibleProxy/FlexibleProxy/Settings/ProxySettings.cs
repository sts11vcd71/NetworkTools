using System.Collections.Generic;

namespace FlexibleProxy.Settings
{
    public class ProxySettings
    {
        public List<Channel> Channels { get; set; }

        public List<DestinationHost> AvailableHosts { get; set; }
 
        public string ServerHost { get; set; }
    }
}

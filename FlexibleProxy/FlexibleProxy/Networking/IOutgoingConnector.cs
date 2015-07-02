using System;

namespace FlexibleProxy.Networking
{
    public interface IOutgoingConnector : IConnector
    {
        event Action<byte[]> OnDataReceived;
    }
}
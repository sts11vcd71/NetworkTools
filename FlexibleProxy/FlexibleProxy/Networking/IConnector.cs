namespace FlexibleProxy.Networking
{
    public interface IConnector
    {
        void SendData(byte[] data);
        void Activate();
        void Deactivate();
    }
}

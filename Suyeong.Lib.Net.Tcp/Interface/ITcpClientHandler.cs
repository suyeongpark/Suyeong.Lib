namespace Suyeong.Lib.Net.Tcp
{
    public interface ITcpClientHandler
    {
        void StartListen();
        void Close();
        void Send(ITcpPacket packet);
    }
}

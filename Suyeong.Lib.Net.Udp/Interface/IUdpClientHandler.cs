namespace Suyeong.Lib.Net.Udp
{
    public interface IUdpClientHandler
    {
        void StartListen();
        void Close();
        void Send(IUdpPacket packet);
    }
}

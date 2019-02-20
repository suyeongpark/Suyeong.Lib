using System;

namespace Suyeong.Lib.Net.Udp
{
    public interface IUdpConnector
    {
        void Start();
        void Close();
        void Send(IUdpPacket packet, Action<IUdpPacket> callback);
    }
}

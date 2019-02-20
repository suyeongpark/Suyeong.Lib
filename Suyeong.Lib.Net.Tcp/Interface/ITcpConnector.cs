using System;

namespace Suyeong.Lib.Net.Tcp
{
    public interface ITcpConnector
    {
        void Start();
        void Close();
        void Send(ITcpPacket packet, Action<ITcpPacket> callback);
    }
}

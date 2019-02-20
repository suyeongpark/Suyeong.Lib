using System;

namespace Suyeong.Lib.Net.Udp
{
    public interface IUdpPacket
    {
        PacketType Type { get; }
        int Index { get; }
        string Protocol { get; }
        DateTime CreateTime { get; }
    }
}

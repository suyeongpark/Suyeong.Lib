using System;

namespace Suyeong.Lib.Net.Tcp
{
    public interface ITcpPacket
    {
        PacketType Type { get; }
        string Protocol { get; }
        DateTime CreateTime { get; }
    }
}

using System;

namespace Suyeong.Lib.Net.Lib
{
    public interface IPacket
    {
        PacketType Type { get; }
        string Protocol { get; }
    }
}

using System;

namespace Suyeong.Lib.Net.Shared
{
    [Serializable]
    public abstract class Packet : IPacket
    {
        public Packet(string protocol)
        {
            this.Protocol = protocol;
        }

        public string Protocol { get; private set; }
    }
}

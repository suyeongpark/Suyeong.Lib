using System;

namespace Suyeong.Lib.Net.Type
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

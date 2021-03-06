using System;

namespace Suyeong.Lib.Net.Shared
{
    [Serializable]
    public class PacketJson : Packet
    {
        public PacketJson(string protocol, string json) : base(protocol: protocol)
        {
            this.Json = json;
        }

        public string Json { get; private set; }
    }
}

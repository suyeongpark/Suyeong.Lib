using System;

namespace Suyeong.Lib.Net.Shared
{
    [Serializable]
    public class PacketSerialized : Packet
    {
        public PacketSerialized(string protocol, byte[] serializedData) : base(protocol: protocol)
        {
            this.SerializedData = serializedData;
        }

        public byte[] SerializedData { get; private set; }
    }
}

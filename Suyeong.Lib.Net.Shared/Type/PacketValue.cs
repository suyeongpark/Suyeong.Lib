using System;

namespace Suyeong.Lib.Net.Shared
{
    [Serializable]
    public class PacketValue : Packet
    {
        public PacketValue(string protocol, object value) : base(protocol: protocol)
        {
            this.Value = value;
        }

        public object Value { get; private set; }
    }
}

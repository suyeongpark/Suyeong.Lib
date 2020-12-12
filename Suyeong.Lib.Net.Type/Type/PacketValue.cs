using System;

namespace Suyeong.Lib.Net.Type
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

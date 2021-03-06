using System;

namespace Suyeong.Lib.Net.Shared
{
    [Serializable]
    public class PacketFile : Packet
    {
        public PacketFile(string protocol, string desc, byte[] fileData) : base(protocol: protocol)
        {
            this.Desc = desc;
            this.FileData = fileData;
        }

        public string Desc { get; private set; }
        public byte[] FileData { get; private set; }
    }
}

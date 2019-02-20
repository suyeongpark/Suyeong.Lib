using System;
namespace Suyeong.Lib.Net.Udp
{
    [Serializable]
    public abstract class UdpPacket : IUdpPacket
    {
        PacketType type;
        int index;
        string protocol;
        DateTime createTime;

        public UdpPacket(PacketType type, int index, string protocol)
        {
            this.type = type;
            this.index = index;
            this.protocol = protocol;
            this.createTime = DateTime.Now;
        }

        public PacketType Type { get { return this.type; } }
        public int Index { get { return this.index; } }
        public string Protocol { get { return this.protocol; } }
        public DateTime CreateTime { get { return this.createTime; } }
    }

    [Serializable]
    public class UdpPacketMessage : UdpPacket
    {
        object data;

        public UdpPacketMessage(PacketType type, int index, string protocol, object data) : base(type: type, index: index, protocol: protocol)
        {
            this.data = data;
        }

        public object Data { get { return this.data; } }
    }

    [Serializable]
    public class UdpPacketFile : UdpPacket
    {
        string fileName;
        byte[] fileData;

        public UdpPacketFile(PacketType type, int index, string protocol, string fileName, byte[] fileData) : base(type: type, index: index, protocol: protocol)
        {
            this.fileName = fileName;
            this.fileData = fileData;
        }

        public string FileName { get { return this.fileName; } }
        public byte[] FileData { get { return this.fileData; } }
    }

}

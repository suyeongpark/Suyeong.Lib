using System;

namespace Suyeong.Lib.Net.Tcp
{
    [Serializable]
    public abstract class TcpPacket : ITcpPacket
    {
        PacketType type;
        string protocol;
        DateTime createTime;

        public TcpPacket(PacketType type, string protocol)
        {
            this.type = type;
            this.protocol = protocol;
            this.createTime = DateTime.Now;
        }

        public PacketType Type { get { return this.type; } }
        public string Protocol { get { return this.protocol; } }
        public DateTime CreateTime { get { return this.createTime; } }
    }

    [Serializable]
    public class TcpPacketMessage : TcpPacket
    {
        object data;

        public TcpPacketMessage(PacketType type, string protocol, object data) : base(type: type, protocol: protocol)
        {
            this.data = data;
        }

        public object Data { get { return this.data; } }
    }

    [Serializable]
    public class TcpPacketFile : TcpPacket
    {
        string fileName;
        byte[] fileData;

        public TcpPacketFile(PacketType type, string protocol, string fileName, byte[] fileData) : base(type: type, protocol: protocol)
        {
            this.fileName = fileName;
            this.fileData = fileData;
        }

        public string FileName { get { return this.fileName; } }
        public byte[] FileData { get { return this.fileData; } }
    }

}

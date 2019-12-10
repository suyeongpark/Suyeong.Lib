using System;

namespace Suyeong.Lib.Net.Lib
{
    [Serializable]
    public abstract class Packet : IPacket
    {
        public Packet(PacketType type, string protocol)
        {
            this.Type = type;
            this.Protocol = protocol;
        }

        public PacketType Type { get; private set; }
        public string Protocol { get; private set; }
    }

    [Serializable]
    public class PacketValue : Packet
    {
        public PacketValue(PacketType type, string protocol, object value) : base(type: type, protocol: protocol)
        {
            this.Value = value;
        }

        public object Value { get; private set; }
    }

    [Serializable]
    public class PacketJson : Packet
    {
        public PacketJson(PacketType type, string protocol, string json) : base(type: type, protocol: protocol)
        {
            this.Json = json;
        }

        public string Json { get; private set; }
    }

    [Serializable]
    public class PacketSerialized : Packet
    {
        public PacketSerialized(PacketType type, string protocol, byte[] serializedData) : base(type: type, protocol: protocol)
        {
            this.SerializedData = serializedData;
        }

        public byte[] SerializedData { get; private set; }
    }

    [Serializable]
    public class PacketFile : Packet
    {
        public PacketFile(PacketType type, string protocol, string fileName, byte[] fileData) : base(type: type, protocol: protocol)
        {
            this.FileName = fileName;
            this.FileData = fileData;
        }

        public string FileName { get; private set; }
        public byte[] FileData { get; private set; }
    }
}

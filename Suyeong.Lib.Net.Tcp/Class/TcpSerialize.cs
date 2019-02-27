using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Suyeong.Lib.Net.Tcp
{
    public static class TcpSerialize
    {
        public static byte[] SerializeObject(object data)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                binaryFormatter.Serialize(memoryStream, data);

                return memoryStream.ToArray();
            }
        }

        public static object DeserializeObject(byte[] data)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                memoryStream.Write(data, 0, data.Length);
                memoryStream.Position = 0;

                BinaryFormatter binaryFormatter = new BinaryFormatter();
                return binaryFormatter.Deserialize(memoryStream);
            }
        }
    }
}

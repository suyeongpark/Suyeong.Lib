using System;
using System.IO;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Suyeong.Lib.Net.Tcp
{
    public static class TcpStream
    {
        const int SIZE_HEADER = 8; // Packet Type 4 + Data Length 4
        const int INDEX_LENGTH = 4;
        const int MAX_SIZE = 1024;

        public static bool SendPacket(NetworkStream networkStream, byte[] data)
        {
            bool result = false;

            try
            {
                int dataLength = data.Length;

                // 1. header를 보낸다.
                byte[] header = BitConverter.GetBytes(dataLength);
                networkStream.Write(header, 0, header.Length);

                // 2. data를 보낸다.
                SendData(networkStream: networkStream, data: data, dataSize: dataLength);

                // 3. flush
                networkStream.Flush();

                result = true;
            }
            catch (Exception)
            {
                throw;
            }

            return result;
        }

        async public static Task<bool> SendPacketAsync(NetworkStream networkStream, byte[] data)
        {
            bool result = false;

            try
            {
                int dataLength = data.Length;

                // 1. header를 보낸다.
                byte[] header = BitConverter.GetBytes(dataLength);
                await networkStream.WriteAsync(header, 0, header.Length);

                // 2. data를 보낸다.
                await SendDataAsync(networkStream: networkStream, data: data, dataLength: dataLength);

                // 3. flush
                await networkStream.FlushAsync().ConfigureAwait(false);

                result = true;
            }
            catch (Exception)
            {
                throw;
            }

            return result;
        }

        public static byte[] ReceivePacket(NetworkStream networkStream)
        {
            byte[] header = new byte[SIZE_HEADER];
            int nbytes = networkStream.Read(header, 0, header.Length);

            if (nbytes > 0)
            {
                int dataLength = BitConverter.ToInt32(header, 0);  // BitConverter.ToInt32 자체가 4바이트를 읽겠다는 의미라서 Start Index만 있으면 된다.
                return ReceiveData(networkStream: networkStream, dataLength: dataLength);
            }
            else
            {
                return null;
            }
        }

        async public static Task<byte[]> ReceivePacketAsync(NetworkStream networkStream)
        {
            byte[] header = new byte[SIZE_HEADER];
            int nbytes = await networkStream.ReadAsync(header, 0, header.Length).ConfigureAwait(false);

            if (nbytes > 0)
            {
                PacketType type = (PacketType)BitConverter.ToInt32(header, 0);
                int dataSize = BitConverter.ToInt32(header, INDEX_LENGTH);  // BitConverter.ToInt32 자체가 4바이트를 읽겠다는 의미라서 Start Index만 있으면 된다.

                return await ReceiveDataAsync(networkStream: networkStream, dataSize: dataSize);
            }
            else
            {
                return null;
            }
        }

        static bool SendData(NetworkStream networkStream, byte[] data, int dataSize)
        {
            bool result = false;

            try
            {
                using (MemoryStream memoryStream = new MemoryStream(data))
                {
                    byte[] buffer;
                    int nbytes = 0;

                    while (dataSize > 0)
                    {
                        buffer = new byte[MAX_SIZE < dataSize ? MAX_SIZE : dataSize];
                        nbytes = memoryStream.Read(buffer, 0, buffer.Length);
                        networkStream.Write(buffer, 0, buffer.Length);
                        dataSize -= nbytes;
                    }
                }

                result = true;
            }
            catch (Exception)
            {
                throw;
            }

            return result;
        }

        async static Task<bool> SendDataAsync(NetworkStream networkStream, byte[] data, int dataLength)
        {
            bool result = false;

            try
            {
                using (MemoryStream memoryStream = new MemoryStream(data))
                {
                    byte[] buffer;
                    int nbytes = 0;

                    while (dataLength > 0)
                    {
                        buffer = new byte[MAX_SIZE < dataLength ? MAX_SIZE : dataLength];
                        nbytes = await memoryStream.ReadAsync(buffer, 0, buffer.Length).ConfigureAwait(false);
                        await networkStream.WriteAsync(buffer, 0, buffer.Length).ConfigureAwait(false);
                        dataLength -= nbytes;
                    }
                }

                result = true;
            }
            catch (Exception)
            {
                throw;
            }

            return result;
        }

        static byte[] ReceiveData(NetworkStream networkStream, int dataLength)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                byte[] buffer;
                int nbytes = 0;

                while (dataLength > 0)
                {
                    buffer = new byte[MAX_SIZE < dataLength ? MAX_SIZE : dataLength];
                    nbytes = networkStream.Read(buffer, 0, buffer.Length);
                    memoryStream.Write(buffer, 0, buffer.Length);
                    dataLength -= nbytes;
                }

                return memoryStream.ToArray();
            }
        }

        async static Task<byte[]> ReceiveDataAsync(NetworkStream networkStream, int dataSize)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                byte[] buffer;
                int nbytes = 0;

                while (dataSize > 0)
                {
                    buffer = new byte[MAX_SIZE < dataSize ? MAX_SIZE : dataSize];
                    nbytes = await networkStream.ReadAsync(buffer, 0, buffer.Length).ConfigureAwait(false);
                    await memoryStream.WriteAsync(buffer, 0, buffer.Length).ConfigureAwait(false);
                    dataSize -= nbytes;
                }

                return memoryStream.ToArray();
            }
        }
    }
}

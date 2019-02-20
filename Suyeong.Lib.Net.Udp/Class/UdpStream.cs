using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Suyeong.Lib.Net.Udp
{
    public static class UdpStream
    {
        // UDP 상에서 최대 전송 가능한 크기는 65,507 이다
        const int SIZE_HEADER = 4; // UDP는 Type을 보내지 않는다.
        const int MAX_SIZE = 65507;

        public static void Send(UdpClient client, byte[] data)
        {
            int dataSize = data.Length;

            // 1. header를 보낸다. --udp는 header를 보내지 않는다.
            SendHeader(client: client, dataSize: dataSize);

            // 2. data를 보낸다.
            SendData(client: client, dataSize: dataSize, data: data);
        }

        public static byte[] Receive(UdpClient client, IPEndPoint endPoint)
        {
            int dataSize = ReceiveHeader(client: client, endPoint: endPoint);

            return dataSize > 0 ? ReceiveData(client: client, dataSize: dataSize, endPoint: endPoint) : null;
        }

        async public static Task SendAsync(UdpClient client, byte[] data)
        {
            int dataSize = data.Length;

            // 1. header를 보낸다. 
            await SendHeaderAsync(client: client, dataSize: dataSize);

            // 2. data를 보낸다.
            await SendDataAsync(client: client, dataSize: dataSize, data: data);
        }

        async public static Task<byte[]> ReceiveAsync(UdpClient client)
        {
            int dataSize = await ReceiveHeaderAsync(client: client);

            return dataSize > 0 ? await ReceiveDataAsync(client: client, dataSize: dataSize) : null;
        }

        static void SendHeader(UdpClient client, int dataSize)
        {
            byte[] header = BitConverter.GetBytes(dataSize);
            client.Send(header, header.Length);
        }

        static void SendData(UdpClient client, int dataSize, byte[] data)
        {
            int send = 0, count = 0, left = 0;
            byte[] buffer = new byte[MAX_SIZE];

            while (send < dataSize)
            {
                left = dataSize - send;
                count = left < MAX_SIZE ? left : MAX_SIZE;
                data.CopyTo(buffer, send);
                client.Send(buffer, buffer.Length);
                send += count;
            }
        }

        static int ReceiveHeader(UdpClient client, IPEndPoint endPoint)
        {
            byte[] header = client.Receive(ref endPoint);

            return header != null ? BitConverter.ToInt32(header, 0) : 0; // BitConverter.ToInt32 자체가 4바이트를 읽겠다는 의미라서 Start Index만 있으면 된다.
        }

        static byte[] ReceiveData(UdpClient client, int dataSize, IPEndPoint endPoint)
        {
            byte[] data = new byte[dataSize];
            byte[] buffer = new byte[MAX_SIZE];
            int nbytes = 0, received = 0, count = 0, left = 0;

            while (received < dataSize)
            {
                left = dataSize - received;
                count = left < MAX_SIZE ? left : MAX_SIZE;
                buffer = client.Receive(ref endPoint);
                buffer.CopyTo(data, received);
                received += nbytes;
            }

            return data;
        }

        async static Task SendHeaderAsync(UdpClient client, int dataSize)
        {
            byte[] header = BitConverter.GetBytes(dataSize);
            await client.SendAsync(header, header.Length);
        }

        async static Task SendDataAsync(UdpClient client, int dataSize, byte[] data)
        {
            int send = 0, count = 0, left = 0;
            byte[] buffer = new byte[MAX_SIZE];

            while (send < dataSize)
            {
                left = dataSize - send;
                count = left < MAX_SIZE ? left : MAX_SIZE;
                data.CopyTo(buffer, send);
                await client.SendAsync(buffer, buffer.Length);
                send += count;
            }
        }

        async static Task<int> ReceiveHeaderAsync(UdpClient client)
        {
            UdpReceiveResult result = await client.ReceiveAsync();

            return result.Buffer != null ? BitConverter.ToInt32(result.Buffer, 0) : 0; // BitConverter.ToInt32 자체가 4바이트를 읽겠다는 의미라서 Start Index만 있으면 된다.
        }

        async static Task<byte[]> ReceiveDataAsync(UdpClient client, int dataSize)
        {
            byte[] data = new byte[dataSize];
            int nbytes = 0, received = 0, count = 0, left = 0;
            UdpReceiveResult result;

            while (received < dataSize)
            {
                left = dataSize - received;
                count = left < MAX_SIZE ? left : MAX_SIZE;
                result = await client.ReceiveAsync();
                result.Buffer.CopyTo(data, received);
                received += nbytes;
            }

            return data;
        }
    }
}

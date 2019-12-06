using System;
using System.Net.Sockets;
using System.Threading.Tasks;
using Suyeong.Lib.Util;

namespace Suyeong.Lib.Net.Tcp
{
    public class TcpClientAsync
    {
        TcpClient client;

        public TcpClientAsync(string ip, int port)
        {
            this.client = new TcpClient(ip, port);
        }

        async public Task Send(ITcpPacket sendPacket, Action<ITcpPacket> callback)
        {
            ITcpPacket recievePacket;
            PacketType type;
            int dataLength, nbytes;
            byte[] header, sendData, compressData, recieveData, decompressData;

            try
            {
                using (NetworkStream stream = this.client.GetStream())
                {
                    // 1. 보낼 데이터를 압축한다.
                    sendData = StreamUtil.SerializeObject(sendPacket);
                    compressData = await StreamUtil.CompressAsync(data: sendData);

                    // 2. 요청의 헤더를 보낸다.
                    dataLength = compressData.Length;
                    header = BitConverter.GetBytes(dataLength);
                    await stream.WriteAsync(header, 0, header.Length);

                    // 3. 요청을 보낸다.
                    await TcpUtil.SendDataAsync(networkStream: stream, data: compressData, dataLength: dataLength);

                    // 4. 결과의 헤더를 받는다.
                    header = new byte[Consts.SIZE_HEADER];
                    nbytes = await stream.ReadAsync(header, 0, header.Length);

                    if (nbytes > 0)
                    {
                        // 5. 결과의 데이터를 받는다.
                        type = (PacketType)BitConverter.ToInt32(header, 0);
                        dataLength = BitConverter.ToInt32(header, Consts.SIZE_INDEX);  // BitConverter.ToInt32 자체가 4바이트를 읽겠다는 의미라서 Start Index만 있으면 된다.
                        recieveData = await TcpUtil.ReceiveDataAsync(networkStream: stream, dataLength: dataLength);

                        // 6. 결과는 압축되어 있으므로 푼다.
                        decompressData = await StreamUtil.DecompressAsync(data: recieveData);
                        recievePacket = StreamUtil.DeserializeObject(decompressData) as ITcpPacket;

                        // 7. 결과를 처리한다.
                        callback(recievePacket);
                    }

                    await stream.FlushAsync();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}

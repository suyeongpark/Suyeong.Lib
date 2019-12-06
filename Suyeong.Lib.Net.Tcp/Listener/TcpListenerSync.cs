using System;
using System.Net;
using System.Net.Sockets;
using Suyeong.Lib.Util;

namespace Suyeong.Lib.Net.Tcp
{
    public class TcpListenerSync
    {
        TcpListener listener;

        public TcpListenerSync(int portNum)
        {
            this.listener = new TcpListener(new IPEndPoint(IPAddress.Any, portNum));
        }

        public void ListenerStart(Func<ITcpPacket, ITcpPacket> callback)
        {
            this.listener.Start();

            ITcpPacket recievePacket, sendPacket;
            PacketType type;
            int nbytes, dataLength;
            byte[] header, recieveData, sendData, decompressData, compressData;

            while (true)
            {
                try
                {
                    using (TcpClient client = this.listener.AcceptTcpClient())
                    using (NetworkStream stream = client.GetStream())
                    {
                        // 1. 요청 헤더를 받는다.
                        header = new byte[Consts.SIZE_HEADER];
                        nbytes = stream.Read(header, 0, header.Length);

                        if (nbytes > 0)
                        {
                            // 2. 요청 데이터를 받는다.
                            type = (PacketType)BitConverter.ToInt32(header, 0);
                            dataLength = BitConverter.ToInt32(header, Consts.SIZE_INDEX);  // BitConverter.ToInt32 자체가 4바이트를 읽겠다는 의미라서 Start Index만 있으면 된다.
                            recieveData = TcpUtil.ReceiveData(networkStream: stream, dataLength: dataLength);

                            // 3. 받은 요청은 압축되어 있으므로 푼다.
                            decompressData = StreamUtil.Decompress(data: recieveData);
                            recievePacket = StreamUtil.DeserializeObject(decompressData) as ITcpPacket;

                            // 4. 요청을 처리한다.
                            sendPacket = callback(recievePacket);

                            // 5. 처리 결과를 압축한다.
                            sendData = StreamUtil.SerializeObject(sendPacket);
                            compressData = StreamUtil.Compress(data: sendData);

                            // 6. 처리한 결과의 헤더를 보낸다.
                            dataLength = compressData.Length;
                            header = BitConverter.GetBytes(dataLength);
                            stream.Write(header, 0, header.Length);

                            // 7. 처리한 결과의 데이터를 보낸다.
                            TcpUtil.SendData(networkStream: stream, data: compressData, dataLength: dataLength);

                            // 8. flush
                            stream.Flush();
                        }
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }

        public void ListenerStop()
        {
            this.listener.Stop();
        }
    }
}

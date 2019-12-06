using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using Suyeong.Lib.Util;

namespace Suyeong.Lib.Net.Tcp
{
    public class TcpListenerCryptAsync
    {
        TcpListener listener;
        byte[] key, iv;

        public TcpListenerCryptAsync(int portNum, byte[] key, byte[] iv)
        {
            this.key = key;
            this.iv = iv;
            this.listener = new TcpListener(new IPEndPoint(IPAddress.Any, portNum));
        }

        async public Task ListenerStartAsync(Func<ITcpPacket, Task<ITcpPacket>> callback)
        {
            this.listener.Start();

            ITcpPacket recievePacket, sendPacket;
            PacketType type;
            int nbytes, dataLength;
            byte[] header, recieveData, sendData, decryptData, encryptData;

            while (true)
            {
                try
                {
                    using (TcpClient client = await this.listener.AcceptTcpClientAsync().ConfigureAwait(false))
                    using (NetworkStream stream = client.GetStream())
                    {
                        // 1. 요청 헤더를 받는다.
                        header = new byte[Consts.SIZE_HEADER];
                        nbytes = await stream.ReadAsync(header, 0, header.Length);

                        if (nbytes > 0)
                        {
                            // 2. 요청 데이터를 받는다.
                            type = (PacketType)BitConverter.ToInt32(header, 0);
                            dataLength = BitConverter.ToInt32(header, Consts.SIZE_INDEX);  // BitConverter.ToInt32 자체가 4바이트를 읽겠다는 의미라서 Start Index만 있으면 된다.
                            recieveData = await TcpUtil.ReceiveDataAsync(networkStream: stream, dataLength: dataLength);

                            // 3. 받은 요청은 암호화되어 있으므로 푼다.
                            decryptData = await StreamUtil.DecryptAsync(data: recieveData, key: this.key, iv: this.iv);
                            recievePacket = StreamUtil.DeserializeObject(decryptData) as ITcpPacket;

                            // 4. 요청을 처리한다.
                            sendPacket = await callback(recievePacket);

                            // 5. 처리 결과를 압호화한다.
                            sendData = StreamUtil.SerializeObject(sendPacket);
                            encryptData = await StreamUtil.EncryptAsync(data: sendData, key: this.key, iv: this.iv);

                            // 6. 처리한 결과의 헤더를 보낸다.
                            dataLength = encryptData.Length;
                            header = BitConverter.GetBytes(dataLength);
                            await stream.WriteAsync(header, 0, header.Length);

                            // 7. 처리한 결과의 데이터를 보낸다.
                            await TcpUtil.SendDataAsync(networkStream: stream, data: encryptData, dataLength: dataLength);

                            // 8. flush
                            await stream.FlushAsync();
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

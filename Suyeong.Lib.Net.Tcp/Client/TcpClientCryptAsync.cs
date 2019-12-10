using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using Suyeong.Lib.Net.Lib;

namespace Suyeong.Lib.Net.Tcp
{
    public class TcpClientCryptAsync
    {
        IPEndPoint serverEndPoint;
        byte[] key, iv;

        public TcpClientCryptAsync(string serverIP, int serverPort, byte[] key, byte[] iv)
        {
            this.serverEndPoint = new IPEndPoint(address: IPAddress.Parse(serverIP), port: serverPort);
            this.key = key;
            this.iv = iv;
        }

        async public Task<IPacket> Send(IPacket sendPacket)
        {
            try
            {
                using (TcpClient client = new TcpClient(localEP: this.serverEndPoint))
                using (NetworkStream stream = client.GetStream())
                {
                    // 1. 보낼 데이터를 압축한다.
                    byte[] sendData = NetUtil.SerializeObject(data: sendPacket);
                    byte[] encryptData = await NetUtil.EncryptAsync(data: sendData, key: this.key, iv: this.iv);

                    // 2. 요청의 헤더를 보낸다.
                    int sendDataLength = encryptData.Length;
                    byte[] sendHeader = BitConverter.GetBytes(value: sendDataLength);
                    await stream.WriteAsync(buffer: sendHeader, offset: 0, count: sendHeader.Length);

                    // 3. 요청을 보낸다.
                    await TcpUtil.SendDataAsync(networkStream: stream, data: encryptData, dataLength: sendDataLength);

                    // 4. 결과의 헤더를 받는다.
                    byte[] receiveHeader = new byte[Consts.SIZE_HEADER];
                    int nbytes = await stream.ReadAsync(buffer: receiveHeader, offset: 0, count: receiveHeader.Length);

                    // 5. 결과의 데이터를 받는다.
                    int receiveDataLength = BitConverter.ToInt32(value: receiveHeader, startIndex: 0);
                    byte[] receiveData = await TcpUtil.ReceiveDataAsync(networkStream: stream, dataLength: receiveDataLength);

                    // 6. 결과는 압축되어 있으므로 푼다.
                    byte[] decryptData = await NetUtil.DecryptAsync(data: receiveData, key: this.key, iv: this.iv);
                    IPacket receivePacket = NetUtil.DeserializeObject(data: decryptData) as IPacket;

                    await stream.FlushAsync();

                    // 7. 결과를 처리한다.
                    return receivePacket;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }

    public class TcpClientCryptAsyncs : List<TcpClientCryptAsync>
    {
        public TcpClientCryptAsyncs()
        {

        }
    }
}

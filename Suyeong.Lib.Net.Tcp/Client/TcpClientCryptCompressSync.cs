using System;
using System.Collections.Generic;
using System.Net.Sockets;
using Suyeong.Lib.Net.Lib;

namespace Suyeong.Lib.Net.Tcp
{
    public class TcpClientCryptCompressSync
    {
        string serverIP;
        int serverPort;
        byte[] key, iv;

        public TcpClientCryptCompressSync(string serverIP, int serverPort, byte[] key, byte[] iv)
        {
            this.serverIP = serverIP;
            this.serverPort = serverPort;
            this.key = key;
            this.iv = iv;
        }

        public IPacket Send(IPacket sendPacket)
        {
            IPacket receivePacket = default;

            try
            {
                using (TcpClient client = new TcpClient(hostname: this.serverIP, port: this.serverPort))
                using (NetworkStream stream = client.GetStream())
                {
                    // 1. 보낼 데이터를 암호화한다.
                    byte[] sendData = NetUtil.SerializeObject(data: sendPacket);
                    byte[] encryptData = NetUtil.EncryptWithCompress(data: sendData, key: this.key, iv: this.iv);

                    // 2. 요청의 헤더를 보낸다.
                    int sendDataLength = encryptData.Length;
                    byte[] sendHeader = BitConverter.GetBytes(value: sendDataLength);
                    stream.Write(buffer: sendHeader, offset: 0, size: sendHeader.Length);

                    // 3. 요청을 보낸다.
                    TcpUtil.SendData(networkStream: stream, data: encryptData, dataLength: sendDataLength);

                    // 4. 결과의 헤더를 받는다.
                    byte[] receiveHeader = new byte[Consts.SIZE_HEADER];
                    int nbytes = stream.Read(buffer: receiveHeader, offset: 0, size: receiveHeader.Length);

                    // 5. 결과의 데이터를 받는다.
                    int receiveDataLength = BitConverter.ToInt32(value: receiveHeader, startIndex: 0);
                    byte[] receiveData = TcpUtil.ReceiveData(networkStream: stream, dataLength: receiveDataLength);

                    // 6. 결과는 압축되어 있으므로 푼다.
                    byte[] decryptData = NetUtil.DecryptWithDecompress(data: receiveData, key: this.key, iv: this.iv);
                    receivePacket = NetUtil.DeserializeObject(data: decryptData) as IPacket;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            return receivePacket;
        }
    }

    public class TcpClientCryptCompressSyncs : List<TcpClientCryptCompressSync>
    {
        public TcpClientCryptCompressSyncs()
        {

        }
    }
}

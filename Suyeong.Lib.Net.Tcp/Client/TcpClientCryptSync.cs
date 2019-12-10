using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using Suyeong.Lib.Net.Lib;

namespace Suyeong.Lib.Net.Tcp
{
    public class TcpClientCryptSync
    {
        IPEndPoint serverEndPoint;
        byte[] key, iv;

        public TcpClientCryptSync(string serverIP, int serverPort, byte[] key, byte[] iv)
        {
            this.serverEndPoint = new IPEndPoint(address: IPAddress.Parse(serverIP), port: serverPort);
            this.key = key;
            this.iv = iv;
        }

        public IPacket Send(IPacket sendPacket)
        {
            try
            {
                using (TcpClient client = new TcpClient(localEP: this.serverEndPoint))
                using (NetworkStream stream = client.GetStream())
                {
                    // 1. 보낼 데이터를 암호화한다.
                    byte[] sendData = NetUtil.SerializeObject(data: sendPacket);
                    byte[] encryptData = NetUtil.Encrypt(data: sendData, key: this.key, iv: this.iv);

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
                    PacketType type = (PacketType)BitConverter.ToInt32(value: receiveHeader, startIndex: 0);
                    int receiveDataLength = BitConverter.ToInt32(value: receiveHeader, startIndex: Consts.SIZE_INDEX);  // BitConverter.ToInt32 자체가 4바이트를 읽겠다는 의미라서 Start Index만 있으면 된다.
                    byte[] receiveData = TcpUtil.ReceiveData(networkStream: stream, dataLength: receiveDataLength);

                    // 6. 결과는 압축되어 있으므로 푼다.
                    byte[] decryptData = NetUtil.Decrypt(data: receiveData, key: this.key, iv: this.iv);
                    IPacket receivePacket = NetUtil.DeserializeObject(decryptData) as IPacket;

                    stream.Flush();

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

    public class TcpClientCryptSyncs : List<TcpClientCryptSync>
    {
        public TcpClientCryptSyncs()
        {

        }
    }
}

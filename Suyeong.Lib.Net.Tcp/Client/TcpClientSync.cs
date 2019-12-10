using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using Suyeong.Lib.Net.Lib;

namespace Suyeong.Lib.Net.Tcp
{
    public class TcpClientSync
    {
        IPEndPoint serverEndPoint;

        public TcpClientSync(string serverIP, int serverPort)
        {
            this.serverEndPoint = new IPEndPoint(address: IPAddress.Parse(serverIP), port: serverPort);
        }

        public IPacket Send(IPacket sendPacket)
        {
            try
            {
                using (TcpClient client = new TcpClient(localEP: this.serverEndPoint))
                using (NetworkStream stream = client.GetStream())
                {
                    // 1. 보낼 데이터를 압축한다.
                    byte[] sendData = NetUtil.SerializeObject(data: sendPacket);
                    byte[] compressData = NetUtil.Compress(data: sendData);

                    // 2. 요청의 헤더를 보낸다.
                    int sendDataLength = compressData.Length;
                    byte[] sendHeader = BitConverter.GetBytes(value: sendDataLength);
                    stream.Write(buffer: sendHeader, offset: 0, size: sendHeader.Length);

                    // 3. 요청을 보낸다.
                    TcpUtil.SendData(networkStream: stream, data: compressData, dataLength: sendDataLength);

                    // 4. 결과의 헤더를 받는다.
                    byte[] receiveHeader = new byte[Consts.SIZE_HEADER];
                    int nbytes = stream.Read(buffer: receiveHeader, offset: 0, size: receiveHeader.Length);

                    // 5. 결과의 데이터를 받는다.
                    int receiveDataLength = BitConverter.ToInt32(value: receiveHeader, startIndex: 0);
                    byte[] receiveData = TcpUtil.ReceiveData(networkStream: stream, dataLength: receiveDataLength);

                    // 6. 결과는 압축되어 있으므로 푼다.
                    byte[] decompressData = NetUtil.Decompress(data: receiveData);
                    IPacket receivePacket = NetUtil.DeserializeObject(data: decompressData) as IPacket;

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

    public class TcpClientSyncs : List<TcpClientSync>
    {
        public TcpClientSyncs()
        {

        }
    }
}

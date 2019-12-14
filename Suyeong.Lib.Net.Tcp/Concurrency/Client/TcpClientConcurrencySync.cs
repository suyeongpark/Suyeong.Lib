﻿using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading;
using Suyeong.Lib.Net.Lib;

namespace Suyeong.Lib.Net.Tcp
{
    public class TcpClientConcurrencySync : IDisposable
    {
        TcpClient client;

        public TcpClientConcurrencySync(string serverIP, int serverPort)
        {
            this.client = new TcpClient(hostname: serverIP, port: serverPort);
        }

        public void Dispose()
        {
            this.client.Close();
        }

        public void Start(string channelID, string userID, Action<IPacket> callback)
        {
            NetworkStream stream;
            IPacket received;
            byte[] receiveHeader, receiveData, decompressData;
            int nbytes, receiveDataLength;

            // 일단 접속할 channel id와 user id를 보낸다.
            // protocol에 channel id를 넣고, value에 user id를 넣는다.
            PacketValue packet = new PacketValue(protocol: channelID, value: userID);
            Send(packet: packet);

            // 그 후에 서버에서 오는 메세지를 듣기 위해 별도의 쓰레드를 돌린다.
            Thread thread = new Thread(() =>
            {
                while (true)
                {
                    try
                    {
                        stream = this.client.GetStream();

                        // 1. 결과의 헤더를 받는다.
                        receiveHeader = new byte[Consts.SIZE_HEADER];
                        nbytes = stream.Read(buffer: receiveHeader, offset: 0, size: receiveHeader.Length);

                        // 2. 결과의 데이터를 받는다.
                        receiveDataLength = BitConverter.ToInt32(value: receiveHeader, startIndex: 0);
                        receiveData = TcpUtil.ReceiveData(networkStream: stream, dataLength: receiveDataLength);

                        stream.Flush();

                        // 3. 결과는 압축되어 있으므로 푼다.
                        decompressData = NetUtil.Decompress(data: receiveData);
                        received = NetUtil.DeserializeObject(data: decompressData) as IPacket;

                        callback(received);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                        Dispose();
                    }
                }
            });

            thread.IsBackground = true;
            thread.Start();
        }

        public void Send(IPacket packet)
        {
            try
            {
                NetworkStream stream = this.client.GetStream();

                // 1. 보낼 데이터를 압축한다.
                byte[] sendData = NetUtil.SerializeObject(data: packet);
                byte[] compressData = NetUtil.Compress(data: sendData);

                // 2. 요청의 헤더를 보낸다.
                int sendDataLength = compressData.Length;
                byte[] sendHeader = BitConverter.GetBytes(value: sendDataLength);
                stream.Write(buffer: sendHeader, offset: 0, size: sendHeader.Length);

                // 3. 요청을 보낸다.
                TcpUtil.SendData(networkStream: stream, data: compressData, dataLength: sendDataLength);

                stream.Flush();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }

    public class TcpClientConcurrencySyncs : List<TcpClientConcurrencySync>
    {
        public TcpClientConcurrencySyncs()
        {

        }
    }
}

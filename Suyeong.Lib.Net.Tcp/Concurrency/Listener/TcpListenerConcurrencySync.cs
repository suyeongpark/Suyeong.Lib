using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using Suyeong.Lib.Net.Lib;

namespace Suyeong.Lib.Net.Tcp
{
    public class TcpListenerConcurrencySync : IDisposable
    {
        TcpListener listener;
        TcpClientHandlerConcurrencySyncDicGroup handlerDicGroup;
        Func<IPacket, IPacket> callback;

        public TcpListenerConcurrencySync(int portNum)
        {
            this.listener = new TcpListener(new IPEndPoint(address: IPAddress.Any, port: portNum));
            this.handlerDicGroup = new TcpClientHandlerConcurrencySyncDicGroup();
        }

        public void Dispose()
        {
            this.listener.Stop();
        }

        /// <summary>
        /// 사용자 입장과 퇴장에 대한 User Enter, User Exit 프로토콜 구현 필요
        /// Protocol: "UserEnter", Value: user ID
        /// Protocol: "UserExit", Value: user ID
        /// </summary>
        /// <param name="callback"></param>
        public void Start(Func<IPacket, IPacket> callback)
        {
            this.callback = callback;

            this.listener.Start();

            TcpClient client;
            NetworkStream stream;
            TcpClientHandlerConcurrencySync handler;
            TcpClientHandlerConcurrencySyncDic handlerDic;
            PacketValue connectPacket;
            string channelID, clientID;
            int receiveDataLength, nbytes;
            byte[] receiveHeader, receiveData, decompressData;

            while (true)
            {
                try
                {
                    client = this.listener.AcceptTcpClient();
                    stream = client.GetStream();

                    // 사용자가 접속하면 우선 사용자로부터 입장할 채널 정보와 사용자 정보를 받는다. 

                    // 1. 요청 헤더를 받는다.
                    receiveHeader = new byte[Consts.SIZE_HEADER];
                    nbytes = stream.Read(buffer: receiveHeader, offset: 0, size: receiveHeader.Length);

                    // 2. 요청 데이터를 받는다.
                    receiveDataLength = BitConverter.ToInt32(value: receiveHeader, startIndex: 0);
                    receiveData = TcpUtil.ReceiveData(networkStream: stream, dataLength: receiveDataLength);

                    stream.Flush();

                    // 3. 받은 요청은 압축되어 있으므로 푼다.
                    decompressData = NetUtil.Decompress(data: receiveData);
                    connectPacket = NetUtil.DeserializeObject(data: decompressData) as PacketValue;                    

                    // protocol에 channel id를 넣고, value에 user id를 넣는다.
                    channelID = connectPacket.Protocol;
                    clientID = connectPacket.Value.ToString();

                    handler = new TcpClientHandlerConcurrencySync(channelID: channelID, clientID: clientID, client: client);
                    handler.Receive += Receive;
                    handler.Disconnect += Disconnect;

                    if (this.handlerDicGroup.TryGetValue(channelID, out handlerDic))
                    {
                        handlerDic.Add(clientID, handler);

                        handlerDicGroup[channelID] = handlerDic;
                    }
                    else
                    {
                        handlerDic = new TcpClientHandlerConcurrencySyncDic();
                        handlerDic.Add(clientID, handler);

                        handlerDicGroup.Add(channelID, handlerDic);
                    }

                    handler.Start();

                    IPacket sendPacket = callback(new PacketValue(protocol: "UserEnter", clientID));
                    SendToChannel(channelID: channelID, sendPacket: sendPacket);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }
        }

        public void SendToChannel(string channelID, IPacket sendPacket)
        {
            TcpClientHandlerConcurrencySyncDic handlerDic;

            if (this.handlerDicGroup.TryGetValue(channelID, out handlerDic))
            {
                foreach (KeyValuePair<string, TcpClientHandlerConcurrencySync> kvp in handlerDic)
                {
                    kvp.Value.Send(packet: sendPacket);
                }
            }
        }

        public void SendToServer(IPacket sendPacket)
        {
            foreach (KeyValuePair<string, TcpClientHandlerConcurrencySyncDic> kvp in this.handlerDicGroup)
            {
                foreach (KeyValuePair<string, TcpClientHandlerConcurrencySync> kvp2 in kvp.Value)
                {
                    kvp2.Value.Send(packet: sendPacket);
                }
            }
        }

        void Receive(string channelID, IPacket receivePacket)
        {
            // 클라이언트가 발생시킨 요청을 처리하고 그 결과를 채널 내 사용자들에게 모두 broadcasting 한다. 당사자 포함.
            IPacket sendPacket = this.callback(receivePacket);

            SendToChannel(channelID: channelID, sendPacket: sendPacket);
        }

        void Disconnect(string channelID, string clientID)
        {
            TcpClientHandlerConcurrencySyncDic handlerDic;

            if (this.handlerDicGroup.TryGetValue(channelID, out handlerDic))
            {
                handlerDic.Remove(clientID);
            }

            IPacket sendPacket = callback(new PacketValue(protocol: "UserExit", clientID));
            SendToChannel(channelID: channelID, sendPacket: sendPacket);
        }
    }

    public class TcpListenerConcurrencySyncs : List<TcpListenerConcurrencySync>
    {
        public TcpListenerConcurrencySyncs()
        {

        }
    }
}

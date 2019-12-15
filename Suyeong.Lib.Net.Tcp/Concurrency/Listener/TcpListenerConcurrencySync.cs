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
        Func<string, string, IPacket> userEnter, userExit;
        Func<IPacket, IPacket> response;

        /// <summary>
        /// userEnter, userExit은 사용자의 입장과 퇴장에 대한 callback으로써 StageID, UserID를 받고 IPacket을 반환한다.
        /// </summary>
        /// <param name="portNum"></param>
        /// <param name="userEnterCallback"></param>
        /// <param name="userExitCallback"></param>
        /// <param name="responseCallbak"></param>
        public TcpListenerConcurrencySync(int portNum, Func<string, string, IPacket> userEnterCallback, Func<string, string, IPacket> userExitCallback, Func<IPacket, IPacket> responseCallbak)
        {
            this.userEnter = userEnterCallback;
            this.userExit = userExitCallback;
            this.response = responseCallbak;

            this.listener = new TcpListener(new IPEndPoint(address: IPAddress.Any, port: portNum));
            this.handlerDicGroup = new TcpClientHandlerConcurrencySyncDicGroup();
        }

        public void Dispose()
        {
            this.listener.Stop();
        }

        public void Start()
        {
            this.listener.Start();

            TcpClient client;
            NetworkStream stream;
            string stageID, userID;

            while (true)
            {
                try
                {
                    client = this.listener.AcceptTcpClient();
                    stream = client.GetStream();

                    // 사용자가 접속하면 우선 사용자로부터 입장할 채널 정보와 사용자 정보를 받는다. 
                    GetStageIdAndUserID(stream: stream, stageID: out stageID, userID: out userID);

                    // 사용자 정보를 이용해서 handler를 추가한다.
                    AddAndStartHandler(client: client, stageID: stageID, userID: userID);

                    // 사용자가 입장한 정보를 broadcast 한다.
                    IPacket sendPacket = this.userEnter(stageID, userID);
                    BroadcastToStage(stageID: stageID, sendPacket: sendPacket);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }
        }

        /// <summary>
        /// 사용자가 stage를 떠나거나 옮길 때 사용
        /// </summary>
        /// <param name="stageID"></param>
        /// <param name="userID"></param>
        public void LeaveStage(string stageID, string userID)
        {
            TcpClientHandlerConcurrencySyncDic handlerDic;
            TcpClientHandlerConcurrencySync handler;

            if (this.handlerDicGroup.TryGetValue(stageID, out handlerDic))
            {
                if (handlerDic.TryGetValue(userID, out handler))
                {
                    handler.Dispose();
                }

                handlerDic.Remove(userID);
            }

            IPacket sendPacket = this.userExit(stageID, userID);
            BroadcastToStage(stageID: stageID, sendPacket: sendPacket);
        }

        public void BroadcastToServer(IPacket sendPacket)
        {
            foreach (KeyValuePair<string, TcpClientHandlerConcurrencySyncDic> kvp in this.handlerDicGroup)
            {
                foreach (KeyValuePair<string, TcpClientHandlerConcurrencySync> kvp2 in kvp.Value)
                {
                    kvp2.Value.Send(packet: sendPacket);
                }
            }
        }

        void GetStageIdAndUserID(NetworkStream stream, out string stageID, out string userID)
        {
            // 1. 요청 헤더를 받는다.
            byte[] receiveHeader = new byte[Consts.SIZE_HEADER];
            int nbytes = stream.Read(buffer: receiveHeader, offset: 0, size: receiveHeader.Length);

            // 2. 요청 데이터를 받는다.
            int receiveDataLength = BitConverter.ToInt32(value: receiveHeader, startIndex: 0);
            byte[] receiveData = TcpUtil.ReceiveData(networkStream: stream, dataLength: receiveDataLength);

            stream.Flush();

            // 3. 받은 요청은 압축되어 있으므로 푼다.
            byte[] decompressData = NetUtil.Decompress(data: receiveData);
            PacketValue connectPacket = NetUtil.DeserializeObject(data: decompressData) as PacketValue;

            // protocol에 입장하려는 stage의 id를 넣고, value에 user id를 넣는다.
            stageID = connectPacket.Protocol;
            userID = connectPacket.Value.ToString();
        }

        void AddAndStartHandler(TcpClient client, string stageID, string userID)
        {
            // 사용자 정보를 이용해서 handler를 만든다.
            TcpClientHandlerConcurrencySync handler = new TcpClientHandlerConcurrencySync(stageID: stageID, userID: userID, client: client);
            handler.Disconnect += LeaveStage;
            handler.Receive += Receive;

            TcpClientHandlerConcurrencySyncDic handlerDic;

            if (this.handlerDicGroup.TryGetValue(stageID, out handlerDic))
            {
                handlerDic.Add(userID, handler);

                handlerDicGroup[stageID] = handlerDic;
            }
            else
            {
                handlerDic = new TcpClientHandlerConcurrencySyncDic();
                handlerDic.Add(userID, handler);

                handlerDicGroup.Add(stageID, handlerDic);
            }

            // hander를 시작한다.
            handler.Start();
        }

        void BroadcastToStage(string stageID, IPacket sendPacket)
        {
            TcpClientHandlerConcurrencySyncDic handlerDic;

            if (this.handlerDicGroup.TryGetValue(stageID, out handlerDic))
            {
                foreach (KeyValuePair<string, TcpClientHandlerConcurrencySync> kvp in handlerDic)
                {
                    kvp.Value.Send(packet: sendPacket);
                }
            }
        }

        void Receive(string channelID, IPacket receivePacket)
        {
            // 클라이언트가 발생시킨 요청을 처리하고 그 결과를 채널 내 사용자들에게 모두 broadcasting 한다. 당사자 포함.
            IPacket sendPacket = this.response(receivePacket);
            BroadcastToStage(stageID: channelID, sendPacket: sendPacket);
        }
    }

    public class TcpListenerConcurrencySyncs : List<TcpListenerConcurrencySync>
    {
        public TcpListenerConcurrencySyncs()
        {

        }
    }
}

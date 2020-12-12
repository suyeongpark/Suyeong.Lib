﻿using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using Suyeong.Lib.Net.Type;

namespace Suyeong.Lib.Net.Tcp
{
    public class TcpListenerConcurrencyCryptAsync : IDisposable
    {
        TcpListener listener;
        TcpClientHandlerConcurrencyCryptAsyncGroupDictionary handlerDicGroup;
        Func<string, string, Task<IPacket>> userEnterAsync, userExitAsync;
        Func<IPacket, Task<IPacket>> response;
        byte[] key, iv;
        bool disposedValue;  // 중복호출 제거용

        /// <summary>
        /// userEnter, userExit은 사용자의 입장과 퇴장에 대한 callback으로써 StageID, UserID를 받고 IPacket을 반환한다.
        /// </summary>
        /// <param name="portNum"></param>
        /// <param name="userEnterCallbackAsync"></param>
        /// <param name="userExitCallbackAsync"></param>
        /// <param name="responseCallbakAsync"></param>
        public TcpListenerConcurrencyCryptAsync(int portNum, byte[] key, byte[] iv, Func<string, string, Task<IPacket>> userEnterCallbackAsync, Func<string, string, Task<IPacket>> userExitCallbackAsync, Func<IPacket, Task<IPacket>> responseCallbakAsync)
        {
            this.key = key;
            this.iv = iv;
            this.userEnterAsync = userEnterCallbackAsync;
            this.userExitAsync = userExitCallbackAsync;
            this.response = responseCallbakAsync;

            this.listener = new TcpListener(new IPEndPoint(address: IPAddress.Any, port: portNum));
            this.handlerDicGroup = new TcpClientHandlerConcurrencyCryptAsyncGroupDictionary();
            this.disposedValue = false;
        }

        // TODO: 아래의 Dispose(bool disposing)에 관리되지 않는 리소스를 해제하는 코드가 포함되어 있는 경우에만 종료자를 재정의합니다. 
        ~TcpListenerConcurrencyCryptAsync()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposedValue)
            {
                // TODO: 관리되는 상태(관리되는 개체)를 삭제
                if (disposing)
                {
                    this.listener.Stop();
                }

                // TODO: 관리되지 않는 리소스(관리되지 않는 개체)를 해제

                // TODO: 큰 필드를 null로 설정.

                this.disposedValue = true;
            }
        }

        async public Task StartAsync()
        {
            this.listener.Start();

            TcpClient client;
            NetworkStream stream;
            TcpClientHandlerConcurrencyCryptAsync handler;
            PacketValue connectPacket;
            int nbytes, receiveDataLength;
            string stageID, userID;
            byte[] receiveHeader, receiveData, decryptData;

            while (true)
            {
                try
                {
                    client = await this.listener.AcceptTcpClientAsync().ConfigureAwait(false);
                    stream = client.GetStream();

                    // 사용자가 접속하면 우선 사용자로부터 입장할 채널 정보와 사용자 정보를 받는다. 
                    // 1. 요청 헤더를 받는다.
                    receiveHeader = new byte[Consts.SIZE_HEADER];
                    nbytes = await stream.ReadAsync(buffer: receiveHeader, offset: 0, count: receiveHeader.Length).ConfigureAwait(false);

                    // 2. 요청 데이터를 받는다.
                    receiveDataLength = BitConverter.ToInt32(value: receiveHeader, startIndex: 0);
                    receiveData = await TcpUtil.ReceiveDataAsync(networkStream: stream, dataLength: receiveDataLength).ConfigureAwait(false);

                    await stream.FlushAsync().ConfigureAwait(false);

                    // 3. 받은 요청은 암호화되어 있으므로 푼다.
                    decryptData = await NetUtil.DecryptAsync(data: receiveData, key: this.key, iv: this.iv).ConfigureAwait(false);
                    connectPacket = NetUtil.DeserializeObject(data: decryptData) as PacketValue;

                    // protocol에 입장하려는 stage의 id를 넣고, value에 user id를 넣는다.
                    stageID = connectPacket.Protocol;
                    userID = connectPacket.Value.ToString();

                    // 사용자 정보를 이용해서 handler를 추가한다.

                    // 사용자 정보를 이용해서 handler를 만든다.
                    handler = new TcpClientHandlerConcurrencyCryptAsync(client: client, stageID: stageID, userID: userID, key: this.key, iv: this.iv);
                    handler.Disconnect += DisconnectAsync;
                    handler.Receive += ReceiveAsync;

                    AddStage(handler: handler, stageID: stageID, userID: userID);

                    // hander를 시작한다.
                    await Task.Run(handler.StartAsync).ConfigureAwait(false);

                    // 사용자가 입장한 정보를 broadcast 한다.
                    IPacket sendPacket = await this.userEnterAsync(stageID, userID).ConfigureAwait(false);
                    await BroadcastToStageAsync(stageID: stageID, sendPacket: sendPacket).ConfigureAwait(false);
                }
                catch (SocketException ex)
                {
                    Console.WriteLine(ex);
                }
            }
        }

        async public Task DisconnectAsync(string stageID, string userID)
        {
            TcpClientHandlerConcurrencyCryptAsyncDictionary handlerDic;
            TcpClientHandlerConcurrencyCryptAsync handler;

            if (this.handlerDicGroup.TryGetValue(stageID, out handlerDic))
            {
                if (handlerDic.TryGetValue(userID, out handler))
                {
                    handler.Dispose();
                }

                handlerDic.Remove(userID);
            }

            IPacket sendPacket = await this.userExitAsync(stageID, userID).ConfigureAwait(false);
            await BroadcastToStageAsync(stageID: stageID, sendPacket: sendPacket).ConfigureAwait(false);
        }

        async public Task MoveStageAsync(string oldStageID, string newStageID, string userID)
        {
            // 1. 기존 stage에서 제거
            TcpClientHandlerConcurrencyCryptAsync handler = RemoveStage(stageID: oldStageID, userID: userID);
            handler.SetStageID(stageID: newStageID);

            // 2. 기존 stage에 퇴장 알림
            IPacket exitPacket = await this.userExitAsync(oldStageID, userID).ConfigureAwait(false);
            await BroadcastToStageAsync(stageID: oldStageID, sendPacket: exitPacket).ConfigureAwait(false);

            // 3. 새로운 stage로 입장
            AddStage(handler: handler, stageID: newStageID, userID: userID);

            // 4. 새로운 stage에 입장 알림
            IPacket enterPacket = await this.userEnterAsync(newStageID, userID).ConfigureAwait(false);
            await BroadcastToStageAsync(stageID: newStageID, sendPacket: enterPacket).ConfigureAwait(false);
        }

        async public Task BroadcastToServerAsync(IPacket sendPacket)
        {
            foreach (KeyValuePair<string, TcpClientHandlerConcurrencyCryptAsyncDictionary> kvp in this.handlerDicGroup)
            {
                foreach (KeyValuePair<string, TcpClientHandlerConcurrencyCryptAsync> kvp2 in kvp.Value)
                {
                    await kvp2.Value.SendAsync(packet: sendPacket).ConfigureAwait(false);
                }
            }
        }

        void AddStage(TcpClientHandlerConcurrencyCryptAsync handler, string stageID, string userID)
        {
            TcpClientHandlerConcurrencyCryptAsyncDictionary handlerDic;

            if (this.handlerDicGroup.TryGetValue(stageID, out handlerDic))
            {
                handlerDic.Add(userID, handler);

                this.handlerDicGroup[stageID] = handlerDic;
            }
            else
            {
                handlerDic = new TcpClientHandlerConcurrencyCryptAsyncDictionary();
                handlerDic.Add(userID, handler);

                this.handlerDicGroup.Add(stageID, handlerDic);
            }
        }

        TcpClientHandlerConcurrencyCryptAsync RemoveStage(string stageID, string userID)
        {
            TcpClientHandlerConcurrencyCryptAsyncDictionary handlerDic;
            TcpClientHandlerConcurrencyCryptAsync handler;

            if (this.handlerDicGroup.TryGetValue(stageID, out handlerDic))
            {
                if (handlerDic.TryGetValue(userID, out handler))
                {
                    handlerDic.Remove(userID);
                    this.handlerDicGroup[stageID] = handlerDic;

                    return handler;
                }
            }

            return null;
        }


        async Task ReceiveAsync(string stageID, IPacket receivePacket)
        {
            // 클라이언트가 발생시킨 요청을 처리하고 그 결과를 채널 내 사용자들에게 모두 broadcasting 한다. 당사자 포함.
            IPacket sendPacket = await this.response(receivePacket).ConfigureAwait(false);

            // disconnect나 stage 이동시에는 null이 나올 수 있음
            if (sendPacket != null)
            {
                await BroadcastToStageAsync(stageID: stageID, sendPacket: sendPacket).ConfigureAwait(false);
            }
        }

        async Task BroadcastToStageAsync(string stageID, IPacket sendPacket)
        {
            TcpClientHandlerConcurrencyCryptAsyncDictionary handlerDic;

            if (this.handlerDicGroup.TryGetValue(stageID, out handlerDic))
            {
                foreach (KeyValuePair<string, TcpClientHandlerConcurrencyCryptAsync> kvp in handlerDic)
                {
                    await kvp.Value.SendAsync(packet: sendPacket).ConfigureAwait(false);
                }
            }
        }
    }

    public class TcpListenerConcurrencyCryptAsyncCollection : List<TcpListenerConcurrencyCryptAsync>
    {
        public TcpListenerConcurrencyCryptAsyncCollection()
        {

        }
    }
}

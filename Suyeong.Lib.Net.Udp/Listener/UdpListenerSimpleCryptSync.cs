﻿using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using Suyeong.Lib.Net.Type;

namespace Suyeong.Lib.Net.Udp
{
    public class UdpListenerSimpleCryptSync : IDisposable
    {
        UdpClient listener;
        byte[] key, iv;
        bool listenOn;
        bool disposedValue;  // 중복호출 제거용

        public UdpListenerSimpleCryptSync(int portNum, byte[] key, byte[] iv)
        {
            this.listener = new UdpClient(portNum);
            this.key = key;
            this.iv = iv;
            this.disposedValue = false;
        }

        // TODO: 아래의 Dispose(bool disposing)에 관리되지 않는 리소스를 해제하는 코드가 포함되어 있는 경우에만 종료자를 재정의합니다. 
        ~UdpListenerSimpleCryptSync()
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
                    this.listener.Close();
                }

                // TODO: 관리되지 않는 리소스(관리되지 않는 개체)를 해제

                // TODO: 큰 필드를 null로 설정.

                this.disposedValue = true;
            }
        }

        public void Start(Func<IPacket, IPacket> callback)
        {
            listenOn = true;

            IPacket receivePacket, sendPacket;
            byte[] receiveData, sendData, decryptData, encryptData;
            IPEndPoint clientEndPoint = new IPEndPoint(IPAddress.Any, 0);

            while (this.listenOn)
            {
                try
                {
                    // 1. 요청을 받는다.
                    receiveData = listener.Receive(remoteEP: ref clientEndPoint);

                    // 2. 요청은 암호화되어 있으므로 푼다.
                    decryptData = NetUtil.Decrypt(data: receiveData, key: this.key, iv: this.iv);
                    receivePacket = NetUtil.DeserializeObject(data: decryptData) as IPacket;

                    // 3. 요청을 처리한다.
                    sendPacket = callback(receivePacket);

                    // 4. 처리 결과를 암호화한다.
                    sendData = NetUtil.SerializeObject(data: sendPacket);
                    encryptData = NetUtil.Encrypt(data: sendData, key: this.key, iv: this.iv);

                    // 5. 요청을 보내온 곳으로 결과를 보낸다.
                    listener.Send(dgram: encryptData, bytes: encryptData.Length, endPoint: clientEndPoint);
                }
                catch (SocketException ex)
                {
                    Console.WriteLine(ex);
                }
            }
        }
    }

    public class UdpListenerSimpleCryptSyncCollection : List<UdpListenerSimpleCryptSync>
    {
        public UdpListenerSimpleCryptSyncCollection()
        {

        }
    }
}

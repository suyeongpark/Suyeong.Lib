﻿using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using Suyeong.Lib.Net.Lib;

namespace Suyeong.Lib.Net.Udp
{
    public class UdpListenerSimpleCryptSync
    {
        UdpClient listener;
        byte[] key, iv;
        bool listenOn;

        public UdpListenerSimpleCryptSync(int portNum, byte[] key, byte[] iv)
        {
            this.listener = new UdpClient(portNum);
            this.key = key;
            this.iv = iv;
        }

        ~UdpListenerSimpleCryptSync()
        {
            this.listener.Close();
        }

        public EndPoint LocalEndPoint
        {
            get { return this.listener.Client.LocalEndPoint; }
        }

        public void ListenerStart(Func<IPacket, IPacket> callback)
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

                    // 2. 요청은 압축되어 있으므로 푼다.
                    decryptData = NetUtil.Decrypt(data: receiveData, key: this.key, iv: this.iv);
                    receivePacket = NetUtil.DeserializeObject(data: decryptData) as IPacket;

                    // 3. 요청을 처리한다.
                    sendPacket = callback(receivePacket);

                    // 4. 처리 결과를 압축한다.
                    sendData = NetUtil.SerializeObject(data: sendPacket);
                    encryptData = NetUtil.Encrypt(data: sendData, key: this.key, iv: this.iv);

                    // 5. 요청을 보내온 곳으로 결과를 보낸다.
                    listener.Send(dgram: encryptData, bytes: encryptData.Length, endPoint: clientEndPoint);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }
        }

        public void ListenerStop()
        {
            listenOn = false;
        }
    }

    public class UdpListenerCryptSyncs : List<UdpListenerSimpleCryptSync>
    {
        public UdpListenerCryptSyncs()
        {

        }
    }
}

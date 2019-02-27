﻿using System;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Suyeong.Lib.Net.Tcp
{
    public class TcpClientHandler : ITcpClientHandler
    {
        public event Action<string> OnMessage;
        public event Action<Guid> OnDisconnect;
        public event Action<Guid, ITcpPacket> OnRequest;

        TcpClient client;
        NetworkStream networkStream;
        Guid guid;
        bool isConnected;

        public TcpClientHandler(TcpClient client, Guid guid)
        {
            this.client = client;
            this.guid = guid;
            this.networkStream = this.client.GetStream();
        }

        public void StartListen()
        {
            this.isConnected = true;
            Task.Run(() => ListenAsync());
        }

        public void Close()
        {
            this.isConnected = false;
            this.networkStream.Close();
            this.client.Close();
        }

        public void Send(ITcpPacket packet)
        {
            Task.Run(() => SendAsync(packet: packet));
        }

        async Task ListenAsync()
        {
            try
            {
                ITcpPacket request;
                byte[] source, decompress;

                while (this.isConnected)
                {
                    source = await TcpStream.ReceivePacketAsync(networkStream: this.networkStream);

                    if (source != null)
                    {
                        decompress = await TcpUtil.DecompressAsync(data: source);
                        request = TcpUtil.DeserializeObject(decompress) as ITcpPacket;

                        // this는 response를 받기 위한 용도
                        OnRequest(this.guid, request);
                    }
                }
            }
            catch (Exception ex)
            {
                this.OnMessage?.Invoke(ex.ToString());
                this.OnDisconnect?.Invoke(this.guid);
            }
        }

        async Task SendAsync(ITcpPacket packet)
        {
            try
            {
                byte[] source = TcpUtil.SerializeObject(packet);
                byte[] compress = await TcpUtil.CompressAsync(data: source);

                await TcpStream.SendPacketAsync(networkStream: this.networkStream, data: compress);
            }
            catch (Exception ex)
            {
                this.OnMessage?.Invoke(ex.ToString());
                this.OnDisconnect?.Invoke(this.guid);
            }
        }
    }
}

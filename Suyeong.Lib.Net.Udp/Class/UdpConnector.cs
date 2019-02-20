using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading.Tasks;
using Suyeong.Lib.Util;

namespace Suyeong.Lib.Net.Udp
{
    public class UdpConnector : IUdpConnector
    {
        public event Action<string> OnMessage;
        public event Action<IUdpPacket> OnNotice;

        UdpClient client;
        bool isConnected;
        Dictionary<string, Action<IUdpPacket>> callbackDic;

        public UdpConnector(string ip, int port)
        {
            this.client = new UdpClient(ip, port);
            this.callbackDic = new Dictionary<string, Action<IUdpPacket>>();
        }

        public void Start()
        {
            this.isConnected = true;
            Task.Run(() => StartAsync());
        }

        public void Close()
        {
            UdpPacketMessage send = new UdpPacketMessage(type: PacketType.Message, index: 0, protocol: "Exit", data: string.Empty);

            Send(packet: send, callback: (packet) =>
            {
                this.isConnected = false;
                UdpPacketMessage receive = packet as UdpPacketMessage;
                bool result = (bool)receive.Data;

                this.client.Close();
            });
        }

        public void Send(IUdpPacket packet, Action<IUdpPacket> callback)
        {
            Task.Run(() => SendAsync(packet: packet, callback: callback));
        }

        async Task StartAsync()
        {
            try
            {
                IUdpPacket result;
                byte[] source, decompress;
                Action<IUdpPacket> callback;

                while (this.isConnected)
                {
                    source = await UdpStream.ReceiveAsync(client: this.client);

                    if (source != null)
                    {
                        decompress = await Deflates.DecompressAsync(data: source);
                        result = Utils.BinaryToObject(decompress) as IUdpPacket;

                        // callbackDic에 있었으면 클라이언트가 요청을 보낸 것에 대한 응답
                        if (this.callbackDic.TryGetValue(result.Protocol, out callback))
                        {
                            this.callbackDic.Remove(result.Protocol);
                            callback(result);
                        }
                        // callbackDic에 없었으면 서버에서 보내온 Broadcast
                        else
                        {
                            this.OnNotice?.Invoke(result);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                this.OnMessage?.Invoke(ex.ToString());
                Close();
            }
        }

        async Task SendAsync(IUdpPacket packet, Action<IUdpPacket> callback)
        {
            // protocol을 callback key로 사용한다.
            if (this.callbackDic.ContainsKey(packet.Protocol))
            {
                this.callbackDic[packet.Protocol] = callback;
            }
            else
            {
                this.callbackDic.Add(packet.Protocol, callback);
            }

            try
            {
                byte[] source = Utils.ObjectToBinary(packet);
                byte[] compress = await Deflates.CompressAsync(data: source);

                await UdpStream.SendAsync(client: this.client, data: compress);
            }
            catch (Exception ex)
            {
                this.OnMessage?.Invoke(ex.ToString());
                Close();
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading.Tasks;
using Suyeong.Lib.Util;

namespace Suyeong.Lib.Net.Udp
{
    public class UdpConnectorCrypt : IUdpConnector
    {
        public event Action<string> OnMessage;
        public event Action<IUdpPacket> OnNotice;

        UdpClient client;
        Dictionary<string, Action<IUdpPacket>> callbackDic;
        byte[] cryptKey, cryptIV;
        bool isConnected;

        public UdpConnectorCrypt(string ip, int port, byte[] cryptKey, byte[] cryptIV)
        {
            this.client = new UdpClient(ip, port);
            this.cryptKey = cryptKey;
            this.cryptIV = cryptIV;
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
                byte[] source, decrypt;
                Action<IUdpPacket> callback;

                while (this.isConnected)
                {
                    source = await UdpStream.ReceiveAsync(client: this.client);

                    if (source != null)
                    {
                        // 암호해제에는 압축해제도 포함되어 있다.
                        decrypt = await CryptUtil.DecryptAsync(data: source, key: this.cryptKey, iv: this.cryptIV);
                        result = Utils.DeserializeObject(decrypt) as IUdpPacket;

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
                byte[] source = Utils.SerializeObject(packet);
                // 암호화에는 압축도 포함되어 있다.
                byte[] encrypt = await CryptUtil.EncryptAsync(data: source, key: this.cryptKey, iv: this.cryptIV);

                await UdpStream.SendAsync(client: this.client, data: encrypt);
            }
            catch (Exception ex)
            {
                this.OnMessage?.Invoke(ex.ToString());
                Close();
            }
        }
    }
}

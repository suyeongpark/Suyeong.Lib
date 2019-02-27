using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Suyeong.Lib.Net.Tcp
{
    public class TcpConnectorCrypt : ITcpConnector
    {
        public event Action<string> OnMessage;
        public event Action<ITcpPacket> OnNotice;

        bool isConnected;
        TcpClient client;
        NetworkStream networkStream;
        byte[] cryptKey, cryptIV;
        Dictionary<string, Action<ITcpPacket>> callbackDic;

        public TcpConnectorCrypt(string ip, int port, byte[] cryptKey, byte[] cryptIV)
        {
            this.client = new TcpClient(ip, port);
            this.cryptKey = cryptKey;
            this.cryptIV = cryptIV;
            this.callbackDic = new Dictionary<string, Action<ITcpPacket>>();
            this.networkStream = this.client.GetStream();
        }

        public void Start()
        {
            this.isConnected = true;
            Task.Run(() => StartAsync());
        }

        public void Close()
        {
            TcpPacketMessage send = new TcpPacketMessage(type: PacketType.Message, protocol: "Exit", data: string.Empty);

            Send(packet: send, callback: (packet) =>
            {
                this.isConnected = false;
                TcpPacketMessage receive = packet as TcpPacketMessage;
                bool result = (bool)receive.Data;

                this.networkStream.Close();
                this.client.Close();
            });
        }

        public void Send(ITcpPacket packet, Action<ITcpPacket> callback)
        {
            Task.Run(() => SendAsync(packet: packet, callback: callback));
        }

        async Task StartAsync()
        {
            try
            {
                ITcpPacket result;
                byte[] source, decrypt;
                Action<ITcpPacket> callback;

                while (this.isConnected)
                {
                    source = await TcpStream.ReceivePacketAsync(networkStream: this.networkStream);

                    if (source != null)
                    {
                        // 암호해제에는 압축해제도 포함되어 있다.
                        decrypt = await TcpCrypt.DecryptAsync(data: source, key: this.cryptKey, iv: this.cryptIV);
                        result = TcpSerialize.DeserializeObject(decrypt) as ITcpPacket;

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

        async Task SendAsync(ITcpPacket packet, Action<ITcpPacket> callback)
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
                byte[] source = TcpSerialize.SerializeObject(packet);
                byte[] encrypt = await TcpCrypt.EncryptAsync(data: source, key: this.cryptKey, iv: this.cryptIV);  // 암호화에는 압축도 포함되어 있다.

                await TcpStream.SendPacketAsync(networkStream: this.networkStream, data: encrypt);
            }
            catch (Exception ex)
            {
                this.OnMessage?.Invoke(ex.ToString());
                Close();
            }
        }
    }
}

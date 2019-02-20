using System;
using System.Net.Sockets;
using System.Threading.Tasks;
using Suyeong.Lib.Util;

namespace Suyeong.Lib.Net.Udp
{
    public class UdpClientHandlerCrypt : IUdpClientHandler
    {
        public event Action<string> OnMessage;
        public event Action<Guid> OnDisconnect;
        public event Action<IUdpPacket, IUdpClientHandler> OnRequest;

        UdpClient client;
        Guid guid;
        byte[] cryptKey, cryptIV;
        bool isConnected;

        public UdpClientHandlerCrypt(UdpClient client, Guid guid, byte[] cryptKey, byte[] cryptIV)
        {
            this.client = client;
            this.guid = guid;
            this.cryptKey = cryptKey;
            this.cryptIV = cryptIV;
        }

        public void StartListen()
        {
            this.isConnected = true;
            Task.Run(() => ListenAsync());
        }

        public void Close()
        {
            this.isConnected = false;
            this.client.Close();
        }

        public void Send(IUdpPacket packet)
        {
            Task.Run(() => SendAsync(packet: packet));
        }

        async Task ListenAsync()
        {
            try
            {
                IUdpPacket request;
                byte[] source, decrypt;

                while (this.isConnected)
                {
                    source = await UdpStream.ReceiveAsync(client: this.client);

                    if (source != null)
                    {
                        // 암호해제에는 압축해제도 포함되어 있다.
                        decrypt = await Crypts.DecryptAsync(data: source, key: this.cryptKey, iv: this.cryptIV);
                        request = Utils.BinaryToObject(decrypt) as IUdpPacket;

                        OnRequest(request, this);
                    }
                }
            }
            catch (Exception ex)
            {
                this.OnMessage?.Invoke(ex.ToString());
                this.OnDisconnect?.Invoke(this.guid);
            }
        }

        async Task SendAsync(IUdpPacket packet)
        {
            try
            {
                byte[] source = Utils.ObjectToBinary(packet);
                // 암호화에는 압축도 포함되어 있다.
                byte[] encrypt = await Crypts.EncryptAsync(data: source, key: this.cryptKey, iv: this.cryptIV);

                await UdpStream.SendAsync(client: this.client, data: encrypt);
            }
            catch (Exception ex)
            {
                this.OnMessage?.Invoke(ex.ToString());
                this.OnDisconnect?.Invoke(this.guid);
            }
        }
    }
}

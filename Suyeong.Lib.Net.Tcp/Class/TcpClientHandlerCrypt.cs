using System;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Suyeong.Lib.Net.Tcp
{
    public class TcpClientHandlerCrypt : ITcpClientHandler
    {
        public event Action<string> OnMessage;
        public event Action<Guid> OnDisconnect;
        public event Action<Guid, ITcpPacket> OnRequest;

        TcpClient client;
        NetworkStream networkStream;
        Guid guid;
        byte[] cryptKey, cryptIV;
        bool isConnected;

        public TcpClientHandlerCrypt(TcpClient client, Guid guid, byte[] cryptKey, byte[] cryptIV)
        {
            this.client = client;
            this.guid = guid;
            this.cryptKey = cryptKey;
            this.cryptIV = cryptIV;
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
                byte[] source, decrypt;

                while (this.isConnected)
                {
                    source = await TcpStream.ReceivePacketAsync(networkStream: this.networkStream);

                    if (source != null)
                    {
                        // 암호해제에는 압축해제도 포함되어 있다.
                        decrypt = await TcpCrypt.DecryptAsync(data: source, key: this.cryptKey, iv: this.cryptIV);
                        request = TcpSerialize.DeserializeObject(decrypt) as ITcpPacket;

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
                byte[] source = TcpSerialize.SerializeObject(packet);
                byte[] encrypt = await TcpCrypt.EncryptAsync(data: source, key: this.cryptKey, iv: this.cryptIV);  // 암호화에는 압축도 포함되어 있다.

                await TcpStream.SendPacketAsync(networkStream: this.networkStream, data: encrypt);
            }
            catch (Exception ex)
            {
                this.OnMessage?.Invoke(ex.ToString());
                this.OnDisconnect?.Invoke(this.guid);
            }
        }
    }
}

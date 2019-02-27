using System;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Suyeong.Lib.Net.Udp
{
    public class UdpClientHandler : IUdpClientHandler
    {
        public event Action<string> OnMessage;
        public event Action<Guid> OnDisconnect;
        public event Action<IUdpPacket, IUdpClientHandler> OnRequest;

        UdpClient client;
        Guid guid;
        bool isConnected;

        public UdpClientHandler(UdpClient client, Guid guid)
        {
            this.client = client;
            this.guid = guid;
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
                byte[] source, decompress;

                while (this.isConnected)
                {
                    source = await UdpStream.ReceiveAsync(client: this.client);

                    if (source != null)
                    {
                        decompress = await UdpUtil.DecompressAsync(data: source);
                        request = UdpUtil.DeserializeObject(decompress) as IUdpPacket;

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
                byte[] source = UdpUtil.SerializeObject(packet);
                byte[] compress = await UdpUtil.CompressAsync(data: source);

                await UdpStream.SendAsync(client: this.client, data: compress);
            }
            catch (Exception ex)
            {
                this.OnMessage?.Invoke(ex.ToString());
                this.OnDisconnect?.Invoke(this.guid);
            }
        }
    }
}

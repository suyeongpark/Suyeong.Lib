using System;
using System.Net.Sockets;
using System.Threading.Tasks;
using Suyeong.Lib.Util;

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
                        decompress = await Deflates.DecompressAsync(data: source);
                        request = Utils.BinaryToObject(decompress) as IUdpPacket;

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
                byte[] compress = await Deflates.CompressAsync(data: source);

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

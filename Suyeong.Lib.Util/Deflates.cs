using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;

namespace Suyeong.Lib.Util
{
    public static class Deflates
    {
        public static byte[] Compress(byte[] data)
        {
            MemoryStream outputStream = new MemoryStream();

            using (DeflateStream deflateStream = new DeflateStream(outputStream, CompressionMode.Compress))
            {
                deflateStream.Write(data, 0, data.Length);
            }

            return outputStream.ToArray();
        }

        public static byte[] Decompress(byte[] data)
        {
            using (MemoryStream outputStream = new MemoryStream())
            using (MemoryStream inputStream = new MemoryStream(data))
            using (DeflateStream deflateStream = new DeflateStream(inputStream, CompressionMode.Decompress))
            {
                deflateStream.CopyTo(outputStream);
                return outputStream.ToArray();
            }
        }

        async public static Task<byte[]> CompressAsync(byte[] data)
        {
            MemoryStream outputStream = new MemoryStream();

            using (DeflateStream deflateStream = new DeflateStream(outputStream, CompressionMode.Compress))
            {
                await deflateStream.WriteAsync(data, 0, data.Length);
            }

            return outputStream.ToArray();
        }

        async public static Task<byte[]> DecompressAsync(byte[] data)
        {
            using (MemoryStream outputStream = new MemoryStream())
            using (MemoryStream inputStream = new MemoryStream(data))
            using (DeflateStream deflateStream = new DeflateStream(inputStream, CompressionMode.Decompress))
            {
                await deflateStream.CopyToAsync(outputStream);
                return outputStream.ToArray();
            }
        }
    }
}

using System.IO;
using System.IO.Compression;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace Suyeong.Lib.Util
{
    public static class Crypts
    {
        public static byte[] Encrypt(byte[] data, byte[] key, byte[] iv)
        {
            MemoryStream memoryStream = new MemoryStream();

            using (Aes algorithm = Aes.Create())
            using (ICryptoTransform encryptor = algorithm.CreateEncryptor(key, iv))
            using (CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
            using (DeflateStream deflateStream = new DeflateStream(cryptoStream, CompressionMode.Compress))
            {
                deflateStream.Write(data, 0, data.Length);
            }

            return memoryStream.ToArray();
        }

        public static byte[] Decrypt(byte[] data, byte[] key, byte[] iv)
        {
            using (MemoryStream outputStream = new MemoryStream())
            using (MemoryStream inputStream = new MemoryStream(data))
            using (Aes algorithm = Aes.Create())
            using (ICryptoTransform decryptor = algorithm.CreateDecryptor(key, iv))
            using (CryptoStream cryptoStream = new CryptoStream(inputStream, decryptor, CryptoStreamMode.Read))
            using (DeflateStream deflateStream = new DeflateStream(cryptoStream, CompressionMode.Decompress))
            {
                deflateStream.CopyTo(outputStream);
                return outputStream.ToArray();
            }
        }

        async public static Task<byte[]> EncryptAsync(byte[] data, byte[] key, byte[] iv)
        {
            MemoryStream memoryStream = new MemoryStream();

            using (Aes algorithm = Aes.Create())
            using (ICryptoTransform encryptor = algorithm.CreateEncryptor(key, iv))
            using (CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
            using (DeflateStream deflateStream = new DeflateStream(cryptoStream, CompressionMode.Compress))
            {
                await deflateStream.WriteAsync(data, 0, data.Length);
            }

            return memoryStream.ToArray();
        }

        async public static Task<byte[]> DecryptAsync(byte[] data, byte[] key, byte[] iv)
        {
            using (MemoryStream outputStream = new MemoryStream())
            using (MemoryStream inputStream = new MemoryStream(data))
            using (Aes algorithm = Aes.Create())
            using (ICryptoTransform decryptor = algorithm.CreateDecryptor(key, iv))
            using (CryptoStream cryptoStream = new CryptoStream(inputStream, decryptor, CryptoStreamMode.Read))
            using (DeflateStream deflateStream = new DeflateStream(cryptoStream, CompressionMode.Decompress))
            {
                await deflateStream.CopyToAsync(outputStream);
                return outputStream.ToArray();
            }
        }
    }
}
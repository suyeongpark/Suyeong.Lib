using System;
using System.IO;
using System.IO.Compression;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace Suyeong.Lib.Util
{
    public static class StreamUtil
    {
        public static byte[] SerializeObject(object obj)
        {
            byte[] serialized = null;

            try
            {
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    BinaryFormatter binaryFormatter = new BinaryFormatter();
                    binaryFormatter.Serialize(memoryStream, obj);

                    serialized = memoryStream.ToArray();
                }
            }
            catch (Exception)
            {
                throw;
            }

            return serialized;
        }

        public static object DeserializeObject(byte[] data)
        {
            object obj = null;

            try
            {
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    memoryStream.Write(data, 0, data.Length);
                    memoryStream.Position = 0;

                    BinaryFormatter binaryFormatter = new BinaryFormatter();
                    obj = binaryFormatter.Deserialize(memoryStream);
                }
            }
            catch (Exception)
            {
                throw;
            }

            return obj;
        }

        public static byte[] Compress(byte[] data)
        {
            byte[] compressed = null;

            try
            {
                MemoryStream outputStream = new MemoryStream();

                using (DeflateStream deflateStream = new DeflateStream(outputStream, CompressionMode.Compress))
                {
                    deflateStream.Write(data, 0, data.Length);
                }

                compressed = outputStream.ToArray();

            }
            catch (Exception)
            {
                throw;
            }

            return compressed;
        }

        async public static Task<byte[]> CompressAsync(byte[] data)
        {
            byte[] compressed = null;

            try
            {
                MemoryStream outputStream = new MemoryStream();

                using (DeflateStream deflateStream = new DeflateStream(outputStream, CompressionMode.Compress))
                {
                    await deflateStream.WriteAsync(data, 0, data.Length).ConfigureAwait(false);
                }

                compressed = outputStream.ToArray();
            }
            catch (Exception)
            {
                throw;
            }

            return compressed;
        }

        public static byte[] Decompress(byte[] data)
        {
            byte[] decompressed = null;

            try
            {
                using (MemoryStream outputStream = new MemoryStream())
                using (MemoryStream inputStream = new MemoryStream(data))
                using (DeflateStream deflateStream = new DeflateStream(inputStream, CompressionMode.Decompress))
                {
                    deflateStream.CopyTo(outputStream);
                    decompressed = outputStream.ToArray();
                }
            }
            catch (Exception)
            {
                throw;
            }

            return decompressed;
        }

        async public static Task<byte[]> DecompressAsync(byte[] data)
        {
            byte[] decompressed = null;

            try
            {
                using (MemoryStream outputStream = new MemoryStream())
                using (MemoryStream inputStream = new MemoryStream(data))
                using (DeflateStream deflateStream = new DeflateStream(inputStream, CompressionMode.Decompress))
                {
                    await deflateStream.CopyToAsync(outputStream).ConfigureAwait(false);
                    decompressed = outputStream.ToArray();
                }
            }
            catch (Exception)
            {
                throw;
            }

            return decompressed;
        }

        public static byte[] Encrypt(byte[] data, byte[] key, byte[] iv)
        {
            byte[] encrypted = null;

            try
            {
                MemoryStream memoryStream = new MemoryStream();

                using (Aes algorithm = Aes.Create())
                using (ICryptoTransform encryptor = algorithm.CreateEncryptor(key, iv))
                using (CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                {
                    cryptoStream.Write(data, 0, data.Length);
                }

                encrypted = memoryStream.ToArray();
            }
            catch (Exception)
            {
                throw;
            }

            return encrypted;
        }

        async public static Task<byte[]> EncryptAsync(byte[] data, byte[] key, byte[] iv)
        {
            byte[] encrypted = null;

            try
            {
                MemoryStream memoryStream = new MemoryStream();

                using (Aes algorithm = Aes.Create())
                using (ICryptoTransform encryptor = algorithm.CreateEncryptor(key, iv))
                using (CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                {
                    await cryptoStream.WriteAsync(data, 0, data.Length).ConfigureAwait(false);
                }

                encrypted = memoryStream.ToArray();
            }
            catch (Exception)
            {
                throw;
            }

            return encrypted;
        }

        public static byte[] Decrypt(byte[] data, byte[] key, byte[] iv)
        {
            byte[] decrypted = null;

            try
            {
                using (MemoryStream outputStream = new MemoryStream())
                using (MemoryStream inputStream = new MemoryStream(data))
                using (Aes algorithm = Aes.Create())
                using (ICryptoTransform decryptor = algorithm.CreateDecryptor(key, iv))
                using (CryptoStream cryptoStream = new CryptoStream(inputStream, decryptor, CryptoStreamMode.Read))
                {
                    cryptoStream.CopyTo(outputStream);
                    decrypted = outputStream.ToArray();
                }
            }
            catch (Exception)
            {
                throw;
            }

            return decrypted;
        }

        async public static Task<byte[]> DecryptAsync(byte[] data, byte[] key, byte[] iv)
        {
            byte[] decrypted = null;

            try
            {
                using (MemoryStream outputStream = new MemoryStream())
                using (MemoryStream inputStream = new MemoryStream(data))
                using (Aes algorithm = Aes.Create())
                using (ICryptoTransform decryptor = algorithm.CreateDecryptor(key, iv))
                using (CryptoStream cryptoStream = new CryptoStream(inputStream, decryptor, CryptoStreamMode.Read))
                {
                    await cryptoStream.CopyToAsync(outputStream).ConfigureAwait(false);
                    decrypted = outputStream.ToArray();
                }
            }
            catch (Exception)
            {
                throw;
            }

            return decrypted;
        }

        public static byte[] EncryptWithCompress(byte[] data, byte[] key, byte[] iv)
        {
            byte[] encrypted = null;

            try
            {
                MemoryStream memoryStream = new MemoryStream();

                using (Aes algorithm = Aes.Create())
                using (ICryptoTransform encryptor = algorithm.CreateEncryptor(key, iv))
                using (CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                using (DeflateStream deflateStream = new DeflateStream(cryptoStream, CompressionMode.Compress))
                {
                    deflateStream.Write(data, 0, data.Length);
                }

                encrypted = memoryStream.ToArray();
            }
            catch (Exception)
            {
                throw;
            }

            return encrypted;
        }

        public static byte[] DecryptWithDecompress(byte[] data, byte[] key, byte[] iv)
        {
            byte[] decrypted = null;

            try
            {
                using (MemoryStream outputStream = new MemoryStream())
                using (MemoryStream inputStream = new MemoryStream(data))
                using (Aes algorithm = Aes.Create())
                using (ICryptoTransform decryptor = algorithm.CreateDecryptor(key, iv))
                using (CryptoStream cryptoStream = new CryptoStream(inputStream, decryptor, CryptoStreamMode.Read))
                using (DeflateStream deflateStream = new DeflateStream(cryptoStream, CompressionMode.Decompress))
                {
                    deflateStream.CopyTo(outputStream);
                    decrypted = outputStream.ToArray();
                }
            }
            catch (Exception)
            {
                throw;
            }

            return decrypted;
        }

        async public static Task<byte[]> EncryptWithCompressAsync(byte[] data, byte[] key, byte[] iv)
        {
            byte[] encrypted = null; 

            try
            {
                MemoryStream memoryStream = new MemoryStream();

                using (Aes algorithm = Aes.Create())
                using (ICryptoTransform encryptor = algorithm.CreateEncryptor(key, iv))
                using (CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                using (DeflateStream deflateStream = new DeflateStream(cryptoStream, CompressionMode.Compress))
                {
                    await deflateStream.WriteAsync(data, 0, data.Length).ConfigureAwait(false);
                }

                encrypted = memoryStream.ToArray();
            }
            catch (Exception)
            {
                throw;
            }

            return encrypted;
        }

        async public static Task<byte[]> DecryptWithDecompressAsync(byte[] data, byte[] key, byte[] iv)
        {
            byte[] decrypted = null;

            try
            {
                using (MemoryStream outputStream = new MemoryStream())
                using (MemoryStream inputStream = new MemoryStream(data))
                using (Aes algorithm = Aes.Create())
                using (ICryptoTransform decryptor = algorithm.CreateDecryptor(key, iv))
                using (CryptoStream cryptoStream = new CryptoStream(inputStream, decryptor, CryptoStreamMode.Read))
                using (DeflateStream deflateStream = new DeflateStream(cryptoStream, CompressionMode.Decompress))
                {
                    await deflateStream.CopyToAsync(outputStream).ConfigureAwait(false);
                    decrypted = outputStream.ToArray();
                }
            }
            catch (Exception)
            {
                throw;
            }

            return decrypted;
        }
    }
}

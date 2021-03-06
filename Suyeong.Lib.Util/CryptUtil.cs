﻿using System;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Security.Cryptography;
using System.Text;

namespace Suyeong.Lib.Util
{
    public static class CryptUtil
    {
        public static string GetChecksumByMD5(byte[] file)
        {
            string result = string.Empty;
            
            try
            {
                using (MD5 md5 = MD5.Create())
                {
                    byte[] hash = md5.ComputeHash(buffer: file);

                    StringBuilder sb = new StringBuilder();

                    foreach (byte b in hash)
                    {
                        // 16진수로 변환
                        sb.Append(value: b.ToString("X2", CultureInfo.InvariantCulture));
                    }

                    result = sb.ToString();
                }
            }
            catch (Exception)
            {
                throw;
            }

            return result;
        }

        public static string CryptText(string text, byte[] key, byte[] iv)
        {
            string result = string.Empty;

            try
            {

                MemoryStream memoryStream = new MemoryStream();

                using (Aes algorithm = Aes.Create())
                using (ICryptoTransform encryptor = algorithm.CreateEncryptor(key, iv))
                using (CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                using (DeflateStream deflateStream = new DeflateStream(cryptoStream, CompressionMode.Compress))
                {
                    byte[] data = StreamUtil.SerializeObject(text);
                    deflateStream.Write(data, 0, data.Length);
                }

                result = string.Join("", memoryStream.ToArray());
            }
            catch (Exception)
            {
                throw;
            }

            return result;
        }
    }
}

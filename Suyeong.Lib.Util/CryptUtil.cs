using System.IO;
using System.IO.Compression;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Suyeong.Lib.Util
{
    public static class CryptUtil
    {
        public static string GetChecksumByMD5(byte[] file)
        {
            MD5 md5 = MD5.Create();

            byte[] hash = md5.ComputeHash(file);

            StringBuilder sb = new StringBuilder();

            foreach (byte b in hash)
            {
                // 16진수로 변환
                sb.Append(b.ToString("X2"));
            }

            return sb.ToString();
        }
    }
}

using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace Suyeong.Lib.Net.Ftp
{
    public static class FtpStream
    {
        public static byte[] FtpDownload(string ftpPath, string user, string password)
        {
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(ftpPath);
            request.Method = WebRequestMethods.Ftp.DownloadFile;
            request.Credentials = new NetworkCredential(user, password);

            using (FtpWebResponse response = (FtpWebResponse)request.GetResponse())
            using (Stream stream = response.GetResponseStream())
            using (MemoryStream memoryStream = new MemoryStream())
            {
                stream.CopyTo(memoryStream);
                return memoryStream.ToArray();
            }
        }

        public static string FtpUpload(byte[] data, string ftpPath, string user, string password)
        {
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(ftpPath);
            request.Method = WebRequestMethods.Ftp.UploadFile;
            request.Credentials = new NetworkCredential(user, password);
            request.ContentLength = data.Length;

            using (Stream stream = request.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);

                using (FtpWebResponse response = (FtpWebResponse)request.GetResponse())
                {
                    return response.StatusDescription;
                }
            }
        }

        async public static Task<byte[]> FtpDownloadAsync(string ftpPath, string user, string password)
        {
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(ftpPath);
            request.Method = WebRequestMethods.Ftp.DownloadFile;
            request.Credentials = new NetworkCredential(user, password);

            using (FtpWebResponse response = (FtpWebResponse)request.GetResponse())
            using (Stream stream = response.GetResponseStream())
            using (MemoryStream memoryStream = new MemoryStream())
            {
                await stream.CopyToAsync(memoryStream);
                return memoryStream.ToArray();
            }
        }

        async public static Task<string> FtpUploadAsync(byte[] data, string ftpPath, string user, string password)
        {
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(ftpPath);
            request.Method = WebRequestMethods.Ftp.UploadFile;
            request.Credentials = new NetworkCredential(user, password);
            request.ContentLength = data.Length;

            using (Stream stream = await request.GetRequestStreamAsync())
            {
                await stream.WriteAsync(data, 0, data.Length);

                using (FtpWebResponse response = (FtpWebResponse)request.GetResponse())
                {
                    return response.StatusDescription;
                }
            }
        }
    }
}

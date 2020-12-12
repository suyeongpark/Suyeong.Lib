using System;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;

namespace Suyeong.Lib.Util
{
    public static class SystemUtil
    {
        const long KILO_BYTE = 1024;
        const long MEGA_BYTE = 1048576;
        const long GIGA_BYTE = 1073741824;

        public static bool ReleaseComObjects(object[] objects)
        {
            bool result = false;

            try
            {
                foreach (object obj in objects)
                {
                    if (obj != null)
                    {
                        Marshal.ReleaseComObject(obj);
                    }
                }

                result = true;
            }
            catch (Exception)
            {
                throw;
            }

            return result;
        }

        public static string[] GetColumnNamesFromDataTable(DataTable table)
        {
            string[] names = null;

            try
            {
                names = new string[table.Columns.Count];

                for (int i = 0; i < table.Columns.Count; i++)
                {
                    names[i] = table.Columns[i].ColumnName;
                }
            }
            catch (Exception)
            {
                throw;
            }

            return names;
        }

        public static string AddNameToFilePath(string filePath, string add)
        {
            string dirName = Path.GetDirectoryName(filePath);
            string fileName = Path.GetFileNameWithoutExtension(filePath);
            string extension = Path.GetExtension(filePath);

            return $@"{dirName}\{fileName}_{add}{extension}";
        }

        public static string GetFilePathAddNumber(string filePath, int count = 0)
        {
            string copyPath = count == 0 ? filePath : AddNameToFilePath(filePath, count.ToString(CultureInfo.InvariantCulture));
            return !File.Exists(copyPath) ? copyPath : GetFilePathAddNumber(filePath, count + 1);
        }

        public static string GetFileSizeUnit(long fileSize)
        {
            if (fileSize > GIGA_BYTE)
            {
                return $"{(fileSize / GIGA_BYTE).ToString("###.##", CultureInfo.InvariantCulture)} GB";
            }
            else if (fileSize > MEGA_BYTE)
            {
                return $"{(fileSize / MEGA_BYTE).ToString("###.##", CultureInfo.InvariantCulture)} MB";
            }
            else if (fileSize > KILO_BYTE)
            {
                return $"{(fileSize / KILO_BYTE).ToString("###.##", CultureInfo.InvariantCulture)} KB";
            }
            else
            {
                return $"{fileSize} Bytes";
            }
        }

        public static Process FindProcess(string processName, string title)
        {
            foreach (Process process in Process.GetProcessesByName(processName))
            {
                if (string.Equals(process.MainWindowTitle, title, StringComparison.InvariantCulture))
                {
                    return process;
                }
            }

            return null;
        }

        public static Process StartProcess(string path)
        {
            try
            {
                ProcessStartInfo psi = new ProcessStartInfo(path);
                psi.UseShellExecute = false;
                psi.CreateNoWindow = false;

                return Process.Start(psi);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}

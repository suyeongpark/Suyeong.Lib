﻿using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Runtime.InteropServices;

namespace Suyeong.Lib.Util
{
    public static class Utils
    {
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

        public static K GetValueFromDictionary<T, K>(Dictionary<T, K> dic, T key)
        {
            K value;

            if (!dic.TryGetValue(key, out value))
            {
                // error
            }

            return value;
        }

        public static string[] GetColNamesFromDataTable(DataTable table)
        {
            string[] names = new string[table.Columns.Count];

            for (int i = 0; i < table.Columns.Count; i++)
            {
                names[i] = table.Columns[i].ColumnName;
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

        public static string GetFileSizeUnit(long fileSize)
        {
            if (fileSize > Numbers.GIGA_BYTE)
            {
                return $"{(fileSize / Numbers.GIGA_BYTE).ToString("###.##")} GB";
            }
            else if (fileSize > Numbers.MEGA_BYTE)
            {
                return $"{(fileSize / Numbers.MEGA_BYTE).ToString("###.##")} MB";
            }
            else if (fileSize > Numbers.KILO_BYTE)
            {
                return $"{(fileSize / Numbers.KILO_BYTE).ToString("###.##")} KB";
            }
            else
            {
                return $"{fileSize} Bytes";
            }
        }

        public static bool CreateFolder(string path)
        {
            bool result = false;

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            return result;
        }

        public static bool RemoveFolder(string path)
        {
            bool result = false;

            try
            {
                DirectoryInfo directoryInfo = new DirectoryInfo(path);

                foreach (FileInfo fileInfo in directoryInfo.GetFiles())
                {
                    fileInfo.Delete();
                }

                directoryInfo.Delete();

                result = true;
            }
            catch (Exception ex)
            {

            }

            return result;
        }
    }
}

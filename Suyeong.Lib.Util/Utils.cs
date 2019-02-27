using System;
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
            return string.Format(@"{0}\{1}_{2}{3}", Path.GetDirectoryName(filePath), Path.GetFileNameWithoutExtension(filePath), add, Path.GetExtension(filePath));
        }
    }
}

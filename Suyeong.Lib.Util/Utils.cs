using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace Suyeong.Lib.Util
{
    public static class Utils
    {
        public static string GetOrdinalIndicator(int num)
        {
            int rest = num % 100;

            if (rest == 11 || rest == 12 || rest == 13)
            {
                return "th";
            }

            switch (rest % 10)
            {
                case 1:
                    return "st";

                case 2:
                    return "nd";

                case 3:
                    return "rd";

                default:
                    return "th";
            }
        }

        public static int GetTimeSpanHHMMSS(TimeSpan timeSpan)
        {
            int second = timeSpan.Seconds;
            int minute = timeSpan.Minutes * 100;
            int hour = timeSpan.Hours * 10000;

            return second + minute + hour;
        }

        public static int FigureMonths(DateTime start, DateTime end)
        {
            return (end.Year - start.Year) * 12 + (end.Month - start.Month);
        }

        public static float ConvertMinuteToTimeStr(int minute)
        {
            int hour = minute / 60;
            float min = (minute % 60) * 0.01f;

            return hour + min;
        }

        public static int FigureMinuteBySecond(int second)
        {
            return (second % 3600) / 60;
        }

        public static int FigureHourBySecond(int second)
        {
            return second / 3600;
        }

        public static float FigureRound(float Num, float Den)
        {
            return IsZero(Den) ? 0f : (float)Math.Round((double)Num / (double)Den) * 1000f * 0.001f;
        }

        public static string[] GetColNamesFromDataTable(DataTable dataTable)
        {
            string[] colNames = new string[dataTable.Columns.Count];

            for (int i = 0; i < dataTable.Columns.Count; i++)
            {
                colNames[i] = dataTable.Columns[i].ColumnName;
            }

            return colNames;
        }

        public static double ConvertExponentialNumber(string value)
        {
            double result = 0d;

            if (!double.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out result))
            {
                // error!
            }

            return result;
        }

        public static bool IsEqual(float val1, float val2)
        {
            return IsZero(num: val1 - val2);
        }

        public static bool IsEqual(double val1, double val2)
        {
            return IsZero(num: val1 - val2);
        }

        public static bool IsZero(float num)
        {
            return (num < float.Epsilon && num > -float.Epsilon);
        }

        public static bool IsZero(double num)
        {
            return (num < double.Epsilon && num > -double.Epsilon);
        }

        public static int GetIntFromString(string str)
        {
            int num = 0;

            if (!int.TryParse(str, out num))
            {
                // error!
            }

            return num;
        }

        public static float GetFloatFromString(string str)
        {
            float num = 0f;

            if (!float.TryParse(str, out num))
            {
                // error!
            }

            return !float.IsNaN(num) ? num : 0f;
        }

        public static double GetDoubleFromString(string str)
        {
            double num = 0d;

            if (!double.TryParse(str, out num))
            {
                // error!
            }

            return !double.IsNaN(num) ? num : 0d;
        }

        public static decimal GetDecimalFromString(string str)
        {
            decimal num = 0;

            if (!decimal.TryParse(str, out num))
            {
                // error!
            }

            return num;
        }

        public static DateTime GetDateFromString(string str)
        {
            DateTime dateTime = new DateTime();

            if (!DateTime.TryParse(str, out dateTime))
            {
                // error!
            }

            return dateTime;
        }

        public static K GetDataFromDictionary<T, K>(Dictionary<T, K> dic, T key)
        {
            K value;

            if (!dic.TryGetValue(key, out value))
            {
                // error
            }

            return value;
        }

        public static byte[] ObjectToBinary(object data)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                binaryFormatter.Serialize(memoryStream, data);

                return memoryStream.ToArray();
            }
        }

        public static object BinaryToObject(byte[] data)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                memoryStream.Write(data, 0, data.Length);
                memoryStream.Position = 0;

                BinaryFormatter binaryFormatter = new BinaryFormatter();
                return binaryFormatter.Deserialize(memoryStream);
            }
        }

        static DataTable GetDataTableFromSeparate(string path, char[] separate, bool hasHeader, Encoding encoding)
        {
            DataTable dataTable = new DataTable();

            if (File.Exists(path))
            {
                using (StreamReader rd = new StreamReader(path, encoding))
                {
                    string[] fieldData;
                    string line;

                    fieldData = rd.ReadLine().Split(separate);

                    if (hasHeader)
                    {
                        for (int i = 0; i < fieldData.Length; i++)
                        {
                            DataColumn datecolumn = new DataColumn(fieldData[i]);
                            datecolumn.AllowDBNull = true;
                            dataTable.Columns.Add(datecolumn);
                        }
                    }
                    else
                    {
                        // header가 없었다면 첫 행을 바로 row로 넣는다.
                        for (int i = 0; i < fieldData.Length; i++)
                        {
                            DataColumn datecolumn = new DataColumn();
                            datecolumn.AllowDBNull = true;
                            dataTable.Columns.Add(datecolumn);

                            if (string.IsNullOrWhiteSpace(fieldData[i]))
                            {
                                fieldData[i] = null;
                            }
                        }

                        dataTable.Rows.Add(fieldData);
                    }

                    while (!rd.EndOfStream)
                    {
                        line = rd.ReadLine();

                        if (line.Length > 0)
                        {
                            fieldData = line.Split(separate);

                            //Making empty value as null
                            for (int i = 0; i < fieldData.Length; i++)
                            {
                                if (string.IsNullOrWhiteSpace(fieldData[i]))
                                {
                                    fieldData[i] = null;
                                }
                            }

                            dataTable.Rows.Add(fieldData);
                        }
                    }
                }
            }

            return dataTable;
        }


        static string MakeSeparateValueFromDataTable(DataTable dataTable, string separate, bool hasTitle)
        {
            StringBuilder csv = new StringBuilder();

            if (hasTitle)
            {
                string[] titles = GetColNamesFromDataTable(dataTable: dataTable);
                csv.AppendLine(string.Join(separate, titles));
            }

            string[] values;

            foreach (DataRow row in dataTable.Rows)
            {
                values = row.ItemArray.Select(x => x.ToString()).ToArray();
                csv.AppendLine(string.Join(separate, values));
            }

            return csv.ToString();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
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
            int num = -1;

            if (!int.TryParse(str, out num))
            {
                // error!
            }

            return num;
        }

        public static float GetFloatFromString(string str)
        {
            float num = -1f;

            if (!float.TryParse(str, out num))
            {
                // error!
            }

            return !float.IsNaN(num) ? num : -1f;
        }

        public static double GetDoubleFromString(string str)
        {
            double num = -1d;

            if (!double.TryParse(str, out num))
            {
                // error!
            }

            return !double.IsNaN(num) ? num : -1d;
        }

        public static decimal GetDecimalFromString(string str)
        {
            decimal num = -1;

            if (!decimal.TryParse(str, out num))
            {
                // error!
            }

            return num;
        }

        public static DateTime GetDateTimeFromString(string str)
        {
            DateTime dateTime = new DateTime();

            if (!DateTime.TryParse(str, out dateTime))
            {
                // error!
            }

            return dateTime;
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

        public static byte[] SerializeObject(object data)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                binaryFormatter.Serialize(memoryStream, data);

                return memoryStream.ToArray();
            }
        }

        public static object DeserializeObject(byte[] data)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                memoryStream.Write(data, 0, data.Length);
                memoryStream.Position = 0;

                BinaryFormatter binaryFormatter = new BinaryFormatter();
                return binaryFormatter.Deserialize(memoryStream);
            }
        }

        public static string CutWordByByteCount(string text, int byteCount)
        {
            Encoding encoding = Encoding.GetEncoding("ks_c_5601-1987");
            byte[] buf = encoding.GetBytes(text);
            return encoding.GetString(buf, 0, byteCount);
        }

        public static string ConvertStringASCII(string text)
        {
            return Encoding.ASCII.GetString(Encoding.Default.GetBytes(text));
        }

        public static string ConvertStringDefault(string text)
        {
            return Encoding.Default.GetString(Encoding.Default.GetBytes(text));
        }

        public static string ConvertStringUTF8(string text)
        {
            return Encoding.UTF8.GetString(Encoding.Default.GetBytes(text));
        }

        public static string ConvertStringUnicode(string text)
        {
            return Encoding.Unicode.GetString(Encoding.Default.GetBytes(text));
        }

        public static bool IsContainTextByIgnoreCase(string source, string text)
        {
            return source.IndexOf(text, StringComparison.OrdinalIgnoreCase) > -1;
        }

        public static string AddNameToFilePath(string filePath, string add)
        {
            return string.Format(@"{0}\{1}_{2}{3}", Path.GetDirectoryName(filePath), Path.GetFileNameWithoutExtension(filePath), add, Path.GetExtension(filePath));
        }

        public static DataTable GetDataTableFromCSV(string filePath, bool hasHeader = true, Encoding encoding = default(Encoding))
        {
            return GetDataTableFromSeparateFile(filePath: filePath, separate: new char[] { ',' }, hasHeader: hasHeader, encoding: encoding);
        }

        public static DataTable GetDataTableFromTSV(string filePath, bool hasHeader = true, Encoding encoding = default(Encoding))
        {
            return GetDataTableFromSeparateFile(filePath: filePath, separate: new char[] { '\t' }, hasHeader: hasHeader, encoding: encoding);
        }

        public static string GetCsvFromDataSet(DataSet dataSet)
        {
            return GetSeparateValueFromDataSet(dataSet: dataSet, separate: ",");
        }

        public static string GetTsvFromDataSet(DataSet dataSet)
        {
            return GetSeparateValueFromDataSet(dataSet: dataSet, separate: "\t");
        }

        public static string GetCsvFromDataTable(DataTable dataTable)
        {
            return GetSeparateValueFromDataTable(dataTable: dataTable, separate: ",");
        }

        public static string GetTsvFromDataTable(DataTable dataTable)
        {
            return GetSeparateValueFromDataTable(dataTable: dataTable, separate: "\t");
        }

        public static string GetCsvFromList<T>(IEnumerable<T> dataList)
        {
            return GetSeparateValueFromList(list: dataList, separate: ",");
        }

        public static string GetTsvFromList<T>(IEnumerable<T> dataList)
        {
            return GetSeparateValueFromList(list: dataList, separate: "\t");
        }

        public static string ConvertToPascalCase(string text)
        {
            if (!string.IsNullOrWhiteSpace(text))
            {
                string[] words = text.Split(new char[] { ' ', }, StringSplitOptions.RemoveEmptyEntries);
                string[] result = new string[words.Length];

                for (int i = 0; i < words.Length; i++)
                {
                    result[i] = words[i][0].ToString().ToUpper() + words[i].Substring(1);
                }

                return string.Join(" ", result);
            }
            else
            {
                return string.Empty;
            }
        }

        static DataTable GetDataTableFromSeparateFile(string filePath, char[] separate, bool hasHeader, Encoding encoding)
        {
            DataTable table = new DataTable();

            if (File.Exists(filePath))
            {
                try
                {
                    string[] values;
                    int index = 0;

                    foreach (string line in File.ReadLines(filePath, encoding))
                    {
                        values = line.Split(separate);

                        // 첫 행처리
                        if (index == 0)
                        {
                            if (hasHeader)
                            {
                                foreach (string value in values)
                                {
                                    table.Columns.Add(new DataColumn(value) { AllowDBNull = true });
                                }
                            }
                            else
                            {
                                // header가 없었다면 컬럼에 빈 데이터를 넣고 첫 행을 바로 row로 넣는다.
                                foreach (string value in values)
                                {
                                    table.Columns.Add(new DataColumn() { AllowDBNull = true });
                                }

                                table.Rows.Add(values);
                            }

                            index++;
                        }
                        else
                        {
                            table.Rows.Add(values);
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw;
                }
            }

            return table;
        }

        static DataTable GetDataTableFromList<T>(IEnumerable<T> list)
        {
            DataTable table = new DataTable();

            FieldInfo[] fieldInfos = typeof(T).GetFields();
            PropertyInfo[] propertyInfos = typeof(T).GetProperties();

            List<string> columns = fieldInfos.Select(field => field.Name).ToList();
            columns.AddRange(propertyInfos.Select(property => property.Name).ToList());

            foreach (string column in columns)
            {
                table.Columns.Add(new DataColumn(column) { AllowDBNull = true });
            }

            List<object> values;

            foreach (T row in list)
            {
                values = fieldInfos.Select(field => field.GetValue(row)).ToList();
                values.AddRange(propertyInfos.Select(property => property.GetValue(row)));
                table.Rows.Add(values);
            }

            return table;
        }

        static DataTable GetDataTableFromListFieldOnly<T>(IEnumerable<T> list)
        {
            DataTable table = new DataTable();

            FieldInfo[] fieldInfos = typeof(T).GetFields();

            List<string> columns = fieldInfos.Select(field => field.Name).ToList();

            foreach (string column in columns)
            {
                table.Columns.Add(new DataColumn(column) { AllowDBNull = true });
            }

            foreach (T row in list)
            {
                table.Rows.Add(fieldInfos.Select(field => field.GetValue(row)));
            }

            return table;
        }

        static DataTable GetDataTableFromListPropertyOnly<T>(IEnumerable<T> list)
        {
            DataTable table = new DataTable();

            PropertyInfo[] propertyInfos = typeof(T).GetProperties();

            List<string> columns = propertyInfos.Select(property => property.Name).ToList();

            foreach (string column in columns)
            {
                table.Columns.Add(new DataColumn(column) { AllowDBNull = true });
            }

            foreach (T row in list)
            {
                table.Rows.Add(propertyInfos.Select(property => property.GetValue(row)));
            }

            return table;
        }


        static string GetSeparateValueFromDataSet(DataSet dataSet, string separate)
        {
            StringBuilder sb = new StringBuilder();

            foreach (DataTable table in dataSet.Tables)
            {
                sb.AppendLine(GetSeparateValueFromDataTable(dataTable: table, separate: separate));
                sb.AppendLine("");  // table 사이를 구분하기 위한 공백
            }

            return sb.ToString();
        }


        static string GetSeparateValueFromDataTable(DataTable dataTable, string separate)
        {
            StringBuilder sb = new StringBuilder();

            string title = string.Join(separate, GetColNamesFromDataTable(table: dataTable));

            if (!string.IsNullOrWhiteSpace(title))
            {
                sb.AppendLine(title);
            }

            foreach (DataRow row in dataTable.Rows)
            {
                sb.AppendLine(string.Join(separate, row.ItemArray));
            }

            return sb.ToString();
        }

        static string GetSeparateValueFromList<T>(IEnumerable<T> list, string separate)
        {
            StringBuilder sb = new StringBuilder();

            FieldInfo[] fieldInfos = typeof(T).GetFields();
            PropertyInfo[] propertyInfos = typeof(T).GetProperties();

            List<string> titles = fieldInfos.Select(field => field.Name).ToList();
            titles.AddRange(propertyInfos.Select(property => property.Name).ToList());
            sb.AppendLine(string.Join(separate, titles));

            List<object> values;

            foreach (T row in list)
            {
                values = fieldInfos.Select(field => field.GetValue(row)).ToList();
                values.AddRange(propertyInfos.Select(property => property.GetValue(row)));
                sb.AppendLine(string.Join(separate, values));
            }

            return sb.ToString();
        }

        static string GetSeparateValueFromListFieldOnly<T>(IEnumerable<T> list, string separate)
        {
            StringBuilder sb = new StringBuilder();

            FieldInfo[] fieldInfos = typeof(T).GetFields();

            string title = string.Join(separate, fieldInfos.Select(field => field.Name));
            sb.AppendLine(title);

            foreach (T row in list)
            {
                sb.AppendLine(string.Join(separate, fieldInfos.Select(field => field.GetValue(row))));
            }

            return sb.ToString();
        }

        static string GetSeparateValueFromListPropertyOnly<T>(IEnumerable<T> list, string separate)
        {
            StringBuilder sb = new StringBuilder();

            PropertyInfo[] propertyInfos = typeof(T).GetProperties();

            string title = string.Join(separate, propertyInfos.Select(property => property.Name));
            sb.AppendLine(title);

            foreach (T row in list)
            {
                sb.AppendLine(string.Join(separate, propertyInfos.Select(property => property.GetValue(row))));
            }

            return sb.ToString();
        }
    }
}

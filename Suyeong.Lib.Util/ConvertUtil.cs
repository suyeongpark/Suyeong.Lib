﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Suyeong.Lib.Util
{
    public static class ConvertUtil
    {
        public static double PixelToPoint(double dpi, double pointPerInch, double value)
        {
            return value * pointPerInch / dpi;
        }

        public static double PointToPixel(double dpi, double pointPerInch, double value)
        {
            return value * dpi / pointPerInch;
        }

        public static double ExponentialStringToDouble(string value, double valueOfFailure = -1d)
        {
            double result = valueOfFailure;

            if (!double.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out result))
            {
                // error!
            }

            return result;
        }

        public static float ExponentialStringToFloat(string value, float valueOfFailure = -1f)
        {
            float result = valueOfFailure;

            if (!float.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out result))
            {
                // error!
            }

            return result;
        }

        public static int StringToInt(string str, int valueOfFailure = -1)
        {
            int num = valueOfFailure;

            if (!int.TryParse(str, out num))
            {
                // error!
            }

            return num;
        }

        public static uint StringToUint(string str, uint valueOfFailure = 0)
        {
            uint num = valueOfFailure;

            if (!uint.TryParse(str, out num))
            {
                // error!
            }

            return num;
        }

        public static long StringToLong(string str, long valueOfFailure = -1)
        {
            long num = valueOfFailure;

            if (!long.TryParse(str, out num))
            {
                // error!
            }

            return num;
        }

        public static ulong StringToUlong(string str, ulong valueOfFailure = 0)
        {
            ulong num = valueOfFailure;

            if (!ulong.TryParse(str, out num))
            {
                // error!
            }

            return num;
        }

        public static float StringToFloat(string str, float valueOfFailure = -1f)
        {
            float num = valueOfFailure;

            if (!float.TryParse(str, out num))
            {
                // error!
            }

            return !float.IsNaN(num) ? num : valueOfFailure;
        }

        public static double StringToDouble(string str, double valueOfFailure = -1d)
        {
            double num = valueOfFailure;

            if (!double.TryParse(str, out num))
            {
                // error!
            }

            return !double.IsNaN(num) ? num : valueOfFailure;
        }

        public static decimal StringToDecimal(string str, decimal valueOfFailure = -1)
        {
            decimal num = valueOfFailure;

            if (!decimal.TryParse(str, out num))
            {
                // error!
            }

            return num;
        }

        public static DateTime StringToDateTime(string str)
        {
            DateTime dateTime = DateTime.MinValue;

            if (!DateTime.TryParse(str, out dateTime))
            {
                // error!
            }

            return dateTime;
        }

        public static DataTable CsvToDataTable(string filePath, bool hasHeader = true, Encoding encoding = default(Encoding))
        {
            return SeparateToDataTable(filePath: filePath, separate: new char[] { ',' }, hasHeader: hasHeader, encoding: encoding);
        }

        public static DataTable TsvToDataTable(string filePath, bool hasHeader = true, Encoding encoding = default(Encoding))
        {
            return SeparateToDataTable(filePath: filePath, separate: new char[] { '\t' }, hasHeader: hasHeader, encoding: encoding);
        }

        public static string DataSetToCsv(DataSet dataSet)
        {
            return DataSetToSeparateValue(dataSet: dataSet, separate: ",");
        }

        public static string DataSetToTsv(DataSet dataSet)
        {
            return DataSetToSeparateValue(dataSet: dataSet, separate: "\t");
        }

        public static string DataTableToCsv(DataTable table)
        {
            return DataTableToSeparateValue(table: table, separate: ",");
        }

        public static string DataTableToTsv(DataTable table)
        {
            return DataTableToSeparateValue(table: table, separate: "\t");
        }

        public static string ListToCsv<T>(IEnumerable<T> dataList)
        {
            return ListToSeparateValue(list: dataList, separate: ",");
        }

        public static string ListToTsv<T>(IEnumerable<T> dataList)
        {
            return ListToSeparateValue(list: dataList, separate: "\t");
        }

        public static DataTable ListToDataTable<T>(IEnumerable<T> list)
        {
            DataTable table = new DataTable();

            try
            {
                FieldInfo[] fieldInfos = typeof(T).GetFields();
                PropertyInfo[] propertyInfos = typeof(T).GetProperties();

                List<string> columns = new List<string>();
                columns.AddRange(fieldInfos.Select(field => field.Name));
                columns.AddRange(propertyInfos.Select(property => property.Name));

                foreach (string column in columns)
                {
                    table.Columns.Add(new DataColumn(column) { AllowDBNull = true });
                }

                List<object> values;

                foreach (T row in list)
                {
                    values = fieldInfos.Select(field => field.GetValue(row)).ToList();
                    values.AddRange(propertyInfos.Select(property => property.GetValue(row)));
                    table.Rows.Add(values.ToArray());
                }
            }
            catch (Exception)
            {
                throw;
            }

            return table;
        }

        public static DataTable ListToDataTableFieldOnly<T>(IEnumerable<T> list)
        {
            DataTable table = new DataTable();

            try
            {
                FieldInfo[] fieldInfos = typeof(T).GetFields();

                List<string> columns = fieldInfos.Select(field => field.Name).ToList();

                foreach (string column in columns)
                {
                    table.Columns.Add(new DataColumn(column) { AllowDBNull = true });
                }

                foreach (T row in list)
                {
                    table.Rows.Add(fieldInfos.Select(field => field.GetValue(row)).ToArray());
                }
            }
            catch (Exception)
            {
                throw;
            }

            return table;
        }

        public static DataTable ListToDataTablePropertyOnly<T>(IEnumerable<T> list)
        {
            if (list == null)
            {
                throw new NullReferenceException();
            }

            DataTable table = new DataTable();

            PropertyInfo[] propertyInfos = typeof(T).GetProperties();

            List<string> columns = propertyInfos.Select(property => property.Name).ToList();

            foreach (string column in columns)
            {
                table.Columns.Add(new DataColumn(column) { AllowDBNull = true });
            }

            foreach (T row in list)
            {
                table.Rows.Add(propertyInfos.Select(property => property.GetValue(row)).ToArray());
            }

            return table;
        }

        static DataTable SeparateToDataTable(string filePath, char[] separate, bool hasHeader, Encoding encoding)
        {
            DataTable table = new DataTable();

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
            catch (Exception)
            {
                throw;
            }

            return table;
        }


        static string DataSetToSeparateValue(DataSet dataSet, string separate)
        {
            StringBuilder sb = new StringBuilder();

            foreach (DataTable table in dataSet.Tables)
            {
                sb.AppendLine(DataTableToSeparateValue(table: table, separate: separate));
                sb.AppendLine("");  // table 사이를 구분하기 위한 공백
            }

            return sb.ToString();
        }

        static string DataTableToSeparateValue(DataTable table, string separate)
        {
            StringBuilder sb = new StringBuilder();

            try
            {
                string title = string.Join(separate, SystemUtil.GetColumnNamesFromDataTable(table: table));

                if (!string.IsNullOrWhiteSpace(title))
                {
                    sb.AppendLine(title);
                }

                foreach (DataRow row in table.Rows)
                {
                    sb.AppendLine(string.Join(separate, row.ItemArray));
                }
            }
            catch (Exception)
            {
                throw;
            }

            return sb.ToString();
        }

        static string ListToSeparateValue<T>(IEnumerable<T> list, string separate)
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

        static string ListToSeparateValueFieldOnly<T>(IEnumerable<T> list, string separate)
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

        static string ListToSeparateValuePropertyOnly<T>(IEnumerable<T> list, string separate)
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

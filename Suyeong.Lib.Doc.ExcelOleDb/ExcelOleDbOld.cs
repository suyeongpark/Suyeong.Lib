using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Text;

namespace Suyeong.Lib.Doc.ExcelOleDb
{
    public static class ExcelOleDbOld
    {
        public static DataSet LoadByDataSet(string filePath, IEnumerable<string> sheetNames, bool hasTitle = true)
        {
            if (sheetNames == null)
            {
                throw new NullReferenceException();
            }

            DataSet dataSet = new DataSet();

            try
            {
                string conStr = GetConStr(filePath: filePath, hasTitle: hasTitle);

                using (OleDbConnection connection = new OleDbConnection(conStr))
                {
                    connection.Open();

                    foreach (string sheetName in sheetNames)
                    {
                        dataSet.Tables.Add(GetSheet(connection: connection, sheetName: sheetName));
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }

            return dataSet;
        }

        public static DataTable LoadByDataTable(string filePath, string sheetName, bool hasTitle = true)
        {
            DataTable table = new DataTable();

            try
            {
                string conStr = GetConStr(filePath: filePath, hasTitle: hasTitle);
                string commandTxt = "Select * From [" + sheetName + "$]";

                using (OleDbConnection connection = new OleDbConnection(conStr))
                {
                    connection.Open();

                    table = GetSheet(connection: connection, sheetName: sheetName);
                }
            }
            catch (Exception)
            {
                throw;
            }

            return table;
        }

        public static bool SaveByDataSet(DataSet dataSet, string filePath, bool hasTitle = true)
        {
            if (dataSet == null)
            {
                throw new NullReferenceException();
            }

            bool result = false;

            try
            {
                string conStr = GetConStr(filePath: filePath, hasTitle: hasTitle);

                foreach (DataTable table in dataSet.Tables)
                {
                    result = SetSheet(table: table, conStr: conStr);
                }
            }
            catch (Exception)
            {
                throw;
            }

            return result;
        }

        public static bool SaveByDataTable(DataTable table, string filePath, bool hasTitle = true)
        {
            if (table == null)
            {
                throw new NullReferenceException();
            }

            bool result = false;

            try
            {
                string conStr = GetConStr(filePath: filePath, hasTitle: hasTitle);

                result = SetSheet(table: table, conStr: conStr);
            }
            catch (Exception)
            {
                throw;
            }

            return result;
        }

        static DataTable GetSheet(OleDbConnection connection, string sheetName)
        {
            DataTable table = new DataTable(sheetName);

            try
            {
                string commandTxt = "Select * From [" + sheetName + "$]";

                using (OleDbCommand command = new OleDbCommand(commandTxt, connection))
                using (OleDbDataAdapter adapter = new OleDbDataAdapter(command))
                {
                    adapter.Fill(table);
                }
            }
            catch (Exception)
            {
                throw;
            }

            return table;
        }

        static bool SetSheet(DataTable table, string conStr)
        {
            bool result = false;

            try
            {
                List<string> columns = GetColumnNames(table: table);
                string tableName = !string.IsNullOrWhiteSpace(table.TableName) ? table.TableName : "Sheet1";

                using (OleDbConnection connection = new OleDbConnection(conStr))
                {
                    connection.Open();

                    using (OleDbCommand command = new OleDbCommand())
                    {
                        command.Connection = connection;

                        // 1. table을 만든다.
                        command.CommandText = $"create table [{tableName}] ({string.Join(" Varchar, ", columns)} Varchar);";
                        command.ExecuteNonQuery();

                        // 2. column name을 만든다.
                        string columnNames = string.Join(", ", columns);

                        // 3. data를 넣는다.
                        foreach (DataRow row in table.Rows)
                        {
                            command.CommandText = $"insert into [{tableName}] ({columnNames}) values ('{GetValues(itemArray: row.ItemArray)}')";
                            command.ExecuteNonQuery();
                        }
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

        static List<string> GetColumnNames(DataTable table)
        {
            List<string> columns = new List<string>();

            foreach (DataColumn column in table.Columns)
            {
                columns.Add(ConvertQuote(text: column.ColumnName));
            }

            return columns;
        }

        static string GetValues(object[] itemArray)
        {
            StringBuilder sb = new StringBuilder();
            string value;

            for (int i = 0; i < itemArray.Length; i++)
            {
                value = (i < itemArray.Length - 1) ? ConvertQuote(text: itemArray[i].ToString()) + "','" : ConvertQuote(text: itemArray[i].ToString());
                sb.Append(value);
            }

            return sb.ToString();
        }

        static string ConvertQuote(string text)
        {
            // ' 제거
            return text.Replace("'", "''");
        }

        static string GetConStr(string filePath, bool hasTitle)
        {
            string extension = Path.GetExtension(filePath).ToLowerInvariant();
            string HDR = hasTitle ? "YES" : "NO";

            // 2007 이상용
            if (string.Equals(extension, ".xlsx", StringComparison.InvariantCulture))
            {
                return $"Provider=Microsoft.ACE.OLEDB.12.0;Extended Properties='Excel 12.0 XML;HDE={HDR};';Data Source={filePath};";
            }
            // 97-2003
            else // .xls 32비트에서만 가능하다.
            {
                return $"Provider=Microsoft.Jet.OLEDB.4.0;Extended Properties='Excel 8.0 XML;HDE={HDR};';Data Source={filePath};";
            }
        }
    }
}

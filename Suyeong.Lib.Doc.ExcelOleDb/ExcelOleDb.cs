using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;

namespace Suyeong.Lib.Doc.ExcelOleDb
{
    public static class ExcelOleDb
    {
        public static DataSet LoadByDataSet(string filePath, IEnumerable<string> sheetNames, bool hasTitle = true)
        {
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
                        command.CommandText = $"create table {tableName} ([{string.Join("] Varchar(255), [", columns)}] Varchar(255))";
                        command.ExecuteNonQuery();

                        // 2. data를 넣는다.
                        string query = $"insert into [{tableName}] ({string.Join(", ", columns)}) values (@{string.Join(", @", columns)})";

                        foreach (DataRow row in table.Rows)
                        {
                            command.CommandText = query;

                            for (int i = 0; i < row.ItemArray.Length; i++)
                            {
                                command.Parameters.Add("@" + columns[i], OleDbType.VarWChar).Value = row[i].ToString();
                            }

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
                // 타이틀에서는 ' 제거
                columns.Add(column.ColumnName.Replace("'", ""));
            }

            return columns;
        }

        static string GetConStr(string filePath, bool hasTitle)
        {
            string extension = Path.GetExtension(filePath).ToLower();
            string HDR = hasTitle ? "YES" : "NO";
            string IMEX = "1";

            // 2007 이상용
            if (string.Equals(extension, ".xlsx"))
            {
                return $"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=\"{filePath}\";Mode=ReadWrite|Share Deny None;Extended Properties='Excel 12.0;HDR={HDR};IMEX={IMEX}';Persist Security Info=False";
            }
            // 97-2003
            else // .xls
            {
                return $"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=\"{filePath}\";Mode=ReadWrite|Share Deny None;Extended Properties='Excel 8.0;HDR={HDR};IMEX={IMEX}';Persist Security Info=False";
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;

namespace Suyeong.Lib.Doc.OleDbExcel
{
    public static class OleExcel
    {
        public static DataSet GetDataSetFromExcel(string filePath, IEnumerable<string> sheetNames, string HDR = "YES", string IMEX = "1")
        {
            DataSet dataSet = new DataSet();

            try
            {
                string conStr = GetExcelConStr(filePath: filePath, HDR: HDR, IMEX: IMEX);

                using (OleDbConnection connection = new OleDbConnection(conStr))
                {
                    connection.Open();

                    foreach (string sheetName in sheetNames)
                    {
                        dataSet.Tables.Add(GetDataTableFromSheet(connection: connection, sheetName: sheetName));
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }

            return dataSet;
        }

        public static DataTable GetDataTableFromExcel(string filePath, string sheetName, string HDR = "YES", string IMEX = "1")
        {
            DataTable table = new DataTable();

            try
            {
                string conStr = GetExcelConStr(filePath: filePath, HDR: HDR, IMEX: IMEX);
                string commandTxt = "Select * From [" + sheetName + "$]";

                using (OleDbConnection connection = new OleDbConnection(conStr))
                {
                    connection.Open();

                    table = GetDataTableFromSheet(connection: connection, sheetName: sheetName);
                }
            }
            catch (Exception ex)
            {
                throw;
            }

            return table;
        }

        public static bool SetExcelFromDataSet(DataSet dataSet, string filePath, string HDR = "YES")
        {
            bool result = false;

            try
            {
                string conStr = GetExcelConStr(filePath: filePath, HDR: HDR, IMEX: "0");

                foreach (DataTable table in dataSet.Tables)
                {
                    result = SetExcelFromDataTable(table: table, conStr: conStr);
                }
            }
            catch (Exception ex)
            {
                throw;
            }

            return result;
        }

        public static bool SetExcelFromDataTable(DataTable table, string filePath, string HDR = "YES")
        {
            bool result = false;

            try
            {
                string conStr = GetExcelConStr(filePath: filePath, HDR: HDR, IMEX: "0");

                result = SetExcelFromDataTable(table: table, conStr: conStr);
            }
            catch (Exception ex)
            {
                throw;
            }

            return result;
        }

        static DataTable GetDataTableFromSheet(OleDbConnection connection, string sheetName)
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
            catch (Exception ex)
            {
                throw;
            }

            return table;
        }

        static bool SetExcelFromDataTable(DataTable table, string conStr)
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
                        string query = $"[{tableName}$] ({string.Join(", ", columns)}) values (@{string.Join(", @", columns)})";

                        foreach (DataRow row in table.Rows)
                        {
                            command.CommandText = $"insert into {query}";

                            for (int i = 0; i < row.ItemArray.Length; i++)
                            {
                                // 실제 데이터에서는 '를 ''로 대체
                                command.Parameters.Add("@" + columns[i], OleDbType.VarWChar).Value = row[i].ToString();
                            }

                            command.ExecuteNonQuery();
                        }
                    }
                }

                result = true;
            }
            catch (Exception ex)
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

        static string GetExcelConStr(string filePath, string HDR, string IMEX)
        {
            string extension = Path.GetExtension(filePath).ToLower();

            // 2007 이상용
            if (string.Equals(extension, ".xlsx"))
            {
                return $"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={filePath};Mode=ReadWrite|Share Deny None;Extended Properties='Excel 12.0;HDR={HDR};IMEX={IMEX}';Persist Security Info=False";
            }
            // 97-2003
            else // .xls
            {
                return $"Provider=Microsoft.Jet.OLEDB.4.0;Data Source={filePath};Mode=ReadWrite|Share Deny None;Extended Properties='Excel 8.0;HDR={HDR};IMEX={IMEX}';Persist Security Info=False";
            }
        }
    }
}

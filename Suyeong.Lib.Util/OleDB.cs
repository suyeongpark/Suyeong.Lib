using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;

namespace Suyeong.Lib.Util
{
    public static class OleDB
    {
        public static DataSet GetDataSetFromExcel(string filePath, string fileName, string HDR = "YES", string IMEX = "1")
        {
            DataSet dataSet = new DataSet(fileName);

            string conStr = GetExcelConStr(filePath: filePath, HDR: HDR, IMEX: IMEX);
            string commandTxt = "Select * From [" + fileName + "$]";

            if (!string.IsNullOrEmpty(conStr))
            {
                using (OleDbConnection connection = new OleDbConnection(conStr))
                using (OleDbCommand command = new OleDbCommand(commandTxt, connection))
                using (OleDbDataAdapter adapter = new OleDbDataAdapter(command))
                {
                    connection.Open();
                    adapter.Fill(dataSet);
                }
            }

            return dataSet;
        }

        public static DataTable GetDataTableFromExcel(string filePath, string sheetName, string HDR = "YES", string IMEX = "1")
        {
            DataTable table = new DataTable(sheetName);

            string conStr = GetExcelConStr(filePath: filePath, HDR: HDR, IMEX: IMEX);
            string commandTxt = "Select * From [" + sheetName + "$]";

            if (!string.IsNullOrEmpty(conStr))
            {
                using (OleDbConnection connection = new OleDbConnection(conStr))
                using (OleDbCommand command = new OleDbCommand(commandTxt, connection))
                using (OleDbDataAdapter adapter = new OleDbDataAdapter(command))
                {
                    connection.Open();
                    adapter.Fill(table);
                }
            }

            return table;
        }

        public static bool SetExcelFromDataSet(DataSet dataSet, string filePath, string HDR = "YES")
        {
            bool result = false;

            string conStr = GetExcelConStr(filePath: filePath, HDR: HDR, IMEX: "0");

            foreach (DataTable table in dataSet.Tables)
            {
                result = SetExcelFrom(table: table, conStr: conStr);
            }

            return result;
        }

        public static bool SetExcelFromDataTable(DataTable table, string filePath, string HDR = "YES")
        {
            string conStr = GetExcelConStr(filePath: filePath, HDR: HDR, IMEX: "0");

            return SetExcelFrom(table: table, conStr: conStr);
        }

        static bool SetExcelFrom(DataTable table, string conStr)
        {
            bool result = false;

            try
            {
                List<string> columns = GetColumnNames(table: table);
                string tableName = !string.IsNullOrWhiteSpace(table.TableName) ? table.TableName : "Sheet1";
                string query = $"[{tableName}$] ({string.Join(", ", columns)}) values (@{string.Join(", @", columns)})";

                using (OleDbConnection connection = new OleDbConnection(conStr))
                {
                    connection.Open();

                    using (OleDbCommand command = new OleDbCommand())
                    {
                        command.Connection = connection;

                        // 1. table을 만든다.
                        command.CommandText = $"create table {query}";

                        foreach (string column in columns)
                        {
                            command.Parameters.Add("@" + column, OleDbType.VarWChar).Value = column;
                        }

                        command.ExecuteNonQuery();


                        // 2. data를 넣는다.
                        foreach (DataRow row in table.Rows)
                        {
                            command.CommandText = $"insert into {query}";

                            for (int i = 0; i < row.ItemArray.Length; i++)
                            {
                                // 실제 데이터에서는 '를 ''로 대체
                                command.Parameters.Add("@" + columns[i], OleDbType.VarWChar).Value = row[i].ToString().Replace("'", "''");
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

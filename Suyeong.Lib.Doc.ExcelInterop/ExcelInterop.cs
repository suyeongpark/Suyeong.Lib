using System;
using System.Data;
using System.Runtime.InteropServices;
using Excel = Microsoft.Office.Interop.Excel;

namespace Suyeong.Lib.Doc.ExcelInterop
{
    public static class ExcelInterop
    {
        public static DataSet LoadByDataSet(string filePath)
        {
            DataSet dataSet = new DataSet();

            Excel.Application application = null;
            Excel.Workbooks workbooks = null;
            Excel.Workbook workbook = null;
            Excel.Range range = null;

            try
            {
                application = new Excel.Application();
                workbooks = application.Workbooks;
                workbook = workbooks.Open(Filename: filePath, ReadOnly: true);

                foreach (Excel.Worksheet worksheet in workbook.Worksheets)
                {
                    range = worksheet.UsedRange;
                    dataSet.Tables.Add(GetWorksheet(tableName: worksheet.Name, range: range));
                }

                workbook.Close();
                application.Quit();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                ReleaseComObjects(new object[] { range, workbook, workbooks, application, });
            }

            return dataSet;
        }

        public static DataTable LoadByDataTable(string filePath, string sheetName)
        {
            DataTable table = new DataTable();

            Excel.Application application = null;
            Excel.Workbooks workbooks = null;
            Excel.Workbook workbook = null;
            Excel.Worksheet worksheet = null;
            Excel.Range range = null;

            try
            {
                application = new Excel.Application();
                workbooks = application.Workbooks;
                workbook = workbooks.Open(Filename: filePath, ReadOnly: true);
                worksheet = workbook.Worksheets[sheetName];
                range = worksheet.UsedRange;

                table = GetWorksheet(tableName: worksheet.Name, range: range);

                workbook.Close();
                application.Quit();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                ReleaseComObjects(new object[] { range, worksheet, workbook, workbooks, application, });
            }

            return table;
        }

        public static bool SaveByDataSet(DataSet dataSet, string filePath)
        {
            bool result = false;

            Excel.Application application = null;
            Excel.Workbooks workbooks = null;
            Excel.Workbook workbook = null;
            Excel.Worksheet worksheet = null;
            Excel.Range range = null;

            try
            {
                application = new Excel.Application();
                workbooks = application.Workbooks;
                workbook = workbooks.Add();

                DataTable table;
                int rowCount, columnCount;

                for (int i = 0; i < dataSet.Tables.Count; i++)
                {
                    table = dataSet.Tables[i];

                    worksheet = workbook.Worksheets.Add();

                    // title이 있다고 가정.
                    rowCount = table.Rows.Count + 1;
                    columnCount = table.Columns.Count;

                    range = worksheet.Range[worksheet.Cells[1, 1], worksheet.Cells[rowCount, columnCount]];
                    range.Value = ConvertData(rowCount: rowCount, columnCount: columnCount, table: table, hasTitle: true);
                }

                workbook.SaveAs(filePath, Excel.XlFileFormat.xlWorkbookDefault);

                workbook.Close();
                application.Quit();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                ReleaseComObjects(new object[] { worksheet, workbook, workbooks, application, });
            }

            return result;
        }

        public static bool SaveByDataTable(DataTable table, string filePath, bool hasTitle = true)
        {
            bool result = false;

            Excel.Application application = null;
            Excel.Workbooks workbooks = null;
            Excel.Workbook workbook = null;
            Excel.Worksheet worksheet = null;
            Excel.Range range = null;

            try
            {
                application = new Excel.Application();
                workbooks = application.Workbooks;
                workbook = workbooks.Add();
                worksheet = workbook.Worksheets.Add();

                // title이 있으면 첫 줄은 header로 쓴다.
                int rowCount = hasTitle ? table.Rows.Count + 1 : table.Rows.Count;
                int columnCount = table.Columns.Count;

                range = worksheet.Range[worksheet.Cells[1, 1], worksheet.Cells[rowCount, columnCount]];
                range.Value = ConvertData(rowCount: rowCount, columnCount: columnCount, table: table, hasTitle: hasTitle);

                workbook.SaveAs(filePath, Excel.XlFileFormat.xlWorkbookDefault);

                workbook.Close();
                application.Quit();

                result = true;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                ReleaseComObjects(new object[] { range, worksheet, workbook, workbooks, application, });
            }

            return result;
        }

        static DataTable GetWorksheet(string tableName, Excel.Range range)
        {
            DataTable table = new DataTable(tableName);

            if (range.Value != null)
            {
                object[,] value = range.Value;
                string[] values;
                int rowCount = value.GetLength(0);
                int columnCount = value.GetLength(1);

                for (int i = 0; i < columnCount; i++)
                {
                    table.Columns.Add(new DataColumn() { AllowDBNull = true });
                }

                for (int row = 0; row < rowCount; row++)
                {
                    values = new string[columnCount];

                    for (int column = 0; column < columnCount; column++)
                    {
                        values[column] = value[row + 1, column + 1]?.ToString();
                    }

                    table.Rows.Add(values);
                }
            }

            return table;
        }

        static object[,] ConvertData(int rowCount, int columnCount, DataTable table, bool hasTitle)
        {
            object[,] data = new object[rowCount, columnCount];

            if (hasTitle)
            {
                for (int columnIndex = 0; columnIndex < columnCount; columnIndex++)
                {
                    data[0, columnIndex] = table.Columns[columnIndex].ColumnName;
                }
            }

            DataRow row;
            int startIndex = hasTitle ? 1 : 0;  // 타이틀이 있다면 row를 1행씩 미룬다.
            int endIndex = hasTitle ? rowCount - 1 : rowCount;  // 타이틀이 있다면 실제 table row 보다 1줄 추가 했으므로 loop는 -1까지 돈다.

            for (int rowIndex = 0; rowIndex < endIndex; rowIndex++)
            {
                row = table.Rows[rowIndex];

                for (int columnIndex = 0; columnIndex < columnCount; columnIndex++)
                {
                    data[startIndex + rowIndex, columnIndex] = row.ItemArray[columnIndex];
                }
            }

            return data;
        }

        static bool ReleaseComObjects(object[] objects)
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
    }
}

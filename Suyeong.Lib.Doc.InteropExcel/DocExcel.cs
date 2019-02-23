using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.InteropServices;
using Excel = Microsoft.Office.Interop.Excel;

namespace Suyeong.Lib.Doc.InteropExcel
{
    public static class DocExcel
    {
        public static bool SaveExcelFromDataSet(DataSet dataSet, string filePath, bool hasTitle)
        {
            bool result = false;

            Excel.Application application = null;
            Excel.Workbook workbook = null;
            Excel.Worksheet worksheet = null;

            try
            {
                application = new Excel.Application();
                workbook = application.Workbooks.Add();

                for (int i = 0; i < dataSet.Tables.Count; i++)
                {
                    worksheet = workbook.Worksheets.Add();
                    result = SetWorksheetFromDataTable(worksheet: worksheet, table: dataSet.Tables[i], hasTitle: hasTitle);
                }

                workbook.SaveAs(filePath, Excel.XlFileFormat.xlWorkbookDefault);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (worksheet != null)
                {
                    ReleaseComObject(worksheet);
                }

                if (workbook != null)
                {
                    workbook.Close(SaveChanges: false);
                    ReleaseComObject(workbook);
                }

                if (application != null)
                {
                    application.Quit();
                    ReleaseComObject(application);
                }
            }

            return result;
        }

        public static bool SaveExcelFromDataTable(DataTable table, string filePath, bool hasTitle)
        {
            bool result = false;

            Excel.Application application = null;
            Excel.Workbook workbook = null;
            Excel.Worksheet worksheet = null;

            try
            {
                application = new Excel.Application();
                workbook = application.Workbooks.Add();
                worksheet = workbook.Worksheets.Add();

                result = SetWorksheetFromDataTable(worksheet: worksheet, table: table, hasTitle: hasTitle);

                workbook.SaveAs(filePath, Excel.XlFileFormat.xlWorkbookDefault);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (worksheet != null)
                {
                    ReleaseComObject(worksheet);
                }

                if (workbook != null)
                {
                    workbook.Close(SaveChanges: false);
                    ReleaseComObject(workbook);
                }

                if (application != null)
                {
                    application.Quit();
                    ReleaseComObject(application);
                }
            }

            return result;
        }

        public static DataSet GetDataSetFromExcel(string filePath)
        {
            DataSet dataSet = new DataSet();

            Excel.Application application = null;
            Excel.Workbook workbook = null;

            try
            {
                application = new Excel.Application();
                workbook = application.Workbooks.Open(Filename: filePath, ReadOnly: true);

                foreach (Excel.Worksheet workSheet in workbook.Worksheets)
                {
                    dataSet.Tables.Add(GetDataTableFromSheet(workSheet: workSheet));
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (workbook != null)
                {
                    workbook.Close(SaveChanges: false);
                    ReleaseComObject(workbook);
                }

                if (application != null)
                {
                    application.Quit();
                    ReleaseComObject(application);
                }
            }

            return dataSet;
        }

        public static DataTable GetDataTableFromExcel(string filePath, string sheetName)
        {
            DataTable table = new DataTable();

            Excel.Application application = null;
            Excel.Workbook workbook = null;
            Excel.Worksheet worksheet = null;

            try
            {
                application = new Excel.Application();
                workbook = application.Workbooks.Open(Filename: filePath, ReadOnly: true);
                worksheet = workbook.Worksheets[sheetName];

                table = GetDataTableFromSheet(workSheet: worksheet);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (worksheet != null)
                {
                    ReleaseComObject(worksheet);
                }

                if (workbook != null)
                {
                    workbook.Close(SaveChanges: false);
                    ReleaseComObject(workbook);
                }

                if (application != null)
                {
                    application.Quit();
                    ReleaseComObject(application);
                }
            }

            return table;
        }

        static DataTable GetDataTableFromSheet(Excel.Worksheet workSheet)
        {
            DataTable table = new DataTable();

            table.TableName = workSheet.Name;
            Excel.Range range = workSheet.UsedRange;

            string[] values;
            object[,] value = range.Value;
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

            return table;
        }

        static bool SetWorksheetFromDataTable(Excel.Worksheet worksheet, DataTable table, bool hasTitle)
        {
            bool result = false;

            try
            {
                // title이 있으면 첫 줄은 header로 쓴다.
                int rowCount = hasTitle ? table.Rows.Count + 1 : table.Rows.Count;
                int columnCount = table.Columns.Count;
                object[,] data = GetData(rowCount: rowCount, columnCount: columnCount, hasTitle: hasTitle, table: table);

                Excel.Range range = worksheet.Range[worksheet.Cells[1, 1], worksheet.Cells[rowCount, columnCount]];
                range.Value = data;

                result = true;
            }
            catch (Exception)
            {
                throw;
            }

            return result;
        }

        static object[,] GetData(int rowCount, int columnCount, bool hasTitle, DataTable table)
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

        static bool CheckTitle(DataTable table)
        {
            List<string> columns = new List<string>();

            foreach (DataColumn column in table.Columns)
            {
                columns.Add(column.ColumnName);
            }

            return !string.IsNullOrWhiteSpace(string.Join("", columns));
        }

        static void ReleaseComObject(object obj)
        {
            try
            {
                if (obj != null)
                {
                    Marshal.ReleaseComObject(obj);
                    obj = null;
                }
            }
            catch (Exception)
            {
                obj = null;
                throw;
            }
            finally
            {
                GC.Collect();
            }
        }
    }
}

using System;
using System.Data;
using System.Runtime.InteropServices;
using Excel = Microsoft.Office.Interop.Excel;

namespace Suyeong.Lib.Doc.InteropExcel
{
    public static class DocExcel
    {
        public static DataSet LoadByDataSet(string filePath)
        {
            DataSet dataSet = new DataSet();

            Excel.Application application = null;
            Excel.Workbooks workbooks = null;
            Excel.Workbook workbook = null;
            Excel.Worksheets worksheets = null;

            try
            {
                application = new Excel.Application();
                workbooks = application.Workbooks;
                workbook = workbooks.Open(Filename: filePath, ReadOnly: true);
                worksheets = workbook.Worksheets as Excel.Worksheets;

                foreach (Excel.Worksheet worksheet in worksheets)
                {
                    dataSet.Tables.Add(GetWorksheet(workSheet: worksheet));
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
                if (worksheets != null)
                {
                    Marshal.ReleaseComObject(worksheets);
                }

                if (workbook != null)
                {
                    Marshal.ReleaseComObject(workbook);
                }

                if (workbooks != null)
                {
                    Marshal.ReleaseComObject(workbooks);
                }

                if (application != null)
                {
                    Marshal.ReleaseComObject(application);
                }
            }

            return dataSet;
        }

        public static DataTable LoadByDataTable(string filePath, string sheetName)
        {
            DataTable table = new DataTable();

            Excel.Application application = null;
            Excel.Workbooks workbooks = null;
            Excel.Workbook workbook = null;
            Excel.Worksheets worksheets = null;
            Excel.Worksheet worksheet = null;

            try
            {
                application = new Excel.Application();
                workbooks = application.Workbooks;
                workbook = workbooks.Open(Filename: filePath, ReadOnly: true);
                worksheets = workbook.Worksheets as Excel.Worksheets;
                worksheet = worksheets[sheetName];

                table = GetWorksheet(workSheet: worksheet);

                workbook.Close();
                application.Quit();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (worksheet != null)
                {
                    Marshal.ReleaseComObject(worksheet);
                }

                if (worksheets != null)
                {
                    Marshal.ReleaseComObject(worksheets);
                }

                if (workbook != null)
                {
                    Marshal.ReleaseComObject(workbook);
                }

                if (workbooks != null)
                {
                    Marshal.ReleaseComObject(workbooks);
                }

                if (application != null)
                {
                    Marshal.ReleaseComObject(application);
                }
            }

            return table;
        }

        public static bool SaveByDataSet(DataSet dataSet, string filePath)
        {
            bool result = false;

            Excel.Application application = null;
            Excel.Workbooks workbooks = null;
            Excel.Workbook workbook = null;
            Excel.Worksheets worksheets = null;
            Excel.Worksheet worksheet = null;

            try
            {
                application = new Excel.Application();
                workbooks = application.Workbooks;
                workbook = workbooks.Add();
                worksheets = workbook.Worksheets as Excel.Worksheets;

                for (int i = 0; i < dataSet.Tables.Count; i++)
                {
                    worksheet = worksheets.Add();
                    result = SetWorksheet(worksheet: worksheet, table: dataSet.Tables[i]);
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
                if (worksheet != null)
                {
                    Marshal.ReleaseComObject(worksheet);
                }

                if (worksheets != null)
                {
                    Marshal.ReleaseComObject(worksheets);
                }

                if (workbook != null)
                {
                    Marshal.ReleaseComObject(workbook);
                }

                if (workbooks != null)
                {
                    Marshal.ReleaseComObject(workbooks);
                }

                if (application != null)
                {
                    Marshal.ReleaseComObject(application);
                }
            }

            return result;
        }

        public static bool SaveByDataTable(DataTable table, string filePath, bool hasTitle = true)
        {
            bool result = false;

            Excel.Application application = null;
            Excel.Workbooks workbooks = null;
            Excel.Workbook workbook = null;
            Excel.Worksheets worksheets = null;
            Excel.Worksheet worksheet = null;

            try
            {
                application = new Excel.Application();
                workbooks = application.Workbooks;
                workbook = workbooks.Add();
                worksheets = workbook.Worksheets as Excel.Worksheets;
                worksheet = worksheets.Add();

                result = SetWorksheet(worksheet: worksheet, table: table, hasTitle: hasTitle);

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
                if (worksheet != null)
                {
                    Marshal.ReleaseComObject(worksheet);
                }

                if (worksheets != null)
                {
                    Marshal.ReleaseComObject(worksheets);
                }

                if (workbook != null)
                {
                    Marshal.ReleaseComObject(workbook);
                }

                if (workbooks != null)
                {
                    Marshal.ReleaseComObject(workbooks);
                }

                if (application != null)
                {
                    Marshal.ReleaseComObject(application);
                }
            }

            return result;
        }

        static DataTable GetWorksheet(Excel.Worksheet workSheet)
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

        static bool SetWorksheet(Excel.Worksheet worksheet, DataTable table, bool hasTitle = true)
        {
            bool result = false;

            try
            {
                // title이 있으면 첫 줄은 header로 쓴다.
                int rowCount = hasTitle ? table.Rows.Count + 1 : table.Rows.Count;
                int columnCount = table.Columns.Count;
                object[,] data = ConvertData(rowCount: rowCount, columnCount: columnCount, hasTitle: hasTitle, table: table);

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

        static object[,] ConvertData(int rowCount, int columnCount, bool hasTitle, DataTable table)
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
    }
}
